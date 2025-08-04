using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum ItemType { Potion, FireKaihuku, Key, TakaraBox }

    public ItemType itemType;
    public GameObject audioPrefab; // AudioPrefabを参照

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (other.CompareTag("Player"))
        {
            switch (itemType)
            {
                case ItemType.Potion:
                    Instantiate(audioPrefab, transform.position, transform.rotation); // 効果音Prefabを生成
                    player?.HpKaihuku();
                    break;

                case ItemType.FireKaihuku:
                    Instantiate(audioPrefab, transform.position, transform.rotation); // 効果音Prefabを生成
                    player?.FierKaihuku();
                    break;

                case ItemType.Key:
                    Instantiate(audioPrefab, transform.position, transform.rotation); // 効果音Prefabを生成
                    break;

                case ItemType.TakaraBox:
                    // 鍵を持っていないと開けられない
                    if (player != null && player.Key() > 0)
                    {
                        Instantiate(audioPrefab, transform.position, transform.rotation); // 効果音Prefabを生成
                        // プレイヤーの鍵を1つ減らす（必要ならPlayerControllerに KeyUse() を追加）
                        typeof(PlayerController).GetMethod("UseKey", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                            ?.Invoke(player, null);
                    }
                    else
                    {
                        Debug.Log("カギがないので宝箱を開けられない");
                        return; // 宝箱を壊さない
                    }
                    break;
            }

            Destroy(gameObject); // アイテムを消去
        }
    }
}
