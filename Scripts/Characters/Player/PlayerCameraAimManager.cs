using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraAimManager : MonoBehaviour
{
    public PlayerHub player;
    public float cameraAimVectorDistance;
    Transform cameraAimPoint;
    bool inAimRoom;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void RecalibrateCameraAimPoint()
    {
        GameObject aimPointObj = new GameObject("EmptyObject");
        cameraAimPoint = aimPointObj.transform;
        cameraAimPoint.position = player.transform.position;
        player.cameraAimPoint = cameraAimPoint;
        inAimRoom = SceneManager.Instance.vcam != null;
    }

    public void HandleCameraAim(float horizontal, float vertical)
    {
        if (!inAimRoom) return;
        Vector2 aimVector = new Vector2(horizontal, vertical).normalized;
        cameraAimPoint.position = player.transform.position + (Vector3)aimVector * cameraAimVectorDistance;
    }
}
