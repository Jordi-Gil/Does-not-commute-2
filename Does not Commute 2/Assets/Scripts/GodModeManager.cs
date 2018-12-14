using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodModeManager : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private LevelManager levelManager;
    #endregion

    #region Unity Methods
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.N))
        {
            Debug.Log("Teleporting");
            levelManager.Teleport();
        }
    }
    #endregion

}
