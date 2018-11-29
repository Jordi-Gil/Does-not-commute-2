using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCar : MonoBehaviour
{

    public Transform carTransform;
    public Transform camTransform;

    public Vector3 offset;
    public float followSpeed = 10;
    public float lookSpeed = 5;

    // LateUpdate is called after Update each frame
    void FixedUpdate()
    {
        Quaternion _quat = Quaternion.LookRotation(carTransform.forward);

        offset.x = carTransform.position.x;


        camTransform.position = Vector3.Lerp(carTransform.position, offset, followSpeed * Time.deltaTime);

        _quat = Quaternion.Slerp(carTransform.rotation, _quat, lookSpeed * Time.deltaTime);

        camTransform.rotation = _quat;
    }



}
