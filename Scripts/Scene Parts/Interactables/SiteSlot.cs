using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteSlot : MonoBehaviour
{
    public int id;

    [System.NonSerialized]
    public string display_name;

    public Site GetSite(){
        return GetComponentInChildren<Site>();
    }
}
