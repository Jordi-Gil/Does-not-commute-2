using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    #region Variables
    [SerializeField]
    private AudioManagerMenu audioManagerMenu;
    [SerializeField]
    private GameObject credits;
    
    #endregion

    #region Public Methods
    public void QuitGame()
    {

        audioManagerMenu.Exit();
        
        while (audioManagerMenu.GetComponent<AudioSource>().isPlaying) ;
        
        Application.Quit();
	}

    public void PlayGame() {
        SceneManager.LoadScene("Level1");
    }

    public void Credits()
    {
        credits.SetActive(true);
    }

    public void Menu()
    {
        credits.SetActive(false);
    }
    #endregion
}
