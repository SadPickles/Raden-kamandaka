using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour
{
    public string targetScene; // nama scene yang ingin dituju

    void OnTriggerEnter2D(Collider2D col)
    {
        // cek apakah collider yang memasuki area trigger adalah pemain
        if (col.gameObject.CompareTag("Player"))
        {
            // berpindah ke scene yang ditentukan
            SceneManager.LoadScene(targetScene);
        }
    }
}