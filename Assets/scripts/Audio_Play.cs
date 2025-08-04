using UnityEngine;

public class Audio_Play : MonoBehaviour
{
    private AudioSource audioSource; // AudioSourceを取得
    bool isAudioPlaying = false;   // 再生中の判定

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();        // 効果音を再生
        isAudioPlaying = true;    // 再生中フラグを設定
    }

    void Update()
    {
        if (!audioSource.isPlaying && isAudioPlaying)
        {
            Destroy(gameObject);  // 効果音再生後にオブジェクトを削除
        }
    }
}