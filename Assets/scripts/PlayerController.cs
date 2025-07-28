using System.Collections;
using UnityEngine;

/// <summary>
/// プレイヤーの移動・攻撃・アイテム取得などを管理するクラス
/// </summary>
public class PlayerController : MonoBehaviour
{
    public readonly float SPEED = 0.1f;

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

    float recaverTime = 0.0f;
    public float delay = 10.0f;
    private float startTime;

    [SerializeField] GameObject firePrefab;
    [SerializeField] Transform fireSpawnPoint;

    private bool canMove = true;

    bool IsStum()
    {
        return recaverTime > 0.0f;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        startTime = Time.time;
    }

    public int Life() => life;
    public int Fier() => fier;
    public int Key() => key;
    public int TakaraBox() => takaraBox;

    bool IsStun() => recaverTime > 0.0f || life <= 0;

    private float fireRecoveryTimer = 0f;
private float fireCooldown = 0.5f; // 強制的に戻る時間

    void Idle()
    {
        animator.SetBool("DizzyBool", false);
    }

void Update()
{
        // 移動キーが押されているかチェック
        bool isMoving = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
        
        if (!canMove)
    {
        fireRecoveryTimer += Time.deltaTime;
        if (fireRecoveryTimer >= fireCooldown)
        {
            canMove = true;
            animator.speed = 1f;
        }

        animator.speed = 0f;
        input = Vector2.zero;
        return;
    }

    input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

    if (IsStun())
    {
            
            recaverTime -= Time.deltaTime;
    }

    float speed = input.magnitude;
        animator.speed = Mathf.Max(0.5f, speed / 2.0f); // 最低速度を保証


        if (input.x != 0)
    {
        transform.localScale = new Vector3(Mathf.Sign(input.x), 1, 1);
            animator.SetBool("WalkBool", true);
        }
        else
        {
            animator.SetBool("WalkBool", false);
        }

        if (Time.time - startTime >= delay && Input.GetKeyDown(KeyCode.Space))
    {
        if (fier > 0)
        {
            canMove = false;
            fireRecoveryTimer = 0f;
            animator.SetTrigger("AttackTrigger");
            GameObject fire = Instantiate(firePrefab, fireSpawnPoint.position, Quaternion.identity);
            Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            fire.GetComponent<FireController>().Shoot(direction);
            fier--;
        }
    }
    Debug.Log("Animator current state: " + animator.GetCurrentAnimatorStateInfo(0).IsName("Fire"));


}

    void FixedUpdate()
    {
        if (Time.time - startTime < delay || IsStun() || input == Vector2.zero) return;
        rigidBody.position += input * SPEED;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsStun()) return;

        switch (collision.tag)
        {
            case "Monster":
                life--;
                Debug.Log("モンスターにぶつかった");
                
                
                if (this.Life() <= 0)
                {
                    animator.SetBool("DieBool", true);
                    
                }
                else
                {
                    animator.SetBool("DizzyBool", true);
                    Invoke("Idle", 1.0f);
                }

                    break;

            case "Key":
                key++;
                Destroy(collision.gameObject);
                Debug.Log("カギを取得した");
                break;

            case "TakaraBox":
                if (key > 0)
                {
                    takaraBox++;
                    key--;
                    Destroy(collision.gameObject);
                    Debug.Log("宝箱を開けた！");

                    if (takaraBox == 1 && key == 0)
                    {
                        GameObject.FindObjectOfType<GameController>()?.SendMessage("TriggerGameClear");
                    }
                }
                else
                {
                    UIManager.Instance.ShowMessage("鍵を持っていません！");
                }
                break;
        }
    }

    public void HpKaihuku()
    {
        life++;
        Debug.Log("HPを回復した");
    }

    public void FierKaihuku()
    {
        fier++;
        Debug.Log("ファイアポイントを回復した");
    }

    public void OnFireAnimationEnd()
    {
        Debug.Log("🔥 OnFireAnimationEnd called! → canMove = true");
        canMove = true;
        animator.speed = 1f;
    }
}
