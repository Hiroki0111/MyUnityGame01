using UnityEngine;
using UnityEngine.SceneManagement;

public class OPGame : MonoBehaviour
{
    private bool fireDetected = false;
    private float fireSpawnTime = 0f;

    void Update()
    {
        if (!fireDetected)
        {
            // Fireオブジェクトが存在するか確認
            GameObject fire = GameObject.FindGameObjectWithTag("Fire");
            if (fire != null)
            {
                fireDetected = true;
                fireSpawnTime = Time.time;
            }
        }
        else
        {
            // Fireが生成されてから3秒後に次のシーンへ
            if (Time.time - fireSpawnTime >= 3f)
            {
                SceneManager.LoadScene("OPScene02");
            }
        }
    }
}