using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData{
	public Dictionary<string, bool> progress_markers;
	public Dictionary<string, int> integer_markers;
	public Dictionary<string, SavedRoom> rooms;
	public SavedInventory player_inventory;
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
	public List<SavedWorldItem> world_items;
	public List<SavedNPC> npcs;
	public Dictionary<string, SavedFill> fills;
}

[System.Serializable]
public class SavedInventoryItem{
	public string item_name;
	public int quantity;

	public SavedInventoryItem(string item_name, int quantity){
		this.item_name = item_name;
		this.quantity = quantity;
	}
}

[System.Serializable]
public class SavedInventory{
	public int width;
	public int height;
	public SavedInventoryItem[] contents;
}

[System.Serializable]
public class SavedWorldItem{
	public string item_id;
	public float x_pos;
	public float y_pos;
}

[System.Serializable]
public class SavedNPC{
	public string name;
	public float x_pos;
	public float y_pos;
	public string default_animation;
	public List<SavedInventory> inventories;
}

[System.Serializable]
public class SavedFill{
	public string id;
	public bool active;
}

public class RoomMappingData{
	public Dictionary<string, PulleyAnchorPointMapping> anchor_points;
	public Dictionary<string, string> build_to_name;
}

public class PulleyAnchorPointMapping{
	public int build_index;
	public int entrance_index;
}
