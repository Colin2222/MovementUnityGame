using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrashableCornerScript : MonoBehaviour
{
    public FillScript fill;
    public int thrashCount = 3;
    int currentThrash = 0;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool Thrash()
    {
        if(fill.active == false){
            return false;
        }

        currentThrash++;
        if(currentThrash >= thrashCount){
            fill.active = false;
            fill.gameObject.SetActive(false);
            return true;
        }
        return false;
    }
}
