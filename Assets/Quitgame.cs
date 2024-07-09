using UnityEngine;
using UnityEngine.UI;

public class Quitgame : MonoBehaviour
{
    public Button quitButton; // Assign your quit button in the Inspector

    void Start()
    {
        quitButton.onClick.AddListener(QuitGameFunction);
    }

    void QuitGameFunction()
    {
        Application.Quit();
    }
}