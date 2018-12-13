using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
    private float time = 200f;
    [SerializeField]
    private TopDownCamera scriptCamera;
    [SerializeField]
    private ArrowHelper scriptArrow;
    [SerializeField]
    private RainManager scriptRain;
    [SerializeField]
    private Text textHintCar;
    [SerializeField]
    private TextMeshProUGUI textTime;
    [SerializeField]
    private GameObject activeCar;
    [SerializeField]
    private List<GameObject> carsPrefabs;
    [SerializeField]
    private List<TransformPair> paths;
    [SerializeField]
    private List<PathCompleted> pathCompleted;

    private int maxRounds;
    private float timeLeft;
    private float timeIniRound;

    public static bool isLose;
    #endregion

    #region Main Methods
    private void Start ()
    {
        timeLeft = time;
        timeIniRound = timeLeft;
        pathCompleted = new List<PathCompleted>();
        Initalize();
    }

    private void FixedUpdate()
    {
        timeLeft -= Time.deltaTime;
        textTime.text = timeLeft.ToString("F2") + " s";
        if (timeLeft <= 0)
        {
            Lose();
        }
    }

    #endregion

    #region Private Methods
    private void MakePaths()
    {
        List<GameObject> aux = new List<GameObject>(carsPrefabs);
        while (aux.Count > 0)
        {
            int carIndex = Random.Range(0, aux.Count-1);

            GameObject car = aux[carIndex];
            
            string start = car.tag + "start";
            string end = car.tag + "end";
            
            GameObject startT = GameObject.FindGameObjectWithTag(start);
            GameObject endT = GameObject.FindGameObjectWithTag(end);

            car = Instantiate(car, startT.transform.position, startT.transform.rotation);
            
            car.SetActive(false);

            paths.Add(new TransformPair(car,startT,endT));

            aux.RemoveAt(carIndex);
        }
    }

    private void Lose()
    {
        isLose = true;
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
            scriptArrow.setTarget(Instantiate(paths[round].p_end), activeCar);
            if (scriptRain != null) scriptRain.setTarget(activeCar);
            activeCar.SetActive(true);
            scriptCamera.ChangeTarget(activeCar);
            textHintCar.text = activeCar.name + ' ' + activeCar.tag;
            timeIniRound = timeLeft;
            InstantiateIA();
        }
    }

    public void RestartRound()
    {
        Debug.Log("Restarting Round manager...");
        activeCar.GetComponent<CarController>().Restart(paths[round].p_start.transform);
        timeLeft = timeIniRound - 1;
    }

    public void RestartLevel()
    {
        timeLeft = time;
        activeCar.GetComponent<CarController>().PlayPlayer(paths[round].p_start.transform.position,
                                                           paths[round].p_start.transform.rotation);
        activeCar.SetActive(false);
        foreach(PathCompleted pathComp in pathCompleted)
        {
            GameObject car = pathComp.getCar();
            car.transform.position = pathComp.getStartPosition();
            car.transform.rotation = pathComp.getStartRotation();
            car.GetComponent<CarController>().PlayPlayer(pathComp.getStartPosition(), pathComp.getStartRotation());
            car.SetActive(false);
        }

        pathCompleted.Clear();

        round = 0;
        activeCar = paths[round].p_car;
        activeCar.SetActive(true);
        scriptArrow.setTarget(Instantiate(paths[round].p_end), activeCar);
        if (scriptRain != null) scriptRain.setTarget(activeCar);
        scriptCamera.ChangeTarget(activeCar);
        textHintCar.text = activeCar.name + ' ' + activeCar.tag;
    }

    public void Exit()
    {
        SceneManager.LoadScene("Menu");
    }

    public void RestartGame()
    {
        timeLeft = time;
        foreach(TransformPair car in paths)
        {
            Destroy(car.p_car);
        }
        paths.Clear();
        pathCompleted.Clear();
        Initalize();
    }
    #endregion

    #region Private Methods

    private void Initalize()
    {
        MakePaths();
        maxRounds = paths.Count;
        activeCar = paths[round].p_car;
        scriptArrow.setTarget(Instantiate(paths[round].p_end), activeCar);
        if (scriptRain != null) scriptRain.setTarget(activeCar);
        activeCar.SetActive(true);
        scriptCamera.ChangeTarget(activeCar);
        textHintCar.text = activeCar.name + ' ' + activeCar.tag;
    }

    private void InstantiateIA()
    {
        foreach (PathCompleted pathComp in pathCompleted)
        {
            GameObject car = pathComp.getCar();
            car.SetActive(true);
            car.transform.position = pathComp.getStartPosition();
            car.transform.rotation = pathComp.getStartRotation();
            List<PointInTime> path;
            pathComp.getPath(out path);
            car.GetComponent<CarController>().setPath(new List<PointInTime>(path));
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
