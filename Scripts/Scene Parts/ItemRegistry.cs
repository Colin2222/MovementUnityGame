using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.AddressableAssets;
using Newtonsoft.Json;

public class ItemRegistry
{
	static ItemRegistry inst;
	Dictionary<string, Item> items;
	bool loaded;
	public static ItemRegistry Instance(){
		if(inst == null){
			inst = new ItemRegistry();
			inst.loaded = false;
		}
		return inst;
	}
	ItemRegistry(){
		items = new Dictionary<string, Item>();
	}
	
	public void LoadItems(string registryXmlFilePath){
		if(!loaded){
			// load in json of player's save into TextAsset
			var operation = Addressables.LoadAssetAsync<TextAsset>("Assets/Data/Items/" + registryXmlFilePath + ".xml");
			TextAsset txtAsset = operation.WaitForCompletion();
			
			// parse json into cutscene object
			ItemJsonList itemJson = JsonConvert.DeserializeObject<ItemJsonList>(txtAsset.text);
			foreach(RegistryItem item in itemJson.registry_items){
				var spriteOperation = Addressables.LoadAssetAsync<Sprite>("Assets/Data/Items/ItemIcons/" + item.icon);
				Sprite spriteAsset = spriteOperation.WaitForCompletion();
				Item insertion = new Item(item.id, item.name, item.type, spriteAsset, item.stack_size);
				items.Add(insertion.id, insertion);
			}
			
			// done parsing json, release asset out of memory
			Addressables.Release(operation);
			loaded = true;
		}
	}
	
	public Item GetItem(string key){
		return items[key];
	}

	public bool isLoaded(){
		return loaded;
	}
}

[System.Serializable]
class ItemJsonList{
	public List<RegistryItem> registry_items;
}

[System.Serializable]
class RegistryItem{
	public string id;
	public string name;
	public int type;
	public string icon;
	public int stack_size;
}
