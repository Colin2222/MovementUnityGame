using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryJumpAutocorrectHandler : MonoBehaviour
{
    public Transform checkPoint;
    public Transform launchPoint;
    public Vector2 checkBoxSize;
    LayerMask cornerLayer;
    // Start is called before the first frame update
    void Start()
    {
        cornerLayer = LayerMask.GetMask("Corners");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public float GetCornerAutocorrectDistance()
    {
        Collider2D hit = Physics2D.OverlapBox(checkPoint.position, checkBoxSize, 0, cornerLayer);
        if (hit != null)
        {
            Transform cornerTransform = hit.transform;
            Debug.Log("HIT CORNER " + (cornerTransform.position.x - launchPoint.transform.position.x));
            return cornerTransform.position.x - launchPoint.transform.position.x;
        }
        Debug.Log("NO HIT");
        return 0;
    }
}
