using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPixelStabilizer : MonoBehaviour
{
	public Transform camTran;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void LateUpdate(){
		Vector3 originalPos = camTran.position;
		Vector3 roundedPos = originalPos * 16;
		roundedPos.x = Mathf.Round(roundedPos.x);
		roundedPos.y = Mathf.Round(roundedPos.y);
		roundedPos = roundedPos / 16;
		Debug.Log(camTran.position);
		camTran.position = roundedPos;
	}
}
