using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    #region Variables
    [SerializeField]
    private AudioSource audio;
    [SerializeField]
    private List<AudioClip> clips;
    #endregion

    #region Public Methods
    private void Start ()
    {
        if (clips == null) clips = new List<AudioClip>();
	}
    #endregion

    #region Public Methods
    public void BoostSound()
    {
        
        audio.PlayOneShot(clips[0]);
    }
    #endregion
}
