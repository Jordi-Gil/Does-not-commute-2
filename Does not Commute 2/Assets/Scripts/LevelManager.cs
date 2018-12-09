using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TransformPair
{
    [SerializeField]
    private GameObject car;
    [SerializeField]
    private GameObject start;
    [SerializeField]
    private GameObject end;

    public TransformPair(GameObject _car, GameObject _start, GameObject _end)
    {
        car = _car;
        start = _start;
        end = _end;
    }
    public GameObject p_start { set { start = value; } get { return start; } }
    public GameObject p_end { set { end = value; } get { return end; } }
    public GameObject p_car { set { car = value; } get { return car; } }
}

public class LevelManager : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private int round = 0;
    [SerializeField]
    private TopDownCamera scriptCamera;
    [SerializeField]
    private ArrowHelp scriptArrow;
    [SerializeField]
    private GameObject activeCar;
    [SerializeField]
    private List<GameObject> carsPrefabs;
    [SerializeField]
    private List<TransformPair> paths;
    [SerializeField]
    private List<PathCompleted> pathCompleted;

    private int maxRounds;
    #endregion

    #region Main Methods
    // Use this for initialization
    private void Start ()
    {
        pathCompleted = new List<PathCompleted>();
        MakePaths();
        maxRounds = paths.Count;
        activeCar = paths[round].p_car;
        scriptArrow.setTarget(paths[round].p_end.gameObject);
        activeCar.SetActive(true);
        scriptCamera.ChangeTarget(activeCar);
	}
    #endregion

    #region Private Methods
    private void MakePaths()
    {
        while(carsPrefabs.Count > 0)
        {
            int carIndex = Random.Range(0,carsPrefabs.Count-1);

            GameObject car = carsPrefabs[carIndex];
            
            string start = car.tag + "start";
            string end = car.tag + "end";

            Debug.Log(start);

            GameObject startT = GameObject.FindGameObjectWithTag(start);
            GameObject endT = GameObject.FindGameObjectWithTag(end);

            car = Instantiate(car, startT.transform.position, startT.transform.rotation);
            
            car.SetActive(false);

            paths.Add(new TransformPair(car,startT,endT));

            carsPrefabs.RemoveAt(carIndex);
        }
    }
    #endregion

    #region Public Methods
    public void NextRound(List<PointInTime> pathCar)
    {
        
        TransformPair path = paths[round];
        pathCompleted.Add(new PathCompleted(path.p_car, path.p_start.transform.position, path.p_start.transform.rotation, path.p_end.transform.position, pathCar));
        round += 1;
        Debug.Log("Next Round: "+round);
        if (round >= maxRounds)
        {
            Finish();
        }
        else
        {
            activeCar = paths[round].p_car;
            scriptArrow.setTarget(paths[round].p_end.gameObject);
            activeCar.SetActive(true);
            scriptCamera.ChangeTarget(activeCar);

            InstantiateIA();
        }
    }
    #endregion

    #region Private Methods

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
