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
		renderer.sprite = item.icon;
	}
}
