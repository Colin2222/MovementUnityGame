using System.Collections;
using System.Collections.Generic;

public class PlayerProfile
{
	public int id;
	public string displayName;
	public Dictionary<string, float> bestTimes;
	
	public PlayerProfile(int id, string displayName){
		this.id = id;
		this.displayName = displayName;
		bestTimes = new Dictionary<string, float>();
	}
}
