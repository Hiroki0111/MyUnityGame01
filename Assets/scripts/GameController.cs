using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public PlayerController player;     // プレイヤー制御スクリプト
    public LifePanel lifePanel;         // ライフUI
    public FierPanel fierPanel;         // 弾UI
    public CoverObj coverObj;           // 画面フェード制御

    void Start()
    {
        coverObj.FadeAlpha(0f); // ゲーム開始時にフェードアウト（透明）

        // 現在のステージ名を保存（後で使う）
        PlayerPrefs.SetString("CurrentStage", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();

        // プレイヤーを動けない状態にしておく（演出中）
        player.canMove = false;

        // 画面演出として5秒後にカバー表示（フェードイン）
        Invoke(nameof(ShowCover), 5f);
    }

    // ★カメラ演出後に呼ばれるように public に変更
    public void EnablePlayerMovement()
    {
        player.EnableMovement(); // プレイヤーを動けるようにする
    }

    private void ShowCover()
    {
        coverObj.FadeAlpha(1f); // カバー画像をフェードイン表示（演出）
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

            // 今のシーン名を保存（リトライ用）
            string currentScene = SceneManager.GetActiveScene().name;
            PlayerPrefs.SetString("LastSceneBeforeGameOver", currentScene);
            PlayerPrefs.Save();

            Invoke("GameOver", 3.5f); // ゲームオーバー画面へ
        }

        // 鍵＆宝箱をゲットしていたらクリア
        if (player.Key() == 1 && player.TakaraBox() == 1)
        {
            enabled = false;
            Invoke("GameClear", 1f);
        }
    }

    void GameOver()
    {
        PlayerPrefs.SetString("LastPlayedStage", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
        
        GameObject.FindFirstObjectByType<CameraController>()?.SendMessage("blackout");
    }

    public void TriggerGameClear()
    {
        Debug.Log("トリガーclearが呼ばれた");
        Invoke("GameClear", 1.5f);

    }
    public void GameClear()
    {
        Debug.Log("Gameclearが呼ばれた");

        enabled = false;

        int randomImageIndex = Random.Range(0, 21);
        PlayerPrefs.SetInt("ClearImage" + GetCurrentStageIndex(), randomImageIndex);
        PlayerPrefs.Save();

        SceneManager.LoadScene("GameClear");
    }

    // ステージ名からインデックス番号を取得（例：1-2 → 2）
    int GetCurrentStageIndex()
    {
        string name = SceneManager.GetActiveScene().name;
        if (name.Contains("1-1")) return 1;
        if (name.Contains("1-2")) return 2;
        if (name.Contains("1-3")) return 3;
        return 0;
    }
}
