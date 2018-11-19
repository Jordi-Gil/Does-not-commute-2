using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCar : MonoBehaviour
{

    public Transform carPlayer;

    public Vector3 offset;
    public float followSpeed = 10;
    public float lookSpeed = 10;
    
    // LateUpdate is called after Update each frame
    void FixedUpdate()
    {
        LookAtTarget();
        MoveToTarget();
    }

    void LookAtTarget()
    {
        Vector3 _lookDirection = carPlayer.position - transform.position;
        Quaternion _rotation = Quaternion.LookRotation(_lookDirection, Vector3.up);

        transform.rotation = Quaternion.Lerp(transform.rotation, _rotation, lookSpeed * Time.deltaTime );

    }

    void MoveToTarget()
    {
        Vector3 _carPosition = carPlayer.position +
                                carPlayer.forward * offset.z +
                                carPlayer.right * offset.x +
                                carPlayer.up * offset.y;

        transform.position = Vector3.Lerp(transform.position, _carPosition, followSpeed * Time.deltaTime);
    }
}
