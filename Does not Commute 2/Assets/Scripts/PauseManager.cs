using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {


    public static bool isPaused;

    [SerializeField]
    private GameObject pauseMenuUI;
    [SerializeField]
    private LevelManager manager;
	
	
	void Update ()
    {
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

    public void Resume()
    {
        Debug.Log("Resuming");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void RestartRound()
    {
        Resume();
        manager.RestartRound();
    }

    public void RestartLevel()
    {
        Resume();
        manager.RestartLevel();
    }

    public void Exit()
    {
        Resume();
        manager.Exit();
    }

    private void Pause()
    {
        Debug.Log("Pausing");
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    
}
