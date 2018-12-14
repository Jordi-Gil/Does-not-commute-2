using UnityEngine;
using System.Collections;

public class DespawnManager : MonoBehaviour
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
        Gizmos.DrawCube(transform.position, new Vector3(15, 4, 15));
    }
    #endregion

}