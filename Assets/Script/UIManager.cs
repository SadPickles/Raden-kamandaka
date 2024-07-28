using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Add this line

public class UIManager : MonoBehaviour
{
    public GameObject gameOverMenu;

    private void OnEnable()
    {
        Health.OnPlayerDead += EnableGameOverMenu;
        CheckpointManager.OnPlayerReachCheckpoint += SaveCheckpoint;
    }

    private void OnDisable()
    {
        Health.OnPlayerDead -= EnableGameOverMenu;
        CheckpointManager.OnPlayerReachCheckpoint -= SaveCheckpoint;
    }

    public void EnableGameOverMenu()
    {
        gameOverMenu.SetActive(true);
    }

    public void RestartLevel()
    {
        if (CheckpointManager.lastCheckpointPosition != Vector3.zero)
        {
            Debug.Log("Restarting from checkpoint: " + CheckpointManager.lastCheckpointPosition);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            SceneManager.sceneLoaded += (scene, loadSceneMode) =>
            {
                Debug.Log("Scene loaded: " + scene.name);
                var player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    Debug.Log("Player found: " + player.name);
                    player.transform.position = CheckpointManager.lastCheckpointPosition;
                    Debug.Log("Player position set to: " + player.transform.position);
                }
                else
                {
                    
                }
            };
        }
        else
        {
            Debug.Log("No checkpoint available, restarting from beginning");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void SaveCheckpoint(Vector3 position)
    {
        CheckpointManager.lastCheckpointPosition = position;
    }
}