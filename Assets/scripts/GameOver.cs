using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 最後にプレイしていたシーン名を取得して戻る
            string lastStage = PlayerPrefs.GetString("LastPlayedStage", "Main");
            SceneManager.LoadScene(lastStage);
        }
    }
}
