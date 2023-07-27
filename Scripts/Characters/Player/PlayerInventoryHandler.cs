using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryHandler : MonoBehaviour
{	
    public InventoryCanvasScript canvas;
	public Inventory inventory;
	[Header("0=Item, 1=Gem, 2=Cassette")]
	public SubInventory currentInventory;
	(int x, int y) currentSlotPos;
	bool isOpen = false;
	
	List<WorldItem> reachableItems;
	
	// Start is called before the first frame update
    void Start()
    {
        if(canvas != null){
			canvas.SyncInventory(inventory);
		}
		currentInventory = inventory.gemInventory;
		reachableItems = new List<WorldItem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public bool ToggleInventory(){
		isOpen = !isOpen;
		canvas.openBagObject.SetActive(isOpen);
		
		if(isOpen){
			switch(currentInventory.id){
				case 0:
					canvas.itemsObject.SetActive(true);
					break;
				case 1:
					canvas.gemsObject.SetActive(true);
					break;
				case 2:
					canvas.cassettesObject.SetActive(true);
					break;
			}
			currentSlotPos = (0, 0);
			canvas.ChangeSelection(currentSlotPos.y, currentSlotPos.x, currentInventory.id);
			UpdateIcons();
		} else{
			canvas.gemsObject.SetActive(false);
			canvas.itemsObject.SetActive(false);
			canvas.cassettesObject.SetActive(false);
		}
		
		return isOpen;
	}
	
	public void MoveUp(){
		if(currentSlotPos.y > 0){
			(currentSlotPos.y)--;
			canvas.ChangeSelection(currentSlotPos.y, currentSlotPos.x, currentInventory.id);
		}
	}
	
	public void MoveDown(){
		if(currentSlotPos.y < currentInventory.height - 1){
			(currentSlotPos.y)++;
			canvas.ChangeSelection(currentSlotPos.y, currentSlotPos.x, currentInventory.id);
		}
	}
	
	public void MoveRight(){
		if(currentSlotPos.x < currentInventory.width - 1){
			(currentSlotPos.x)++;
			canvas.ChangeSelection(currentSlotPos.y, currentSlotPos.x, currentInventory.id);
		}
	}
	
	public void MoveLeft(){
		if(currentSlotPos.x > 0){
			(currentSlotPos.x)--;
			canvas.ChangeSelection(currentSlotPos.y, currentSlotPos.x, currentInventory.id);
		}
	}
	
	public void PageRight(){
		canvas.gemsObject.SetActive(false);
		canvas.itemsObject.SetActive(false);
		canvas.cassettesObject.SetActive(false);
		
		int oldId = currentInventory.id;
		
		switch(currentInventory.id){
			case 0:
				currentInventory = inventory.gemInventory;
				canvas.gemsObject.SetActive(true);
				break;
			case 1:
				currentInventory = inventory.cassetteInventory;
				canvas.cassettesObject.SetActive(true);
				break;
			case 2:
				currentInventory = inventory.itemInventory;
				canvas.itemsObject.SetActive(true);
				break;
			default:
				break;
		}
		
		currentSlotPos = (0, 0);
		canvas.ChangePage(oldId, currentInventory.id);
		UpdateIcons();
	}
	
	public void PageLeft(){
		canvas.gemsObject.SetActive(false);
		canvas.itemsObject.SetActive(false);
		canvas.cassettesObject.SetActive(false);
		
		int oldId = currentInventory.id;
		
		switch(currentInventory.id){
			case 0:
				currentInventory = inventory.cassetteInventory;
				canvas.cassettesObject.SetActive(true);
				break;
			case 1:
				currentInventory = inventory.itemInventory;
				canvas.itemsObject.SetActive(true);
				break;
			case 2:
				currentInventory = inventory.gemInventory;
				canvas.gemsObject.SetActive(true);
				break;
			default:
				break;
		}
		
		currentSlotPos = (0, 0);
		canvas.ChangePage(oldId, currentInventory.id);
		UpdateIcons();
	}
	
	public void Pickup(){
		if(reachableItems.Count > 0){
			WorldItem pickup = reachableItems[0];
			Item pickupItem = pickup.item;
			reachableItems.RemoveAt(0);
			Destroy(pickup.gameObject);
			inventory.AddItem(pickupItem, 1);
			UpdateIcons();
		}
	}
	
	// TODO: MAKE FUNCTIONS TO ONLY CHANGE THE RELEVANT ICONS IN THE INVENTORY
	void UpdateIcons(){
		for(int i = 0; i < currentInventory.height; i++){
			for(int j = 0; j < currentInventory.width; j++){
				if(currentInventory.contents[i, j] != null){
					canvas.SetIcon(j, i, currentInventory.id, currentInventory.contents[i, j].item.icon);
				}
			}
		}
	}
	
	void OnTriggerEnter2D(Collider2D col){
		reachableItems.Add(col.GetComponent<WorldItem>());
	}
	
	void OnTriggerExit2D(Collider2D col){
		reachableItems.Remove(col.GetComponent<WorldItem>());
	}
}
