using System.Collections;
using UnityEngine;

public class EnemyController02 : MonoBehaviour
{
    public int hp = 1; // モンスターの体力
    public float moveSpeed = 2f; // 移動速度

    [SerializeField] private GameObject[] dropItems; // ドロップアイテムのプレハブ

    private Rigidbody2D rb;
    private int moveDirection = -1; // 初期の移動方向（-1=下向き）
    private Vector3 baseScale; // モンスターの初期スケール
    public GameObject audioPrefab; // やられたときのサウンド

    public GameObject player; // プレイヤーの参照（使わないなら削除してもOK）

    void Start()
    {
        // Rigidbody2D を取得
        rb = GetComponent<Rigidbody2D>();

        // スケールを取得しておく（反転させる場合用）
        baseScale = new Vector3(transform.localScale.x, Mathf.Abs(transform.localScale.y), transform.localScale.z);
    }

    void Update()
    {
        // Rigidbody の velocity を使って上下に移動（x=固定、y方向だけ動く）
        rb.velocity = new Vector2(rb.velocity.x, moveDirection * moveSpeed);

        transform.localScale = new Vector3(baseScale.x * -moveDirection, baseScale.y, baseScale.z);

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 上下の壁やトリガーにぶつかったら移動方向を反転
        if (collision.CompareTag("Wall"))
        {
            moveDirection *= -1;
        }

        // プレイヤーに当たったときも反転（必要なければこの処理は削除してOK）
        if (collision.CompareTag("Player"))
        {
            moveDirection *= -1;
        }
    }

    public void TakeDamage()
    {
        hp--;

        if (hp <= 0)
        {
            // やられた音を再生
            Instantiate(audioPrefab, transform.position, transform.rotation);

            // アイテムをドロップ
            DropItem();

            // 自身を削除
            Destroy(gameObject);
        }
    }

    private void DropItem()
    {
        if (dropItems.Length == 0) return;

        // ランダムにアイテムを1つ選ぶ
        int index = Random.Range(0, dropItems.Length);

        // 少し下にずらしてドロップ
        Vector3 dropPos = transform.position + new Vector3(0, -0.3f, 0);
        Instantiate(dropItems[index], dropPos, Quaternion.identity);
    }
}
