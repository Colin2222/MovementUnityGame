using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData{
	public Dictionary<string, Quest> completed_quests;
	public Dictionary<string, Quest> current_quests;
	public Dictionary<string, Quest> uncompleted_quests;
	public Dictionary<string, Room> rooms;
}

[System.Serializable]
public class Quest{
	public string id;
	public string display_name;
	public List<string> decision_tags;
}

[System.Serializable]
public class Site{
	public int site_id;
	public string display_name;
	public bool active;
}

[System.Serializable]
public enum SiteTag{
	GENERAL,
	BUILDING,
	WELL
}

[System.Serializable]
public class Room{
	public string id;
	public string display_name;
	public List<Site> sites;
}
