using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class OPPlayerController : MonoBehaviour
{
    public readonly float SPEED = 0.1f;
    private Rigidbody2D rigidBody;
    private Vector2 input;
    const int DefaultLife = 3;
    const int DefaultFier = 5;
    const int DefaultKey = 0;
    const int DefaultTakaraBox = 0;

    int life = DefaultLife;
    int fier = DefaultFier;
    int key = DefaultKey;
    int takaraBox = DefaultTakaraBox;
    float recaverTime = 0.0f;
    Animator animator;

    public float delay = 10.0f;
    private float startTime;

    [SerializeField] GameObject firePrefab;      // ファイアのプレハブ
    [SerializeField] Transform fireSpawnPoint;   // 発射位置

    void Start()
    {
        this.animator = GetComponent<Animator>();
        this.rigidBody = GetComponent<Rigidbody2D>();
        this.rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public int Life() => life;
    public int Fier() => fier;
    public int Key() => key;
    public int TakaraBox() => takaraBox;

    bool IsStun() => recaverTime > 0.0f || life <= 0;

    private void Update()
    {
        input = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical"));

        if (IsStun())
        {
            recaverTime -= Time.deltaTime;
        }

        float speed = input.magnitude;
        this.animator.speed = speed / 2.0f;

        if (input.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(input.x), 1, 1);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (fier > 0)
            {
                GameObject fire = Instantiate(firePrefab, fireSpawnPoint.position, Quaternion.identity);
                Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
                fire.GetComponent<FireController>().Shoot(direction);
                fier--;
            }
        }
    }

    private void FixedUpdate()
    {
        if (Time.time - startTime < delay) return;
        if (IsStun()) return;
        if (input == Vector2.zero) return;

        rigidBody.position += input * SPEED;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsStun()) return;

        if (collision.gameObject.tag == "Monster")
        {
            life--;
            Debug.Log("モンスターにぶつかった");
        }
        else if (collision.gameObject.tag == "Key")
        {
            key++;
            Debug.Log("カギを取得した");
        }
        else if (collision.gameObject.tag == "TakaraBox")
        {
            takaraBox++;
            Debug.Log("宝箱を取得した");
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
}
