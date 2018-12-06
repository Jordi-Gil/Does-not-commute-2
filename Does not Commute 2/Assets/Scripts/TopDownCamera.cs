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
    private float distance = 10f;
    [SerializeField]
    private float height = 20f;
    [SerializeField]
    private float angle = 45f;
    [SerializeField]
    private float smoothVelocity = 0.5f;
    
    private Transform cameraTransform;
    private Vector3 refVelocity;
    #endregion


    #region Main Method
    private void Start ()
    {
        cameraTransform = GetComponent<Transform>();
        HandleCamera();
    }
    #endregion   

    private void Update ()
    {
        HandleCamera();
    }

    #region Helper Methods
    private void HandleCamera()
    {
        if (!targetTransform) return;
        Vector3 worldPosition = (Vector3.forward * -distance) + (Vector3.up * height);
   
        Debug.DrawLine(targetTransform.position, worldPosition, Color.red);
        //Vector3 rotatedVector = Quaternion.AngleAxis(angle, Vector3.up) * worldPosition;
        //Debug.DrawLine(targetTransform.position, rotatedVector, Color.green);

        Vector3 flatCarPosition = targetTransform.position;
        flatCarPosition.y = 40f;
        Vector3 finalPosition = flatCarPosition /*+ rotatedVector*/;
        Debug.DrawLine(targetTransform.position, finalPosition, Color.blue);

        cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position,finalPosition,ref refVelocity ,smoothVelocity);
        cameraTransform.LookAt(targetTransform.position); 
    }
    #endregion

    #region Public Methods

    public void ChangeTarget(GameObject _target)
    {
        targetTransform = _target.transform;
    }

    #endregion
}
