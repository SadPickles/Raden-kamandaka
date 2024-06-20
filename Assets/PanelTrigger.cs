using UnityEngine;

public class PanelTrigger : MonoBehaviour
{
    public GameObject panel; // assign your panel game object in the inspector

    void OnTriggerEnter2D(Collider2D other)
    {
        // check if the collider that entered the trigger is the one you want to trigger the panel
        if (other.gameObject.CompareTag("Player"))
        {
            // show the panel
            panel.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // check if the collider that exited the trigger is the one you want to hide the panel
        if (other.gameObject.CompareTag("Player"))
        {
            // hide the panel
            panel.SetActive(false);
        }
    }
}