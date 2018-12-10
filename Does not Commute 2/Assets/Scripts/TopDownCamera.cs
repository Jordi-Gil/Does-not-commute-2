using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour {

    #region Variables
    //Las variables pasan a ser privadas para estar protegidas por otros scrips, pero añadimos
    //[SerializeField] para que nos salga en els inspector y así poder ajustar los parametros desde el Editor de Unity
    [SerializeField]
    private Transform targetTransform;
    [SerializeField]
    private float height = 50f;
    [SerializeField]
    private float smoothVelocity = 10f;
    
    private Transform cameraTransform;
    
    #endregion


    #region Main Method
    private void Start ()
    {
        cameraTransform = GetComponent<Transform>();
        cameraTransform.rotation = Quaternion.identity;
        cameraTransform.Rotate(80,0,0);
        if (targetTransform != null)
            HandleCamera();
    }
    #endregion   

    private void Update ()
    {
        if(targetTransform != null)
            HandleCamera();
    }

    #region Helper Methods
    private void HandleCamera()
    {

        Vector3 finalPosition = targetTransform.position;
        finalPosition.y += height;
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, finalPosition, smoothVelocity);
        
    }
    #endregion

    #region Public Methods

    public void ChangeTarget(GameObject _target)
    {
        targetTransform = _target.transform;
    }

    #endregion
}
