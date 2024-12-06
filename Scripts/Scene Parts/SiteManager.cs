using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteManager : MonoBehaviour
{
    public SceneManager sceneManager;
    Dictionary<int, Site> sites;
    Dictionary<int, SiteSlot> siteSlots;

    void Awake(){
        DetectSites();
    }
    // Start is called before the first frame update
    void Start()
    {
        LoadSites(sceneManager.GetCurrentRoomSave().site_slots);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DetectSites(){
        sites = new Dictionary<int, Site>();
        siteSlots = new Dictionary<int, SiteSlot>();
        GameObject[] siteObjectArray = GameObject.FindGameObjectsWithTag("SiteSlot");
        foreach(GameObject siteObj in siteObjectArray){
            SiteSlot slot = siteObj.GetComponent<SiteSlot>();
            siteSlots.Add(slot.id, slot);
            sites.Add(slot.id, slot.site);
        }
    }

    public void LoadSites(List<SavedSiteSlot> savedSlots){
        SitePrefabRegistry registry = sceneManager.sitePrefabRegistry;
        foreach(SavedSiteSlot slot in savedSlots){
            if(slot.site.name != "none"){
                Instantiate(registry.GetPrefab(slot.site.name), siteSlots[slot.id].transform.position, Quaternion.identity);
            }
        }
    }
}
