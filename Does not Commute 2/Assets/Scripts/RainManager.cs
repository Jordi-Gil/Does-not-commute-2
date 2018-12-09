using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainManager : MonoBehaviour
{

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Transform particleSystemTransform;
    // Use this for initialization
    void Start () {
        particleSystemTransform = GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 finalPosition = player.transform.position;
        finalPosition.y += 40f;
        particleSystemTransform.position = Vector3.Lerp(particleSystemTransform.position, finalPosition, 20);
    }

    public void setTarget(GameObject _player)
    {
        player = _player;
    }
}
