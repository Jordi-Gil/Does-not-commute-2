using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    #region Public Methods
    public void QuitGame() {
        Application.Quit();
	}
    public void PlayGame() {
        SceneManager.LoadScene("Level1");
    }
    #endregion
}
