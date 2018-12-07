using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CarAxis
{
    public WheelCollider leftWheelCollider;
    public Transform leftWheelTransform;
    public WheelCollider rightWheelCollider;
    public Transform rightWheelTransform;
    public bool motor;
    public bool steering;
    public bool rear;
}

public class CarController : MonoBehaviour
{

    #region Variables
    [SerializeField]
    private Transform carTransform;
    [SerializeField]
    private Rigidbody carBody;
    [SerializeField]
    private float maxSteerAngle = 10f;
    [SerializeField]
    private float motorForce = 400f;
    [SerializeField]
    private float brake = 2500f;
    [SerializeField]
    private float speed = 0;
    [SerializeField]
    private Vector3 CenterOfMass = new Vector3(0,-3, 0.5f);
    [SerializeField]
    private bool controlUser = true;
    [SerializeField]
    private List<CarAxis> Info_Axis;

    private float gasInput;
    private float brakeInput;
    private float handbrakeInput;
    private float steerInput;
    private float steeringAngle;
    private float motorTorque;
    private LevelManager levelManager;

    private List<PointInTime> path;
    #endregion

    #region Unity Methods
    private void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<LevelManager>();
        path = new List<PointInTime>();
        
        carBody.centerOfMass = CenterOfMass;
    }

    void FixedUpdate()
    {
        if (controlUser)
        {
            GetInput();
            Steer();
            Accelerate();
            Braking();
            UpdateWheelPoses();
            Record();

        }
        else {
            Play();
        }

        speed = carBody.velocity.magnitude * 3.6f;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(gameObject.tag+"end"))
        {
            if (controlUser)
            {
                carBody.isKinematic = true;
                levelManager.NextRound(path);   
            }
        }
    }
    #endregion

    #region Privaye Methods
    private void GetInput()
    {  
        gasInput = Mathf.Clamp(Input.GetAxis("Vertical"),-0.5f,0.5f);
        steerInput = Input.GetAxis("Horizontal");
        brakeInput = Input.GetKey(KeyCode.R) ? 1 : 0;
        handbrakeInput = Mathf.Abs(Input.GetAxis("Jump")); //Space bar
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

    private void Record()
    {
        path.Add(new PointInTime(transform.position, transform.rotation));
    }

    private void Play()
    {
        if(path.Count > 0)
        {
            PointInTime point = path[0];
            transform.position = point.getPosition();
            transform.rotation = point.getRotation();
            path.RemoveAt(0);
        }
    }
    #endregion

    #region Public Methods
    public void PlayIA()
    {
        controlUser = false;
    }

    public void setPath(List<PointInTime> _path)
    {
        path = _path;
    }
    #endregion
}
