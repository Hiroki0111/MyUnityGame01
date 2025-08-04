using UnityEngine;

/// <summary>
/// オブジェクトを指定されたポイント順に移動させるスクリプト
/// </summary>
public class AutoMove : MonoBehaviour
{
    [SerializeField] private Transform[] points;  // 移動ポイント
    [SerializeField] private float speed = 0.5f;    // 移動速度（Inspectorで調整可）

    private Rigidbody2D rb;
    private int currentIndex = 0;
    private Transform target;
    private bool isMoving = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// 移動開始（外部から呼ぶ）
    /// </summary>
    public void Tenshi()
    {
        if (points == null || points.Length == 0)
        {
            Debug.LogWarning("移動ポイントが設定されていません");
            return;
        }
        currentIndex = 0;
        target = points[currentIndex];
        isMoving = true;
    }

    private void FixedUpdate()
    {
        if (!isMoving || target == null)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 currentPos = transform.position;
        Vector2 targetPos = target.position;
        Vector2 direction = (targetPos - currentPos).normalized;

        float distance = Vector2.Distance(currentPos, targetPos);

        // 減速処理（距離に応じて速度制御）
        float moveSpeed = Mathf.Min(speed, distance * 5f);
        rb.velocity = direction * moveSpeed;

        if (distance <= 0.3f)
        {
            // 位置を正確に合わせる
            transform.position = target.position;

            currentIndex++;
            if (currentIndex >= points.Length)
            {
                rb.velocity = Vector2.zero;
                isMoving = false;
                target = null;
            }
            else
            {
                target = points[currentIndex];
            }
        }
    }

}
