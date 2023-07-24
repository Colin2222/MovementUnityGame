using System.Collections;
using System.Collections.Generic;

public class PlayerProfile
{
	public int id;
	public string displayName;
	public Dictionary<int, float> bestTimes;
	
	public PlayerProfile(int id, string displayName){
		this.id = id;
		this.displayName = displayName;
		bestTimes = new Dictionary<int, float>();
	}
}
