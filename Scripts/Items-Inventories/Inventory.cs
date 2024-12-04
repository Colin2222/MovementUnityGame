using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	public InventoryItem[,] contents;
	public int[,] maxQuantities;
	public string[,] slotTypes;
	public int width;
	public int height;
	public int id;
    
	void Awake(){
		// initialize max quantities to 10000, default value
		// owner sites can override this value
		maxQuantities = new int[height, width];
		for(int i = 0; i < height; i++){
			for(int j = 0; j < width; j++){
				maxQuantities[i, j] = 10000;
			}
		}
		slotTypes = new string[height, width];
	}
    void Start()
    {
		contents = new InventoryItem[height, width];
    }

    public void AddItem(Item insertion, int quantity){
		(int invenX, int invenY) searchData = FindOpenSlot(insertion);
		if(searchData.invenX != -1){
			contents[searchData.invenY, searchData.invenX].item = insertion;
			contents[searchData.invenY, searchData.invenX].quantity += quantity;
		}
	}

	public void SwapItem(int x1, int y1, int x2, int y2){
		InventoryItem temp = contents[y1, x1];
		contents[y1, x1] = contents[y2, x2];
		contents[y2, x2] = temp;
	}

	public void TradeItem(Inventory otherInventory, int xOther, int yOther, int xThis, int yThis){
		InventoryItem temp = contents[yThis, xThis];
		contents[yThis, xThis] = otherInventory.contents[yOther, xOther];
		otherInventory.contents[yOther, xOther] = temp;
	}

	// meant for inserting items into an empty or existing stack (of the same item of course)
	public void InsertItem(Inventory otherInventory, int xOther, int yOther, int xThis, int yThis){
		// check if the receiving slot is compatible with the item being inserted
		if(slotTypes[yThis, xThis] != null && slotTypes[yThis, xThis] != otherInventory.contents[yOther, xOther].item.id){
			return;
		}

		// check if the receiving slot is empty
		if(contents[yThis, xThis] != null){
			// check if the items are the same
			if(contents[yThis, xThis].item.id == otherInventory.contents[yOther, xOther].item.id){
				int totalQuantity = contents[yThis, xThis].quantity + otherInventory.contents[yOther, xOther].quantity;
				if(totalQuantity <= maxQuantities[yThis, xThis]){
				contents[yThis, xThis] = otherInventory.contents[yOther, xOther];
				contents[yThis, xThis].quantity = totalQuantity;
				otherInventory.contents[yOther, xOther] = null;
				} else{
					contents[yThis, xThis].quantity = maxQuantities[yThis, xThis];
					otherInventory.contents[yOther, xOther].quantity = totalQuantity - maxQuantities[yThis, xThis];
				}
			} else{
				// check there would be no quantities going over maximums from swapping and other slot is compatible slot
				if(!(contents[yThis, xThis].quantity > otherInventory.maxQuantities[yOther, xOther] || otherInventory.contents[yOther, xOther].quantity > maxQuantities[yThis, xThis])){
					// check if other slot is compatible with item from this inventory
					if(otherInventory.slotTypes[yOther, xOther] == null || slotTypes[yOther, xOther] == contents[yThis, xThis].item.id){
						TradeItem(otherInventory, xOther, yOther, xThis, yThis);
					}
				}
			}
		} else{
			int totalQuantity = otherInventory.contents[yOther, xOther].quantity;
			if(totalQuantity <= maxQuantities[yThis, xThis]){
				contents[yThis, xThis] = otherInventory.contents[yOther, xOther];
				otherInventory.contents[yOther, xOther] = null;
			} else{
				contents[yThis, xThis] = new InventoryItem(otherInventory.contents[yOther, xOther].item, maxQuantities[yThis, xThis]);
				otherInventory.contents[yOther, xOther].quantity = totalQuantity - maxQuantities[yThis, xThis];
			}
			return;
		}
	}
	
	// returns tuple of inventory coordinates to place insertion
	// searches for existing stack of insertion item, returns empty slot if none found
	(int xIndex, int yIndex) FindOpenSlot(Item insertion){
		(int xIndex, int yIndex) firstEmptySlot = (-1, -1);	
		for(int i = 0; i < height; i++){
			for(int j = 0; j < width; j++){
				if(firstEmptySlot == (-1, -1) && contents[i, j] == null){
					firstEmptySlot = (j, i);
				} else if(contents[i, j] != null && contents[i, j].item.id == insertion.id){
					return (j, i);
				}
			}
		}
		
		contents[firstEmptySlot.yIndex, firstEmptySlot.xIndex] = new InventoryItem(insertion, 0);
		return firstEmptySlot;
	}

	public void ResetInventory(int width, int height){
		this.width = width;
		this.height = height;
		contents = new InventoryItem[height, width];

		// initialize max quantities to 10000, default value
		// owner sites can override this value
		maxQuantities = new int[height, width];
		for(int i = 0; i < height; i++){
			for(int j = 0; j < width; j++){
				maxQuantities[i, j] = 10000;
			}
		}
	}
}
