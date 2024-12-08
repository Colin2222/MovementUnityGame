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
            sites.Add(slot.id, slot.GetSite());
        }
    }

    public void LoadSites(List<SavedSiteSlot> savedSlots){
        SitePrefabRegistry registry = sceneManager.sitePrefabRegistry;
        foreach(SavedSiteSlot slot in savedSlots){
            if(slot.site != null){
                GameObject siteObj = Instantiate(registry.GetPrefab(slot.site.name), siteSlots[slot.id].transform.position, Quaternion.identity, siteSlots[slot.id].transform);
                Site site = siteObj.GetComponent<Site>();
                site.LoadSite(slot.site);
            }
        }
    }

    public SavedRoom SaveSites(){
        SavedRoom savedRoom = new SavedRoom();
        savedRoom.site_slots = new List<SavedSiteSlot>();
        foreach(KeyValuePair<int, SiteSlot> entry in siteSlots){
            SavedSiteSlot savedSlot = new SavedSiteSlot();
            savedSlot.id = entry.Key;
            savedSlot.display_name = entry.Value.display_name;

            if(entry.Value.GetSite() != null){
                savedSlot.site = entry.Value.GetSite().SaveSite();
            }
            savedRoom.site_slots.Add(savedSlot);
        }
        return savedRoom;
    }
}
