using UnityEngine;
using System.Collections;

public class CheckPointManager : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private Color color;
    #endregion

    #region Main Method
    private void Start()
    {
        
    }
    
    private void OnDrawGizmos()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position, new Vector3(4, 2, 1));
    }
    #endregion

    #region Helper Method
    #endregion
}