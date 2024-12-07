using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData{
	public Dictionary<string, bool> progress_markers;
	public Dictionary<string, SavedRoom> rooms;
}

// the sometimes interactable objects that the player can build on sites
// think pulley anchor point, smeltery, water wheel, house
[System.Serializable]
public class SavedSite{
	public string name;
	public List<SavedInventory> inventories;
	public Dictionary<string, string> additional_data;
}

// the "slot" in the game world that the player can build things on
[System.Serializable]
public class SavedSiteSlot{
	public int id;
	public string display_name;
	public SavedSite site;
}

[System.Serializable]
public class SavedRoom{
	public List<SavedSiteSlot> site_slots;
}

[System.Serializable]
public class SavedInventoryItem{
	public string item_name;
	public int quantity;
}

[System.Serializable]
public class SavedInventory{
	public int width;
	public int height;
	public SavedInventoryItem[] contents;
}
