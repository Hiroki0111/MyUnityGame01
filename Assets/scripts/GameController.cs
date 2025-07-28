using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public PlayerController player;
    public LifePanel lifePanel;
    public FierPanel fierPanel;
    public CoverObj coverObj;

    private void Start()
    {
        coverObj.FadeAlpha(0f); // ゲーム開始時にカバーを非表示にする

        Invoke(nameof(ShowCover), 5f); // 5秒後にカバーを表示（演出用）
        PlayerPrefs.SetString("CurrentStage", SceneManager.GetActiveScene().name);
    PlayerPrefs.Save();
    }

    private void ShowCover()
    {
        coverObj.FadeAlpha(1f); // カバーをフェードイン表示
    }

    void Update()
    {
        // プレイヤーの現在のライフや弾数をUIに反映
        lifePanel.UpdateLife(player.Life());
        fierPanel.UpdateLife(player.Fier());

        // ライフが0になったらゲームオーバー処理へ
        if (player.Life() <= 0)
        {
            enabled = false;

            // 今のシーン名を保存（後でリトライ用に使う）
            string currentScene = SceneManager.GetActiveScene().name;
            PlayerPrefs.SetString("LastSceneBeforeGameOver", currentScene);
            PlayerPrefs.Save();

            Invoke("GameOver", 2.0f);
        }

        // 鍵＆宝箱をゲットしていたらクリア
        if (player.Key() == 1 && player.TakaraBox() == 1)
        {
            enabled = false;
            Invoke("GameClear", 2.0f);
        }
    }

    void GameOver()
    {
         PlayerPrefs.SetString("LastPlayedStage", SceneManager.GetActiveScene().name);
    PlayerPrefs.Save();
    SceneManager.LoadScene("GameOver"); // ゲームオーバー画面へ
    }

    public void TriggerGameClear()
{
    enabled = false;

    int randomImageIndex = Random.Range(0, 21);
    PlayerPrefs.SetInt("ClearImage" + GetCurrentStageIndex(), randomImageIndex);
    PlayerPrefs.Save();

    SceneManager.LoadScene("GameClear");
}

// ステージ名から「1」「2」「3」などのインデックスを取得（例：1-2 → 2）
int GetCurrentStageIndex()
{
    string name = SceneManager.GetActiveScene().name;
    if (name.Contains("1-1")) return 1;
    if (name.Contains("1-2")) return 2;
    if (name.Contains("1-3")) return 3;
    return 0;
}

}
