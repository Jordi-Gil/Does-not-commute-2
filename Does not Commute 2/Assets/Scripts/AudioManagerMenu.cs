using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerMenu : MonoBehaviour {

    #region Variables
    [SerializeField]
    private AudioClip exitClip;
    [SerializeField]
    private AudioSource audio;
    #endregion


    #region PublicMethods
    public void Exit()
    {
        audio.Stop();
        audio.clip = exitClip;
        audio.loop = false;
    }
    #endregion
}
