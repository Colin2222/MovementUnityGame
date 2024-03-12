using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.AddressableAssets;
using Newtonsoft.Json;

public class PersistentState : MonoBehaviour
{
    public int entranceNumber;
	public GameSaveData saveData;
	public string currentPlayerName = "player";
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
		// load in json of player's save into TextAsset
		var operation = Addressables.LoadAssetAsync<TextAsset>(addressHeader + playerName + "_save.txt");
		TextAsset freshSaveTxt = operation.WaitForCompletion();
		
		// parse json into cutscene object
		saveData = JsonConvert.DeserializeObject<GameSaveData>(freshSaveTxt.text);
		
		// done parsing json, release asset out of memory
		Addressables.Release(operation);
	}
	
	public void WriteSave(){
		// load in json of player's save into TextAsset
		var operation = Addressables.LoadAssetAsync<TextAsset>(addressHeader + currentPlayerName + "_save.txt");
		TextAsset freshSaveTxt = operation.WaitForCompletion();
		
		// serialize saveData into json
		string jsonText = JsonConvert.SerializeObject(saveData, Formatting.Indented);
		File.WriteAllText(addressHeader + currentPlayerName + "_save.txt", jsonText);
		
		// done parsing json, release asset out of memory
		Addressables.Release(operation);
	}
}
