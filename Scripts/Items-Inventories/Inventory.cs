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
	public InventoryItem[,] gemInventory;
	[System.NonSerialized]
	public InventoryItem[,] itemInventory;
	[System.NonSerialized]
	public InventoryItem[,] cassetteInventory;
	
	[System.NonSerialized]
	public bool hasGemInventory;
	[System.NonSerialized]
	public bool hasItemInventory;
	[System.NonSerialized]
	public bool hasCassetteInventory;
    // Start is called before the first frame update
    void Start()
    {
        gemInventory = new InventoryItem[gemInventoryHeight, gemInventoryWidth];
		/*
		for(int i = 0; i < gemInventoryHeight; i++){
			for(int j = 0; j < gemInventoryWidth; j++){
				gemInventory[i, j] = null;
			}
		}
		*/
        itemInventory = new InventoryItem[itemInventoryHeight, itemInventoryWidth];
        cassetteInventory = new InventoryItem[cassetteInventoryHeight, cassetteInventoryWidth];
    }

    public void AddInventoryItem(InventoryItem insertion){
		
	}
	
	(int, int) FindOpenSlot(InventoryItem[,] currentInven, InventoryItem insertion){
		return (0,0);
	}
	
	
	void checkInventoryPresence(){
		hasGemInventory = gemInventoryWidth > 0 && gemInventoryHeight > 0;
		hasItemInventory = itemInventoryWidth > 0 && itemInventoryHeight > 0;
		hasCassetteInventory = cassetteInventoryWidth > 0 && cassetteInventoryHeight > 0;
	}
}
