using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    public static float elapsedTime; // Variabel static untuk menyimpan nilai waktu

    private void Start()
    {
        // Jika elapsedTime masih 0, maka game baru dimulai
        if (elapsedTime == 0)
        {
            elapsedTime = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Fungsi untuk reset waktu ketika game over
    public static void ResetTime()
    {
        elapsedTime = 0;
    }
}