using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCPrefabRegistry : MonoBehaviour
{
	Dictionary<string, NPCPrefabEntry> npcPrefabs;
    public NPCPrefabEntry[] NPCPrefabEntries;

    void Awake(){
        npcPrefabs = new Dictionary<string, NPCPrefabEntry>();
        foreach(NPCPrefabEntry entry in NPCPrefabEntries){
            npcPrefabs.Add(entry.id, entry);
        }
    }
	
	public NPCPrefabEntry GetEntry(string key){
		return npcPrefabs[key];
	}

    public GameObject GetPrefab(string key){
        return npcPrefabs[key].prefab;
    }
}

[System.Serializable]
public class NPCPrefabEntry{
    public string id;
    public GameObject prefab;
}
