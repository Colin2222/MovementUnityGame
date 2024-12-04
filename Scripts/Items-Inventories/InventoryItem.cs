using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    public Item item;
	public int quantity;
	public int maxQuantity;
	
	public InventoryItem(Item item, int quantity, int maxQuantity = 10000){
		this.item = item;
		this.quantity = quantity;
		this.maxQuantity = maxQuantity;
	}
	
	public InventoryItem(){
		
	}
}
