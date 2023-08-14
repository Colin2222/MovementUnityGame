using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfile
{
	public int id;
	public string displayName;
	public string spritesheetCode;
	public Dictionary<int, float> bestTimes;
	public List<(int type, int colorId)> customizationCodes;
	
	public PlayerProfile(int id, string displayName, string spritesheetCode){
		this.id = id;
		this.displayName = displayName;
		this.spritesheetCode = spritesheetCode;
		bestTimes = new Dictionary<int, float>();
		customizationCodes = new List<(int, int)>();
	}
}
