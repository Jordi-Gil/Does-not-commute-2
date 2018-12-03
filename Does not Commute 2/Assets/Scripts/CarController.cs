using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CarAxis //: System.Object
{
    public WheelCollider leftWheelCollider;
    public Transform leftWheelTransform;
    public WheelCollider rightWheelCollider;
    public Transform rightWheelTransform;
    public bool motor;
    public bool steering;
    public bool rear;
}


public struct UserInput
{
    public float gasInput;
    public float brakeInput;
    public float handbrakeInput;
    public float steerInput;

    public UserInput(float _gasInput, float _brakeInput, float _handbrakeInput, float _steerInput)
    {
        gasInput = _gasInput;
        brakeInput = _brakeInput;
        handbrakeInput = _handbrakeInput;
        steerInput = _steerInput;
    }
}

public class CarController : MonoBehaviour
{

    #region Variables
    [SerializeField]
    private Transform carTransform;
    [SerializeField]
    private Rigidbody carBody;
    [SerializeField]
    private float maxSteerAngle = 30;
    [SerializeField]
    private float motorForce = 100;
    [SerializeField]
    private float brake = 2500f;
    [SerializeField]
    private float antiRollForce = 100;
    [SerializeField]
    private bool isUser = true;
    [SerializeField]
    private List<CarAxis> Info_Axis;

    private float gasInput;
    private float brakeInput;
    private float handbrakeInput;
    private float steerInput;
    private float steeringAngle;
    private float motorTorque;
    private LevelManager levelManager;
    private int index;
    private static List<UserInput> path = new List<UserInput>();
    #endregion

    #region Unity Methods
    private void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<LevelManager>();
        isUser = true;
    }

    void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        Braking();
        AntiSwayController();
        UpdateWheelPoses();
        if (!isUser) index += 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(gameObject.tag))
        {

            foreach (CarAxis carAxis in Info_Axis)
            {
                carAxis.leftWheelCollider.motorTorque = 0;
                carAxis.rightWheelCollider.motorTorque = 0;
            }
            if(isUser) levelManager.NextRound();

        }
    }
    #endregion

    #region Privaye Methods
    private void GetInput()
    {
        if (isUser)
        {
            
            gasInput = Input.GetAxis("Vertical");
            steerInput = Input.GetAxis("Horizontal");
            brakeInput = Input.GetKey(KeyCode.R) ? 1 : 0;
            handbrakeInput = Mathf.Abs(Input.GetAxis("Jump")); //Space bar

            path.Add(new UserInput(gasInput,brake,handbrakeInput,steerInput));

        }
        else {

            gasInput = path[index].gasInput;
            steerInput = path[index].steerInput;
            brakeInput = path[index].brakeInput;
            handbrakeInput = path[index].handbrakeInput;

        }
    }
    private void Steer()
    {
        steeringAngle = maxSteerAngle * steerInput;
        foreach (CarAxis carAxis in Info_Axis) {
            if (carAxis.steering) {
                carAxis.leftWheelCollider.steerAngle = steeringAngle;
                carAxis.rightWheelCollider.steerAngle = steeringAngle;
            }
        }
    }
    private void Accelerate()
    {
        motorTorque = motorForce * gasInput;
        foreach (CarAxis carAxis in Info_Axis)
        {
            if (carAxis.motor)
            {
                carAxis.leftWheelCollider.motorTorque = motorTorque;
                carAxis.rightWheelCollider.motorTorque = motorTorque;
            }
        }
    }
    private void Braking()
    {
        if (handbrakeInput > 0.01f)
        {
            foreach (CarAxis carAxis in Info_Axis)
            {
                if (carAxis.rear) { 
                    ApplyBrakeTorque(carAxis.leftWheelCollider, (brake * 1.5f) * handbrakeInput);
                    ApplyBrakeTorque(carAxis.rightWheelCollider, (brake * 1.5f) * handbrakeInput);
                }
            }
        }
        else {
            float brakeAux;
            foreach (CarAxis carAxis in Info_Axis)
            {
                if (carAxis.rear) brakeAux = brake * Mathf.Clamp(brakeInput, 0, 1) / 2f;
                else brakeAux = brake * Mathf.Clamp(brakeInput, 0, 1);

                ApplyBrakeTorque(carAxis.leftWheelCollider, brakeAux);
                ApplyBrakeTorque(carAxis.rightWheelCollider, brakeAux);
            }
        }
    }
    private void ApplyBrakeTorque(WheelCollider _collider, float _brake)
    {
        _collider.brakeTorque = _brake;
    }
    private void UpdateWheelPoses()
    {
        foreach (CarAxis carAxis in Info_Axis)
        {
            UpdateWheelPoses(carAxis.leftWheelCollider, carAxis.leftWheelTransform);
            UpdateWheelPoses(carAxis.rightWheelCollider, carAxis.rightWheelTransform);
        }
    }
    private void UpdateWheelPoses(WheelCollider _collider, Transform _transform)
    {
        Vector3 _position = _transform.position;
        Quaternion _quat = _transform.rotation;

        // out -> like & in c++
        _collider.GetWorldPose(out _position, out _quat);

        _transform.position = _position;
        _transform.rotation = _quat;
    }
    private void AntiSwayController()
    {
        foreach (CarAxis carAxis in Info_Axis)
        {
            WheelHit leftHit, rightHit;
            if (!carAxis.leftWheelCollider.GetGroundHit(out leftHit)) {
                carBody.AddForceAtPosition(-carTransform.up * antiRollForce,carAxis.leftWheelTransform.position);
            }

            if (!carAxis.rightWheelCollider.GetGroundHit(out rightHit))
            {
                carBody.AddForceAtPosition(-carTransform.up * antiRollForce, carAxis.rightWheelTransform.position);
            }
        }
    }
    #endregion

    #region Public Methods
    public void restartPos(Vector3 pos_ini, Quaternion rot_ini)
    {

        carTransform.position = pos_ini;
        carTransform.rotation = rot_ini;

        foreach(CarAxis carAxis in Info_Axis)
        {
            carAxis.leftWheelCollider.brakeTorque = Mathf.Infinity;
            carAxis.rightWheelCollider.brakeTorque = Mathf.Infinity;
        }

        isUser = false;
    }
    #endregion
}
