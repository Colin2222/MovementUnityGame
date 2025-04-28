using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackdropManager : MonoBehaviour
{
    public Sprite[] backdrops;
    public bool backdropActive = true;

    public void SetBackdrop(){
        if(!backdropActive) return;
        
        SpriteRenderer sr = GameObject.FindWithTag("MainCamera").transform.GetChild(0).GetComponent<SpriteRenderer>();
        int backdropIndex = SessionManager.Instance.GetIntegerMarker("time_marker");
        sr.sprite = backdrops[backdropIndex];
    }
}
