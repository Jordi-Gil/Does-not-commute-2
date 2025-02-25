﻿using System.Collections;
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
    private LevelManager levelManager;
    [SerializeField]
    private AudioManager audioManager;
    [SerializeField]
    private float speed;
    [SerializeField]
    private Transform carTransform;
    [SerializeField]
    private Rigidbody carBody;
    [SerializeField]
    private float maxSteerAngle = 15f;
    [SerializeField]
    private float motorForce = 150;
    [SerializeField]
    private float brake = 2500f;
    [SerializeField]
    private float antiRollForce = 5000f;
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
    
    [SerializeField]
    private List<PointInTime> path;
    #endregion

    #region Unity Methods
    private void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<LevelManager>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        path = new List<PointInTime>();
        
        carBody.centerOfMass = Vector3.down;
    }

    private void FixedUpdate()
    {
        speed = carBody.velocity.magnitude * 3.6f;
        if (controlUser)
        {
            GetInput();
            Steer();
            Accelerate();
            AntiRollBars();
            Braking();
            UpdateWheelPoses();
            Record();
        }
        else {
            Play();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Collision"))
        {

            audioManager.HitSound();
            if (speed > 45f) { 
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                motorForce -= 20f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.CompareTag(gameObject.tag + "end"))
        {
            Debug.Log(other.name);
            if (controlUser)
            {
                gameObject.SetActive(false);
                carBody.isKinematic = true;
                levelManager.NextRound(path);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else if (other.gameObject.CompareTag("Boost10"))
        {
            Debug.Log(other.name);
            levelManager.BoostTime(10f);
            audioManager.BoostSound();
            other.gameObject.SetActive(false);
            
        }else if (other.gameObject.CompareTag("Boost20"))
        {
            Debug.Log(other.name);
            levelManager.BoostTime(20f);
            other.gameObject.SetActive(false);
            audioManager.BoostSound();
            other.gameObject.GetComponent<AudioSource>().Play();
        }
        else if(other.gameObject.CompareTag("Boost50"))
        {
             Debug.Log(other.name);
            levelManager.BoostTime(50f);
            other.gameObject.SetActive(false);
            audioManager.BoostSound();
            other.gameObject.GetComponent<AudioSource>().Play();
        }


    }
    #endregion

    #region Privaye Methods
    private void GetInput()
    {  
        gasInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
        brakeInput = Input.GetKey(KeyCode.F) ? 1 : 0;
        handbrakeInput = Input.GetKey(KeyCode.Space) ? 1f : 0f;
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

    private void AntiRollBars()
    {
        foreach (CarAxis carAxis in Info_Axis)
        {
            WheelHit wheelHitL, wheelHitR;
            bool groundedWheelLeft = carAxis.leftWheelCollider.GetGroundHit(out wheelHitL);
            bool groundedWheelRight = carAxis.rightWheelCollider.GetGroundHit(out wheelHitR);

            if (!groundedWheelLeft)
                carBody.AddForceAtPosition(-carAxis.leftWheelCollider.transform.up * antiRollForce, carAxis.leftWheelCollider.transform.position);
            if (!groundedWheelRight)
                carBody.AddForceAtPosition(-carAxis.rightWheelCollider.transform.up * antiRollForce, carAxis.rightWheelCollider.transform.position);
        }
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

    public void PlayPlayer(Vector3 _position, Quaternion _rotation)
    {
        controlUser = true;
        path.Clear();
        carTransform.position = _position;
        carTransform.rotation = _rotation;
        carBody.isKinematic = false;
    }

    public void setPath(List<PointInTime> _path)
    {
        path = _path;
    }

    public void Restart(Transform _transform)
    {
        path.Clear();
        Debug.Log(path.Count);
        carTransform.position = _transform.position;
        carTransform.rotation = _transform.rotation;
        Debug.Log(_transform.position);
    }

    public void FixCar(float _value)
    {
        motorForce += _value;
    }

    #endregion
}
