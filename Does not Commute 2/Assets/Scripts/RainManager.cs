using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainManager : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Transform particleSystemTransform;
    #endregion

    #region Unity Methods
    private void Start () {
        particleSystemTransform = GetComponent<Transform>();
    }

    private void Update () {
        Vector3 finalPosition = player.transform.position;
        finalPosition.y += 40f;
        particleSystemTransform.position = Vector3.Lerp(particleSystemTransform.position, finalPosition, 20);
    }

    #endregion

    #region Public Methods
    public void setTarget(GameObject _player)
    {
        player = _player;
    }
    #endregion
}
