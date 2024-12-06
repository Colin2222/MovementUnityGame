using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SitePrefabRegistry : MonoBehaviour
{
	Dictionary<string, SitePrefabEntry> sitePrefabs;
    public SitePrefabEntry[] sitePrefabEntries;

    void Awake(){
        sitePrefabs = new Dictionary<string, SitePrefabEntry>();
        foreach(SitePrefabEntry entry in sitePrefabEntries){
            sitePrefabs.Add(entry.id, entry);
        }
    }
	
	public SitePrefabEntry GetEntry(string key){
		return sitePrefabs[key];
	}
}

[System.Serializable]
public class SitePrefabEntry{
    public string id;
    public GameObject prefab;
    public ConstructionRequirement[] requirements;
}
