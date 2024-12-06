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
        Debug.Log(sitePrefabs.Count);
    }
	
	public SitePrefabEntry GetEntry(string key){
		return sitePrefabs[key];
	}

    public GameObject GetPrefab(string key){
        return sitePrefabs[key].prefab;
    }
}

[System.Serializable]
public class SitePrefabEntry{
    public string id;
    public GameObject prefab;
    public ConstructionRequirement[] requirements;
}
