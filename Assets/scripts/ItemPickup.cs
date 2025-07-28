using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum ItemType { Potion, FireKaihuku, Key, TakaraBox }

    public ItemType itemType;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (other.CompareTag("Player"))
        {
            switch (itemType)
            {
                case ItemType.Potion:
                    player?.HpKaihuku();
                    break;

                case ItemType.FireKaihuku:
                    player?.FierKaihuku();
                    break;

                case ItemType.Key:
                    // 鍵を増やす
                    Debug.Log("カギを取得した");
                    break;

                case ItemType.TakaraBox:
                    // 鍵を持っていないと開けられない
                    if (player != null && player.Key() > 0)
                    {
                        Debug.Log("宝箱を開けた！");
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
