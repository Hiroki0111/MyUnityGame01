using UnityEngine;

public class Play_Audio : MonoBehaviour
{
    public GameObject audioPrefab; // AudioPrefabを参照

    void OnCollisionEnter(Collision collision)
    {
        Instantiate(audioPrefab, transform.position, transform.rotation); // 効果音Prefabを生成
        Destroy(gameObject); // 元のオブジェクトを破壊
    }
}