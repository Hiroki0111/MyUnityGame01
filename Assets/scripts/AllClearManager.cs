using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AllClearManager : MonoBehaviour
{
    public Image image1;
    public Image image2;
    public Image image3;

    public Text titleText;
    public Button returnButton;
    public Text buttonText;
    public Text messageText;

    void Start()
{
    if (buttonText == null && returnButton != null)
    {
        buttonText = returnButton.GetComponentInChildren<Text>();
        if (buttonText == null)
        {
            Debug.LogError("returnButtonの子にTextコンポーネントが見つかりません！");
        }
        else
        {
            Debug.Log("buttonText を自動割り当てしました: " + buttonText.name);
        }
    }

        Sprite[] images = Resources.LoadAll<Sprite>("ClearImages");
        Debug.Log("AllClearScene 読み込み画像数: " + images.Length);

        int idx1 = PlayerPrefs.GetInt("1-1_ImageIndex", 0);
        int idx2 = PlayerPrefs.GetInt("1-2_ImageIndex", 0);
        int idx3 = PlayerPrefs.GetInt("1-3_ImageIndex", 0);

        if (images.Length > idx1) image1.sprite = images[idx1];
        if (images.Length > idx2) image2.sprite = images[idx2];
        if (images.Length > idx3) image3.sprite = images[idx3];

        // 透明度対策
        image1.color = Color.white;
        image2.color = Color.white;
        image3.color = Color.white;

        titleText.text = "全ステージクリア！";
        buttonText.text = "タイトルに戻る";

        returnButton.onClick.AddListener(() =>
{
    Debug.Log("戻るボタンが押されました！");
    PlayerPrefs.DeleteKey("CurrentStage");
    SceneManager.LoadScene("OPScene01");
});
    }
}
