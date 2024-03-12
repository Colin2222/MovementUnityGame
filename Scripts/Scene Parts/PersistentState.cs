using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.AddressableAssets;
using Newtonsoft.Json;

public class PersistentState : MonoBehaviour
{
    public int entranceNumber;
	public GameSaveData saveData;
	string addressHeader = "Assets/Data/SaveData/";

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
	
	public void CreateNewSave(){
		// load in json of fresh save into TextAsset
		var operation = Addressables.LoadAssetAsync<TextAsset>(addressHeader + "fresh_save.txt");
		TextAsset freshSaveTxt = operation.WaitForCompletion();
		
		// parse json into cutscene object
		saveData = JsonConvert.DeserializeObject<GameSaveData>(freshSaveTxt.text);
		
		// done parsing json, release asset out of memory
		Addressables.Release(operation);
	}
	
	public void LoadSave(string playerName){
		
	}
}
