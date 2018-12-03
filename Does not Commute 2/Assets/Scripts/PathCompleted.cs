using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PathCompleted
{
    public PathCompleted(GameObject _car, Vector3 _startPosition, Quaternion _startRotation, Vector3 _destination, List<PointInTime> _path)
    {
        car = _car;
        startPosition = _startPosition;
        startRotation = _startRotation;
        destination = _destination;
        path = _path;
    }

    public GameObject getCar()
    {
        return car;
    }

    public Vector3 getStartPosition()
    {
        return startPosition;
    }

    public Quaternion getStartRotation()
    {
        return startRotation;
    }

    public Vector3 getDestination()
    {
        return destination;
    }

    public void getPath(out List<PointInTime> _path)
    {
        _path = path;
    }

    [SerializeField]
    private GameObject car;
    [SerializeField]
    private Vector3 startPosition;
    [SerializeField]
    private Quaternion startRotation;
    [SerializeField]
    private Vector3 destination;
    [SerializeField]
    private List<PointInTime> path;
}