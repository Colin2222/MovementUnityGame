using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public string item_id;
	[System.NonSerialized]
	public Item item;
	public SpriteRenderer renderer;
	
	void Start(){
		item = ItemRegistry.Instance().GetItem(item_id);
		Debug.Log("Item: " + item.name);
		Debug.Log("Icon: " + item.icon);
		renderer.sprite = item.icon;
	}
}
