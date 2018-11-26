using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CarWheels : System.Object
{
    public WheelCollider leftWheel;
    public Transform leftWheelTransform;
    public WheelCollider rightWheel;
    public Transform rightWheelTransform;
    public bool motor;
    public bool steering;
    public bool rear;
}

public class CarController : MonoBehaviour
{

    private float gasInput;
    private float brakeInput;
    private float handbrakeInput;
    private float steerInput;
    private float steeringAngle;
    private float motorTorque;
    

    public Rigidbody carBody;

    public float maxSteerAngle = 30;
    public float motorForce = 400;
    public float brakeForce = 2500f;
    public float brake = 2500f;

    public List<CarWheels> Info_Axis;

    private void Start()
    {
        float massWheel = carBody.mass / 10f;
        foreach (CarWheels carAxis in Info_Axis) {
            carAxis.leftWheel.mass = massWheel;
            carAxis.rightWheel.mass = massWheel;
        }
        
    }

    // This function is called every fixed framerate frame.
    // FixedUpdate should be used instead of Update when dealing with Rigidbody.
    // For example when adding a force to a rigidbody, you have to apply the force every fixed frame inside FixedUpdate 
    // instead of every frame inside Update.
    void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        Braking();
        UpdateWheelPoses();
    }

    private void GetInput()
    {
        gasInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
        brakeInput = Mathf.Clamp01(-Input.GetAxis("Vertical"));
        handbrakeInput = Mathf.Abs(Input.GetAxis("Jump"));
    }

    private void Steer()
    {
        steeringAngle = maxSteerAngle * steerInput;
        foreach (CarWheels carAxis in Info_Axis) {
            if (carAxis.steering) {
                carAxis.leftWheel.steerAngle = steeringAngle;
                carAxis.rightWheel.steerAngle = steeringAngle;
            }
        }
    }

    private void Accelerate()
    {
        motorTorque = motorForce * gasInput;
        foreach (CarWheels carAxis in Info_Axis)
        {
            if (carAxis.motor)
            {
                carAxis.leftWheel.motorTorque = motorTorque;
                carAxis.rightWheel.motorTorque = motorTorque;
            }
        }
    }

    private void Braking()
    {
        if (handbrakeInput > 0.01f)
        {
            foreach (CarWheels carAxis in Info_Axis)
            {
                if (carAxis.rear) { 
                    ApplyBrakeTorque(carAxis.leftWheel, (brake * 1.5f) * handbrakeInput);
                    ApplyBrakeTorque(carAxis.rightWheel, (brake * 1.5f) * handbrakeInput);
                }
            }
        }
        else {
            float brakeAux;
            foreach (CarWheels carAxis in Info_Axis)
            {
                if (carAxis.rear) brakeAux = brake * Mathf.Clamp(brakeInput, 0, 1) / 2f;
                else brakeAux = brake * Mathf.Clamp(brakeInput, 0, 1);

                ApplyBrakeTorque(carAxis.leftWheel, brakeAux);
                ApplyBrakeTorque(carAxis.rightWheel, brakeAux);
            }
        }
    }

    private void ApplyBrakeTorque(WheelCollider _collider, float _brake)
    {
        _collider.brakeTorque = _brake;
    }

    private void UpdateWheelPoses()
    {
        foreach (CarWheels carAxis in Info_Axis)
        {
            UpdateWheelPoses(carAxis.leftWheel, carAxis.leftWheelTransform);
            UpdateWheelPoses(carAxis.rightWheel, carAxis.rightWheelTransform);
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
}
