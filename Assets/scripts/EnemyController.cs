using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int hp = 1;
    public float moveSpeed = 2f;

    [SerializeField] private GameObject[] dropItems;  // アイテムプレハブ配列

    private Rigidbody2D rb;
    private int moveDirection = -1; // ← 左向きでスタート
    private Vector3 baseScale;
    public GameObject audioPrefab; // AudioPrefabを参照

    public GameObject player;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        baseScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

    }

    void Update()
    {
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
        transform.localScale = new Vector3(baseScale.x * -moveDirection, baseScale.y, baseScale.z);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            moveDirection *= -1;
        }
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
            Instantiate(audioPrefab, transform.position, transform.rotation);
            DropItem();
            Destroy(gameObject);

        }
    }

    private void DropItem()
    {

        if (dropItems.Length == 0) return;

        int index = Random.Range(0, dropItems.Length);

        Vector3 dropPos = transform.position + new Vector3(0, -0.3f, 0);  // 少し下にずらす
        Instantiate(dropItems[index], dropPos, Quaternion.identity);

    }
}
