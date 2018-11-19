using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    private float gasInput;
    private float brakeInput;
    private float handbrakeInput;
    private float steerInput;
    private float steeringAngle;

    public Rigidbody carBody;

    public WheelCollider frontDriverW, frontPassengerW;
    public WheelCollider rearDriverW, rearPassengerW;
    public Transform frontDriverT, frontPassengerT;
    public Transform rearDriverT, rearPassengerT;

    public float maxSteerAngle = 30;
    public float motorForce = 400;
    public float brakeForce = 2500f;
    public float brake = 2500f;

    private void Start()
    {
        float massWheel = carBody.mass / 20f;

        frontDriverW.mass    = massWheel;
        frontPassengerW.mass = massWheel;
        rearDriverW.mass     = massWheel;
        rearPassengerW.mass  = massWheel;
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
        brakeInput = Mathf.Clamp01(-Input.GetAxis("Vertical"));
        handbrakeInput = Input.GetKey(KeyCode.Space) ? 1f : 0f; 
        steerInput = Input.GetAxis("Horizontal");
    }

    private void Steer()
    {
        steeringAngle = maxSteerAngle * steerInput;
        frontDriverW.steerAngle = steeringAngle;
        frontPassengerW.steerAngle = steeringAngle;
    }

    private void Accelerate()
    {
        frontDriverW.motorTorque = gasInput * motorForce;
        frontPassengerW.motorTorque = gasInput * motorForce;
    }

    private void Braking()
    {
        if (handbrakeInput > 0.1f)
        {
            ApplyBrakeTorque(rearDriverW, (brake * 1.5f) * handbrakeInput);
            ApplyBrakeTorque(rearPassengerW, (brake * 1.5f) * handbrakeInput);
        }
        else {
            ApplyBrakeTorque(frontDriverW, brake * Mathf.Clamp(brakeInput, 0, 1));
            ApplyBrakeTorque(frontPassengerW, brake * Mathf.Clamp(brakeInput, 0, 1));
            ApplyBrakeTorque(rearDriverW, brake * Mathf.Clamp(brakeInput, 0, 1) / 2f);
            ApplyBrakeTorque(rearPassengerW, brake * Mathf.Clamp(brakeInput, 0, 1) / 2f);
        }
    }

    private void ApplyBrakeTorque(WheelCollider _collider, float _brake)
    {
        _collider.brakeTorque = _brake;
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPoses(frontDriverW, frontDriverT);
        UpdateWheelPoses(frontPassengerW, frontPassengerT);
        UpdateWheelPoses(rearDriverW, rearDriverT);
        UpdateWheelPoses(rearPassengerW, rearPassengerT);
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
