using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CarsInfo
{
    public GameObject car;
    public CarController scriptController;
    public int index;
}

public class LevelManager : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private int round = 0;
    [SerializeField]
    private TopDownCamera scriptCamera;
    [SerializeField]
    private List<CarsInfo> cars;
    #endregion


    #region Main Methods
    // Use this for initialization
    private void Start ()
    {
        GameObject clone;
        clone = Instantiate(cars[round].car, new Vector3(50,0,10), Quaternion.identity);
        
	}

    private void OnApplicationQuit()
    {
        foreach (CarsInfo carInf in cars)
        {
            carInf.car.tag = "Car";
            carInf.scriptController.enabled = true;
        }

        cars[0].car.tag = "ActiveCar";
    }

    #endregion

    #region Public Methods
    public void NextRound()
    {
        round += 1;
        Instantiate(cars[round].car, new Vector3(50, 0, 10), Quaternion.identity);
        for (int i = round - 1; i >= 0; --i) {
            Debug.Log(cars[i].car.name + " desactivated");
            cars[i].car.tag = "Car";
            //cars[i].scriptController.enabled = false;
        }
        cars[round].car.tag = "ActiveCar";
        scriptCamera.ChangeTarget();
    }
    #endregion


}
