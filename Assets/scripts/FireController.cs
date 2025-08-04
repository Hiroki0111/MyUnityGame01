using UnityEngine;

public class FireController : MonoBehaviour
{
    public float speed = 10.0f;        // 弾の速度
    public float lifeTime = 2.0f;      // 弾の寿命（秒）

    private Vector2 direction = Vector2.right;

    // 初期化用メソッド（プレイヤーが呼び出す）
    public void Shoot(Vector2 dir)
    {
        direction = dir.normalized;

        // 向きに応じてスプライトを反転
        if (direction.x < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        Destroy(gameObject, lifeTime); // 一定時間後に自動で削除
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage();  // ダメージを与える
            }
            EnemyController02 enemy02 = collision.GetComponent<EnemyController02>();
            if (enemy02 != null)
            {
                enemy02.TakeDamage();
            }
            Destroy(gameObject);  // 弾は消す
        }
        else if (!collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
