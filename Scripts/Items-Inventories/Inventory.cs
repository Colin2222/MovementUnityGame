using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	public int gemInventoryWidth;
	public int gemInventoryHeight;
	public int itemInventoryWidth;
	public int itemInventoryHeight;
	public int cassetteInventoryWidth;
	public int cassetteInventoryHeight;
	
	[System.NonSerialized]
	public SubInventory gemInventory;
	[System.NonSerialized]
	public SubInventory itemInventory;
	[System.NonSerialized]
	public SubInventory cassetteInventory;
	
	[System.NonSerialized]
	public bool hasGemInventory;
	[System.NonSerialized]
	public bool hasItemInventory;
	[System.NonSerialized]
	public bool hasCassetteInventory;
    // Start is called before the first frame update
    void Start()
    {
        gemInventory = new SubInventory(gemInventoryHeight, gemInventoryWidth, 1);
		/*
		for(int i = 0; i < gemInventoryHeight; i++){
			for(int j = 0; j < gemInventoryWidth; j++){
				gemInventory[i, j] = null;
			}
		}
		*/
        itemInventory = new SubInventory(itemInventoryHeight, itemInventoryWidth, 0);
        cassetteInventory = new SubInventory(cassetteInventoryHeight, cassetteInventoryWidth, 2);
    }

    public void AddItem(Item insertion, int quantity){
		SubInventory currentInven;
		switch(insertion.type){
			case 0:
				currentInven = itemInventory;
				break;
			case 1:
				currentInven = gemInventory;
				break;
			case 2:
				currentInven = cassetteInventory;
				break;
			default:
				currentInven = itemInventory;
				break;
		}
		(int sCoordX, int sCoordY) startCoordinates = (0, 0);
		
		while(quantity > 0){
			(int invenX, int invenY, int num) searchData = FindOpenSlot(currentInven, insertion, startCoordinates);
			if(searchData.invenX != -1){
				currentInven.contents[searchData.invenX, searchData.invenY].item = insertion;
				currentInven.contents[searchData.invenX, searchData.invenY].quantity += Mathf.Clamp(quantity, 0, searchData.num);
				quantity -= Mathf.Clamp(quantity, 0, searchData.num);
			}
			startCoordinates = (searchData.invenX + 1, searchData.invenY);
		}
	}
	
	// returns tuple with first two values as inventory coordinates. third value is the amount that can fit in the slot
	(int invenX, int invenY, int num) FindOpenSlot(SubInventory currentInven, Item insertion, (int startX, int startY) startCoordinates){
		// check rest of first row
		for(int j = startCoordinates.startX; j < currentInven.width; j++){
			if(currentInven.contents[startCoordinates.startY, j] == null){
				currentInven.contents[startCoordinates.startY, j] = new InventoryItem();
			} else if(currentInven.contents[startCoordinates.startY, j].item.id == insertion.id){
				return (j, startCoordinates.startY, insertion.stackSize - currentInven.contents[startCoordinates.startY, j].quantity);
			}
		}
		
		// check subsequent rows entirely
		for(int i = startCoordinates.startY; i < currentInven.height; i++){
			for(int j = 0; j < currentInven.width; j++){
				if(currentInven.contents[i, j].item == null || currentInven.contents[i, j].item.id == insertion.id){
					return (j, i, insertion.stackSize - currentInven.contents[i, j].quantity);
				}
			}
		}
		
		return (-1, -1, 0);
	}
	
	
	void checkInventoryPresence(){
		hasGemInventory = gemInventoryWidth > 0 && gemInventoryHeight > 0;
		hasItemInventory = itemInventoryWidth > 0 && itemInventoryHeight > 0;
		hasCassetteInventory = cassetteInventoryWidth > 0 && cassetteInventoryHeight > 0;
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
