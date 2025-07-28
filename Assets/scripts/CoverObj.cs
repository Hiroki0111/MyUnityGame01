using UnityEngine;

public class CoverObj : MonoBehaviour
{
    public float targetAlpha = 0.5f;
    public float duration = 1f;

    private SpriteRenderer _spriteRenderer;
    private float _startAlpha;
    private float _timer = 0f;
    private bool _isFading = false;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found.");
            enabled = false;
            return;
        }

        // 最初の透明化（オプション）
        Color color = _spriteRenderer.color;
        color.a = 0f;
        _spriteRenderer.color = color;

        _startAlpha = color.a;
    }

    void Update()
    {
        if (_isFading)
        {
            _timer += Time.deltaTime;
            float t = Mathf.Clamp01(_timer / duration);
            Color color = _spriteRenderer.color;
            color.a = Mathf.Lerp(_startAlpha, targetAlpha, t);
            _spriteRenderer.color = color;

            if (t >= 1f)
            {
                _isFading = false;
                _timer = 0f;
            }
        }
    }

    public void FadeAlpha(float newAlpha)
    {
        if (_spriteRenderer == null) return;

        // 変化がある場合だけ開始
        if (!Mathf.Approximately(_spriteRenderer.color.a, newAlpha))
        {
            _startAlpha = _spriteRenderer.color.a;
            targetAlpha = newAlpha;
            _isFading = true;
            _timer = 0f; // ←忘れがち！
        }
    }

}
