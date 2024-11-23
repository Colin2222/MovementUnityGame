using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
	public string id;
	public string name;
	public int type;
	public Sprite icon;
	public int stackSize;
	
	public Item(string id, string name, int type, Sprite icon, int stackSize){
		this.id = id;
		this.name = name;
		this.type = type;
		this.icon = icon;
		this.stackSize = stackSize;
	}
}
