using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour {


    public static bool isPaused;
    

    [SerializeField]
    private GameObject pauseMenuUI;
    [SerializeField]
    private GameObject outOfTimeUI;
    [SerializeField]
    private LevelManager manager;
	
	
	void Update ()
    {
        if (LevelManager.isLose)
        {
            outOfTimeUI.SetActive(true);
            Time.timeScale = 0;
        }
        else {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
	    }
    }

    public void Resume()
    {
        Debug.Log("Resuming...");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void RestartRound()
    {
        Debug.Log("Restarting Round...");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
        manager.RestartRound();
    }

    public void RestartLevel()
    {
        Debug.Log("Restarting Level...");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
        manager.RestartLevel();
    }

    public void Exit()
    {
        Debug.Log("Exitting...");
        pauseMenuUI.SetActive(false);
        outOfTimeUI.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
        manager.Exit();
    }

    public void RestartGame()
    {
        Debug.Log("Restarting Game");
        LevelManager.isLose = false;
        outOfTimeUI.SetActive(false);
        Time.timeScale = 1;
        manager.RestartGame();
    }

    private void Pause()
    {
        Debug.Log("Pausing");
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    
}
