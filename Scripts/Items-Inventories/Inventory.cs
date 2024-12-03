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

	public void TradeItem(int x1, int y1, Inventory otherInventory, int x2, int y2){
		InventoryItem temp = contents[y2, x2];
		contents[y2, x2] = otherInventory.contents[y1, x1];
		otherInventory.contents[y1, x1] = temp;
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
	}
}
