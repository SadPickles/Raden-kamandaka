using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private GameObject dialogueCanvas;

    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private TMP_Text dialogueText;

    [SerializeField] private Image portraitImage;

    [SerializeField] private string[] speaker;

    [SerializeField][TextArea] private string[] dialogueWords;

    [SerializeField] private Sprite[] portrait;

    private bool dialogueActivated;
    private int step;

    private PlayerController playerController;
    private PlayerCombat playerCombat;
    private Animator playerAnimator;

    private BoxCollider2D boxCollider;

    public string nextScene; // The name of the next scene (optional)

    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerCombat = GameObject.FindWithTag("Player").GetComponent<PlayerCombat>();
        playerAnimator = GameObject.FindWithTag("Player").GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && dialogueActivated == true)
        {
            if (step >= speaker.Length)
            {
                DisableDialogue();
                if (!string.IsNullOrEmpty(nextScene))
                {
                    LoadNextScene();
                }
            }
            else
            {
                if (speakerText != null)
                {
                    speakerText.text = speaker[step];
                }
                if (dialogueText != null)
                {
                    dialogueText.text = dialogueWords[step];
                }
                if (portraitImage != null)
                {
                    portraitImage.sprite = portrait[step];
                }
                step += 1;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            dialogueActivated = true;
            if (playerController != null)
            {
                playerController.enabled = false;
            }
            if (playerCombat != null)
            {
                playerCombat.enabled = false;
            }
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                playerRigidbody.velocity = Vector2.zero;
            }
            if (dialogueCanvas != null)
            {
                dialogueCanvas.SetActive(true);
            }
            step = 0;
            if (speakerText != null)
            {
                speakerText.text = speaker[step];
            }
            if (dialogueText != null)
            {
                dialogueText.text = dialogueWords[step];
            }
            if (portraitImage != null)
            {
                portraitImage.sprite = portrait[step];
            }
            if (playerAnimator != null)
            {
                playerAnimator.SetBool("IsIdle", true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        dialogueActivated = false;
        if (dialogueCanvas != null && dialogueCanvas.activeSelf)
        {
            dialogueCanvas.SetActive(false);
        }
        if (playerController != null)
        {
            playerController.enabled = true;
        }
        if (playerCombat != null)
        {
            playerCombat.enabled = true;
        }
        Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector2.zero;
        }
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("IsIdle", false);
        }
    }

    private void DisableDialogue()
    {
        if (dialogueCanvas != null)
        {
            dialogueCanvas.SetActive(false);
        }
        step = 0;
        if (playerController != null)
        {
            playerController.enabled = true;
        }
        if (playerCombat != null)
        {
            playerCombat.enabled = true;
        }
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("IsIdle", false);
        }
    }
    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextScene))
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}