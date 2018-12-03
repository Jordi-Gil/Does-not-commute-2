using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct TransformPair
{
    [SerializeField]
    private Transform first;
    [SerializeField]
    private Transform second;

    public Transform p_first { set { first = value; } get { return first; } }
    public Transform p_second { set { second = value; } get { return second; } }
}

public class LevelManager : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private int round = 0;
    [SerializeField]
    private TopDownCamera scriptCamera;
    [SerializeField]
    private List<GameObject> carsPrefabs;
    [SerializeField]
    private List<TransformPair> paths;
    [SerializeField]
    private List<PathCompleted> pathCompleted;
    [SerializeField]
    private GameObject activeCar;
    #endregion


    #region Main Methods
    // Use this for initialization
    private void Start ()
    {
        pathCompleted = new List<PathCompleted>();
        activeCar = Instantiate(carsPrefabs[round], paths[round].p_first.position, Quaternion.identity);
        //carsPrefabs[round] = activeCar;
        scriptCamera.ChangeTarget(activeCar);
	}
    #endregion

    #region Public Methods
    public void NextRound(List<PointInTime> pathCar)
    {
        TransformPair path = paths[round];
        pathCompleted.Add(new PathCompleted(activeCar, path.p_first.position, path.p_first.rotation, path.p_second.position, pathCar));
        round += 1;
        if (round >= carsPrefabs.Count)
        {
            Finish();
        }
        else
        {
            activeCar = Instantiate(carsPrefabs[round], paths[round].p_first.position, Quaternion.identity);
            carsPrefabs[round] = activeCar;
            scriptCamera.ChangeTarget(activeCar);

            InstantiateIA();
        }
    }
    #endregion

    #region Public Methods
    private void InstantiateIA()
    {
        foreach (PathCompleted pathComp in pathCompleted)
        {
            GameObject car = pathComp.getCar();
            car.transform.position = pathComp.getStartPosition();
            car.transform.rotation = pathComp.getStartRotation();
            List<PointInTime> path;
            pathComp.getPath(out path);
            car.GetComponent<CarController>().setPath(path);
            car.GetComponent<CarController>().PlayIA();
        }
    }

    private void Finish()
    {
        foreach (PathCompleted pathComp in pathCompleted)
        {
            Destroy(pathComp.getCar());
        }
    }
    #endregion
}
