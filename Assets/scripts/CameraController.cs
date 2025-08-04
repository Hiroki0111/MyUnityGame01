using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    GameObject player;
    GameController gameController; // ★GameController 参照を追加

    public float zoomTime = 2f;            // ズームする時間
    public float delayBeforeZoom = 5f;     // ズーム開始前の待機時間
    public float targetSize = 5f;          // プレイヤーに近づいた後のカメラサイズ
    public float initialSize = 15f;        // 開始時のカメラサイズ

    private bool followPlayer = false;     // プレイヤー追従開始フラグ
    private Camera cam;

    // ★ START! を表示するText（TextMeshPro）
    [SerializeField] private GameObject startTextObject;

    void Start()
    {
        player = GameObject.Find("PlayerPrefab");
        gameController = FindObjectOfType<GameController>();
        cam = GetComponent<Camera>();
        cam.orthographicSize = initialSize;

        // 念のため START! テキストを非表示にしておく
        if (startTextObject != null) startTextObject.SetActive(false);

        StartCoroutine(ZoomToPlayer()); // カメラズーム開始
    }

    IEnumerator ZoomToPlayer()
    {
        yield return new WaitForSeconds(delayBeforeZoom); // ズーム開始前の待機

        float time = 0f;
        float startSize = cam.orthographicSize;

        while (time < zoomTime)
        {
            time += Time.deltaTime;
            float t = time / zoomTime;

            // カメラのズーム処理
            cam.orthographicSize = Mathf.Lerp(startSize, targetSize, t);

            // プレイヤー位置へ徐々に移動
            Vector3 playerPos = this.player.transform.position;
            Vector3 targetPos = new Vector3(playerPos.x, playerPos.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, t);

            yield return null;
        }

        // ズーム完了時に最終値を補正
        cam.orthographicSize = targetSize;
        followPlayer = true; // カメラ追従ON

        // ★ズーム完了時にプレイヤー操作を許可
        if (gameController != null)
        {
            gameController.EnablePlayerMovement();
        }
        // ★ START! 表示
        if (startTextObject != null)
        {
            startTextObject.SetActive(true);
            yield return new WaitForSeconds(3f);
            startTextObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!followPlayer) return;

        // カメラをプレイヤーに追従
        Vector3 playerPos = this.player.transform.position;
        Vector3 targetPos = new Vector3(playerPos.x, playerPos.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 5f);
    }

    [SerializeField] private Image _PanelImage;
    [SerializeField] private float _speed;
    private bool isSceneChange;
    private Color PanelColor;
    private void Awake()
    {
        isSceneChange = false;
        PanelColor = _PanelImage.color;
    }
    public void blackout()
    {
        StartCoroutine(Sceneblackout());
    }
    private IEnumerator Sceneblackout()
    {
        while (!isSceneChange)
        {
            PanelColor.a += 0.1f;
            _PanelImage.color = PanelColor;
            if (PanelColor.a >= 1)
                isSceneChange = true;
            yield return new WaitForSeconds(_speed);
        }
        SceneManager.LoadScene("GameOver");
    }
}
