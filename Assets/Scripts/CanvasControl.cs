using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasControl : MonoBehaviour
{
    public GameObject pauseMenuCanvas;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            pauseMenuCanvas.SetActive(isPaused);
            if (isPaused)
            {
                Time.timeScale = 0f; 
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }
}
