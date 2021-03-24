using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    [SerializeField] public GameObject pauseUI;
    public GameObject visorCanvas;
    
    

    [SerializeField] private bool isPaused;
    private bool talking = false;
    
    

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!talking)
            {
                isPaused = !isPaused;
            }
        }

        if (isPaused)
        {
            OpenPause();
        } else
        {
            ClosePause();
        }
        
    }
    public void OpenPause()
    {
        pauseUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        isPaused = true;
        visorCanvas.SetActive(false);
       
    }

    public void ClosePause()
    {
        pauseUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        isPaused = false;
        visorCanvas.SetActive(true);
       
    }

    public void ConvoStatus(bool convo)
    {

        talking = convo;

    }
}
