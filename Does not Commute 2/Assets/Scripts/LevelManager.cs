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
    private List<GameObject> cars;
    [SerializeField]
    private List<TransformPair> paths;
    private int numRounds;
    [SerializeField]
    private GameObject activeCar;
    #endregion


    #region Main Methods
    // Use this for initialization
    private void Start ()
    {
        numRounds = cars.Count;
        activeCar = Instantiate(cars[round], paths[round].p_first.position, Quaternion.identity);
        cars[round] = activeCar;
        scriptCamera.ChangeTarget(activeCar);
	}

    private void OnApplicationQuit()
    {
        
    }

    #endregion

    #region Public Methods
    public void NextRound()
    {
        cars[round].GetComponent<CarController>().enabled = false; //Destruirlo y guardar info
        round += 1;
        if (round >= numRounds) return;
        activeCar = Instantiate(cars[round], new Vector3(50, 0, 10), Quaternion.identity);
        cars[round] = activeCar;
        scriptCamera.ChangeTarget(activeCar);
    }
    #endregion
}
