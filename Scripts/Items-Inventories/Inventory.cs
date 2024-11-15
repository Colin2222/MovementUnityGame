using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	public InventoryItem[,] contents;
	public int width;
	public int height;
	public int id;
    // Start is called before the first frame update
    void Start()
    {
		contents = new InventoryItem[height, width];
    }

    public void AddItem(Item insertion, int quantity){
		(int invenX, int invenY) searchData = FindOpenSlot(insertion);
		Debug.Log(searchData);
		if(searchData.invenX != -1){
			contents[searchData.invenY, searchData.invenX].item = insertion;
			contents[searchData.invenY, searchData.invenX].quantity += quantity;
		}
	}
	
	// returns tuple of inventory coordinates to place insertion
	// searches for existing stack of insertion item, returns empty slot if none found
	(int xIndex, int yIndex) FindOpenSlot(Item insertion){
		(int xIndex, int yIndex) firstEmptySlot = (-1, -1);	
		for(int i = 0; i < height; i++){
			for(int j = 0; j < width; j++){
				if(contents[i, j].item == null && firstEmptySlot == (-1, -1)){
					firstEmptySlot = (j, i);
				} else if(contents[i, j].item.id == insertion.id){
					return (j, i);
				}
			}
		}
		
		return firstEmptySlot;
	}
}

public class SubInventory{
	public InventoryItem[,] contents;
	public int width;
	public int height;
	public int id;
	
	public SubInventory(int height, int width, int id){
		this.contents = new InventoryItem[height, width];
		this.width = width;
		this.height = height;
		this.id = id;
	}
}
