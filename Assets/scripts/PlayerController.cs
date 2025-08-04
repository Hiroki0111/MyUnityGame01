using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public readonly float SPEED = 0.5f; // 移動速度の基準値
    public float jumpForce = 1.5f;      // ノックバック力

    public Rigidbody2D rb;
    private Rigidbody2D rigidBody;

    private Vector2 input;
    private Animator animator;

    const int DefaultLife = 3;
    const int DefaultFier = 5;
    const int DefaultKey = 0;
    const int DefaultTakaraBox = 0;

    int life = DefaultLife;
    int fier = DefaultFier;
    int key = DefaultKey;
    int takaraBox = DefaultTakaraBox;

    int honoodasenai = 0; // 炎攻撃を制限するカウンタ

    public float clearugoken = 5.0f;

    float recaverTime = 0.0f;
    public float stunDuration = 1.0f;            // スタン時間（被弾直後の硬直）
    public float invincibilityDuration = 1.0f;   // 無敵時間（ダメージ再発防止）
    private bool isInvincible = false;

    public float delay = 1.0f; // ゲーム開始時の待機時間
    private float startTime;

    [SerializeField] GameObject firePrefab;
    [SerializeField] Transform fireSpawnPoint;

    private bool canAttack = true;      // 攻撃可能フラグ
    private float fireCooldown = 0.5f;  // 炎の連射間隔

    public bool canMove = true;         // 移動可能フラグ（外部からアクセス可能に修正）

    [SerializeField] private AudioSource a;
    [SerializeField] private AudioClip b;
    [SerializeField] private AudioClip teki;
    [SerializeField] private AudioClip shibou;

    private bool isDead = false; // 死亡状態フラグ追加

    public GameObject audioPrefab; // AudioPrefabを参照

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        startTime = Time.time;
    }

    // 各ステータスの取得関数
    public int Life() => life;
    public int Fier() => fier;
    public int Key() => key;
    public int TakaraBox() => takaraBox;

    // スタン状態か判定
    bool IsStun() => recaverTime > 0.0f || life <= 0;

    // アニメーションIdleへ戻す
    void Idle() => animator.SetBool("DizzyBool", false);

    void Update()
    {
        // 死亡アニメーションが終わったらDieBoolをfalseに戻す処理
        if (isDead)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Die") && stateInfo.normalizedTime >= 1f)
            {
                animator.SetBool("DieBool", false);
                isDead = false; 
                Invoke("Tamashiinokoru", 1f);

                // ここで死亡アニメーション終了後の処理を追加可能
                // 例: ゲームオーバー画面の表示など
            }
            return; // 死亡中はそれ以外の操作を受け付けない
        }

        if (!IsStun())
        {
            input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
        else
        {
            input = Vector2.zero;
            recaverTime -= Time.deltaTime;
        }

        float speed = input.magnitude;
        animator.speed = Mathf.Max(0.5f, speed / 2.0f);

        // 向きと歩きアニメーション
        if (input.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(input.x), 1, 1);
            animator.SetBool("WalkBool", true);
        }
        else if (input.y != 0)
        {
            animator.SetBool("WalkBool", true);
        }
        else
        {
            animator.SetBool("WalkBool", false);
        }

        // 攻撃処理（スタン中やクールダウン中は禁止）
        if (honoodasenai == 0 && canAttack && !IsStun() && Time.time - startTime >= delay && Input.GetKeyDown(KeyCode.Space))
        {
            if (fier > 0)
            {
                canAttack = false;
                animator.SetTrigger("AttackTrigger");

                GameObject fire = Instantiate(firePrefab, fireSpawnPoint.position, Quaternion.identity);
                Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
                fire.GetComponent<FireController>().Shoot(direction);

                fier--;
                a.PlayOneShot(b);
                Invoke(nameof(EnableAttack), fireCooldown);
            }
        }
    }

    void FixedUpdate()
    {
        if (Time.time - startTime < delay || IsStun() || !canMove || isDead)
        {
            rigidBody.velocity = Vector2.zero;
            return;
        }

        rigidBody.velocity = input.normalized * SPEED * 7f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsStun() || isInvincible || isDead) return;

        switch (collision.tag)
        {
            case "Monster":
                life--;
                a.PlayOneShot(teki);

                // スタン処理
                recaverTime = stunDuration;
                animator.SetBool("DizzyBool", true);
                Invoke(nameof(Idle), stunDuration);

                // 攻撃禁止状態へ（スタン解除後も＋1秒攻撃禁止）
                honoodasenai++;
                Invoke(nameof(Hoge), stunDuration + 1.0f);

                // ノックバック
                Vector2 jumpDir = (transform.position - collision.transform.position).normalized;
                rb.velocity = Vector2.zero;
                rb.AddForce(jumpDir * jumpForce, ForceMode2D.Impulse);

                // 死亡処理
                if (life <= 0)
                {
                    GameObject.FindFirstObjectByType<AutoMove>()?.SendMessage("Tenshi");
                    a.PlayOneShot(shibou);
                    animator.SetBool("DieBool", true);
                    isDead = true; // 死亡フラグON
                }

                // 無敵時間の付与
                StartCoroutine(InvincibilityFrames());
                break;

            case "Key":
                key++;
                Destroy(collision.gameObject);
                break;

            case "TakaraBox":
                if (key > 0)
                {
                    Instantiate(audioPrefab, transform.position, transform.rotation); // 効果音Prefabを生成
                    honoodasenai++;
                    recaverTime = clearugoken;
                    takaraBox++;
                    key--;
                    Destroy(collision.gameObject);

                    if (takaraBox == 1 && key == 0)
                    {
                        GameObject.FindFirstObjectByType<GameController>()?.SendMessage("TriggerGameClear");
                    }
                }
                else
                {
                    UIManager.Instance.ShowMessage("鍵を持っていません！");
                }
                break;
        }
    }

    // HP回復
    const int MaxLife = 3;

    public void HpKaihuku()
    {
        if (life < MaxLife)
        {
            life++;
        }
    }

    // 炎（Fier）回復
    const int MaxFier = 5; // 上限を明示して定義

    public void FierKaihuku()
    {
        if (fier < MaxFier)
        {
            fier++;
        }
    }

    // 攻撃再許可
    void EnableAttack() => canAttack = true;

    // 無敵状態を付与して時間経過後に解除
    IEnumerator InvincibilityFrames()
    {
        isInvincible = true;
        canMove = false;
        yield return new WaitForSeconds(invincibilityDuration);
        canMove = true;
        isInvincible = false;
    }

    // honoodasenaiを1減らして攻撃再許可
    void Hoge() => honoodasenai--;

    // 外部スクリプトから移動を許可する関数（GameControllerなどで使用可能）
    public void EnableMovement()
    {
        canMove = true;
    }

    // 攻撃アニメーション終了時に移動許可（Animatorイベントから呼び出される）
    public void OnFireAnimationEnd()
    {
        canMove = true;
        animator.speed = 1f;
    }
    void Tamashiinokoru()
    {
        gameObject.SetActive(false);
    }
}
