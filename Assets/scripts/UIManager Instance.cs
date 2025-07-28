using System.Collections;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private TextMeshProUGUI messageText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowMessage(string message, float duration = 2f)
    {
        if (messageText == null)
        {
            Debug.LogError("messageText が割り当てられていません！");
            return;
        }

        messageText.text = message;
        StartCoroutine(ShowMessageCoroutine(duration));
    }

    private IEnumerator ShowMessageCoroutine(float duration)
    {
        messageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        messageText.gameObject.SetActive(false);
    }
}
