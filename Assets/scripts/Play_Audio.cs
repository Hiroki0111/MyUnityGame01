using UnityEngine;

public class Play_Audio : MonoBehaviour
{
    public GameObject audioPrefab; // AudioPrefab���Q��

    void OnCollisionEnter(Collision collision)
    {
        Instantiate(audioPrefab, transform.position, transform.rotation); // ���ʉ�Prefab�𐶐�
        Destroy(gameObject); // ���̃I�u�W�F�N�g��j��
    }
}