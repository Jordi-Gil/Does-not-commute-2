using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHelper : MonoBehaviour
{

    #region Variables
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float speed = 20f;
    #endregion

    #region Unity Methods
    private void LateUpdate ()
    {
        if(player != null && target != null) { 
            Vector3 vectorToTarget = target.transform.position - player.transform.position;
            float angle = Mathf.Atan2(vectorToTarget.z, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);
        }
    }
    #endregion

    #region Public Methods
    public void setTarget(GameObject _target, GameObject _player)
    {
        _target.SetActive(false);
        target = _target;
        player = _player;
    }
#endregion
}
