using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition instance;
    [SerializeField] Animator transitionAnim;
    [SerializeField] BoxCollider2D triggerBoxCollider;
    [SerializeField] string targetSceneName; // Add a field to specify the target scene name

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Trigger the "Start" animation at the beginning of the game
        transitionAnim.SetTrigger("Start");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            NextLevel();
        }
        else
        {
            // Trigger the "Start" animation when the player enters the box trigger
            transitionAnim.SetTrigger("Start");
        }
    }

    public void NextLevel()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(targetSceneName); // Load the target scene
        transitionAnim.SetTrigger("Start");
    }
}