using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static Vector3 lastCheckpointPosition;
    public static event System.Action<Vector3> OnPlayerReachCheckpoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            lastCheckpointPosition = other.transform.position;
            OnPlayerReachCheckpoint?.Invoke(lastCheckpointPosition);
        }
    }
}