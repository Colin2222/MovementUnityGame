using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData{
	public Dictionary<string, bool> progressMarkers;
	public Dictionary<string, SavedRoom> rooms;
}

// the "slot" in the game world that the player can build things on
[System.Serializable]
public class SavedSite{
	public int id;
	public int type;
	public string display_name;
	public bool active;
	public SavedStructure structure;
}

// the sometimes interactable objects that the player can build on sites
// think pulley anchor point, smeltery, water wheel, house
[System.Serializable]
public class SavedStructure{
	public int type;
	public bool constructed;
	//public Dictionary<string, int>;
}

[System.Serializable]
public class SavedRoom{
	public string id;
	public List<SavedSite> sites;
}
