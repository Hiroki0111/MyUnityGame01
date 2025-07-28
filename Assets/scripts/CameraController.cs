using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player;

    public float zoomTime = 2f;            // ズームする時間
    public float delayBeforeZoom = 5f;     // 開始前の待機時間
    public float targetSize = 5f;          // プレイヤーに近づいた後のカメラサイズ
    public float initialSize = 15f;        // 開始時のカメラサイズ

    private bool followPlayer = false;     // プレイヤー追従開始フラグ
    private Camera cam;

    void Start()
    {
        this.player = GameObject.Find("PlayerPrefab");
        cam = GetComponent<Camera>();
        cam.orthographicSize = initialSize;
        StartCoroutine(ZoomToPlayer());
    }

    IEnumerator ZoomToPlayer()
    {
        yield return new WaitForSeconds(delayBeforeZoom);

        float time = 0f;
        float startSize = cam.orthographicSize;

        while (time < zoomTime)
        {
            time += Time.deltaTime;
            float t = time / zoomTime;
            cam.orthographicSize = Mathf.Lerp(startSize, targetSize, t);

            Vector3 playerPos = this.player.transform.position;
            Vector3 targetPos = new Vector3(playerPos.x, playerPos.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, t);

            yield return null;
        }

        cam.orthographicSize = targetSize;
        followPlayer = true; // プレイヤー追従開始
    }

    void Update()
    {
        if (!followPlayer) return;

        Vector3 playerPos = this.player.transform.position;
        Vector3 targetPos = new Vector3(playerPos.x, playerPos.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 5f);
    }
}