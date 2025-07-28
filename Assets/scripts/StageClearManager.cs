using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageClearManager : MonoBehaviour
{
    public Image clearImage;
    public Button nextButton;
    public Text messageText;
    public Text buttonText;  // ← ボタンの中の Text をここに指定！

    private int selectedImageIndex;

    void Start()
    {
        // 背景画像ランダム設定
        Sprite[] images = Resources.LoadAll<Sprite>("ClearImages");
        Debug.Log("画像数: " + images.Length);

        if (images.Length > 0 && clearImage != null)
        {
            selectedImageIndex = Random.Range(0, images.Length);
            clearImage.sprite = images[selectedImageIndex];
            clearImage.color = Color.white;
            Debug.Log("画像設定完了: " + images[selectedImageIndex].name);
        }

        // 現在ステージ取得と記録
        string currentStage = PlayerPrefs.GetString("CurrentStage", "1-1");
        Debug.Log("現在ステージ: " + currentStage);
        PlayerPrefs.SetInt(currentStage + "_ImageIndex", selectedImageIndex);
        PlayerPrefs.Save();

        // ボタンの表示変更（1-3 なら全クリアに進む）
        if (currentStage == "1-3")
        {
            buttonText.text = "全クリアを見る";
        }
        else
        {
            buttonText.text = "つぎへ";
        }

        nextButton.onClick.AddListener(OnNextButtonClicked);
    }

    void OnNextButtonClicked()
    {
        string currentStage = PlayerPrefs.GetString("CurrentStage", "1-1");
        string nextStage = "";

        switch (currentStage)
        {
            case "1-1": nextStage = "1-2"; break;
            case "1-2": nextStage = "1-3"; break;
            case "1-3":
                SceneManager.LoadScene("AllClear");
                return;
            default:
                nextStage = "1-1"; break;
        }

        PlayerPrefs.SetString("CurrentStage", nextStage);
        PlayerPrefs.Save();
        SceneManager.LoadScene(nextStage);
    }
}
