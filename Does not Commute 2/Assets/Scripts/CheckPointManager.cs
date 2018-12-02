using UnityEngine;
using System.Collections;

public class CheckPointManager : MonoBehaviour
{
    
    [SerializeField]
    private Color color;
    private LevelManager levelManager;

    #region Main Method

    private void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<LevelManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " win");
        levelManager.NextRound();
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