using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour {

    [SerializeField]
    private Color color;
    // Use this for initialization
    private void OnDrawGizmos()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position, new Vector3(4, 2, 4));
    }
}
