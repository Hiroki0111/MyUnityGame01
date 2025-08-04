using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro を使うために必要な名前空間

public class BlinkerScript : MonoBehaviour
{
    // 点滅スピードを調整するためのパラメータ（Inspectorから変更可能）
    public float speed = 1.0f;

    // 経過時間を保持する変数（サイン波に使用）
    private float time;

    // このスクリプトがアタッチされている GameObject にある TextMeshProUGUI コンポーネントの参照
    private TextMeshProUGUI text;

    void Start()
    {
        // TextMeshProUGUI コンポーネントを取得
        // 取得できなければ text は null のままになる（Updateでチェックする）
        text = this.gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // text が null でない（＝正常に取得できている）場合のみ処理を実行
        if (text != null)
        {
            // 毎フレーム、テキストの透明度（アルファ値）を更新して点滅させる
            text.color = GetTextColorAlpha(text.color);
        }
    }

    /// <summary>
    /// 渡された色のアルファ値をサイン波に基づいて変化させ、点滅効果を出す
    /// </summary>
    /// <param name="color">元のテキストカラー</param>
    /// <returns>アルファ値を変化させたカラー</returns>
    Color GetTextColorAlpha(Color color)
    {
        // 経過時間に speed を掛けて変化の速さを調整
        time += Time.deltaTime * speed * 5.0f;

        // Mathf.Sin(time) は -1 〜 1 の範囲になるので、
        // +1 して 0 〜 2、さらに ÷2 して 0 〜 1 の範囲に収める（Colorのアルファ値は0〜1）
        color.a = (Mathf.Sin(time) + 1.0f) / 2.0f;

        return color;
    }
}
