using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private Color color;
    #endregion

    #region Unity Methods
    private void OnDrawGizmos()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position, new Vector3(4, 4, 4));
    }
    #endregion

    #region Public Methods
    public void setColor(Color _color)
    {
        color = _color;
    }
    #endregion
}
