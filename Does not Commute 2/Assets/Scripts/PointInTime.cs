using UnityEngine;

[System.Serializable]
public class PointInTime
{
    #region Public Methods
    public PointInTime(Vector3 _position, Quaternion _rotation)
    {
        position = _position;
        rotation = _rotation;
    }

    public void setPosition(Vector3 _value)
    {
        position = _value;
    }

    public Vector3 getPosition()
    {
        return position;
    }

    public void setRotation(Quaternion _value)
    {
        rotation = _value;
    }

    public Quaternion getRotation()
    {
        return rotation;
    }
    #endregion

    #region Variables
    [SerializeField]
    private Vector3 position;
    [SerializeField]
    private Quaternion rotation;
    #endregion
}