using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour {


    public static bool isPaused;
    #region Variables
    [SerializeField]
    private GameObject pauseMenuUI;
    [SerializeField]
    private GameObject outOfTimeUI;
    [SerializeField]
    private LevelManager manager;
    #endregion

    #region Unity Methods
    private void Update ()
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
    #endregion

    #region Public Methods
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void RestartRound()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
        manager.RestartRound();
    }

    public void RestartLevel()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
        manager.RestartLevel();
    }

    public void Exit()
    {
        pauseMenuUI.SetActive(false);
        LevelManager.isLose = false;
        outOfTimeUI.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
        manager.Exit();
    }

    public void RestartGame()
    {
        LevelManager.isLose = false;
        outOfTimeUI.SetActive(false);
        Time.timeScale = 1;
        manager.RestartGame();
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }
    #endregion
}
