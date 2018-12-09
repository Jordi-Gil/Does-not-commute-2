using UnityEngine;

public class ArrowHelp : MonoBehaviour {

    [SerializeField]
    private GameObject target;

    public void setTarget(GameObject _target)
    {
        target = _target;
    }

    void Update()
    {
        PositionArrow();
    }

    void PositionArrow()
    {
        //<Renderer>().enabled = false;

        Vector3 v3Pos = Camera.main.WorldToViewportPoint(target.transform.position);

        if (v3Pos.z < Camera.main.nearClipPlane)
            return;

        if (v3Pos.x >= 0.0f && v3Pos.x <= 1.0f && v3Pos.y >= 0.0f && v3Pos.y <= 1.0f)
            return;

        //GetComponent<Renderer>().enabled = true;
        v3Pos.x -= 0.5f;
        v3Pos.y -= 0.5f;
        v3Pos.z = 0;

        float fAngle = Mathf.Atan2(v3Pos.x, v3Pos.y);
        transform.localEulerAngles = new Vector3(0.0f, 0.0f, -fAngle * Mathf.Rad2Deg);

        v3Pos.x = 0.5f * Mathf.Sin(fAngle) + 0.5f;
        v3Pos.y = 0.5f * Mathf.Cos(fAngle) + 0.5f;
        v3Pos.z = Camera.main.nearClipPlane + 0.01f;
        transform.position = Camera.main.ViewportToWorldPoint(v3Pos);
    }
}
