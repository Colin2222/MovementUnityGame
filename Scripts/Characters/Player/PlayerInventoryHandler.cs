using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryHandler : MonoBehaviour
{	
    public InventoryCanvasScript canvas;
	public Inventory inventory;
	//[Header("0=Item, 1=Gem, 2=Cassette")]
	public int invWidth = 4;
	public int invHeight = 4;

	public GameObject itemPrefab;
	(int x, int y) currentSlotPos;
	(int x, int y) selectionPos;
	bool inSelection = false;
	bool isOpen = false;
	
	List<WorldItem> reachableItems;
	
	void Awake(){
		SaveDataHelperMethods.LoadInventory(inventory, SceneManager.Instance.sessionManager.saveData.player_inventory);
	}

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void SceneSwitchReset(){
		GameObject canvasObj = GameObject.FindWithTag("InventoryUI");
        if(canvasObj != null){
			canvas = canvasObj.GetComponent<InventoryCanvasScript>();
			canvas.SyncInventory(inventory);
		}
		reachableItems = new List<WorldItem>();
	}

	public bool ToggleInventory(){
		isOpen = !isOpen;
		canvas.openBagObject.SetActive(isOpen);
		
		if(isOpen){
			canvas.itemsObject.SetActive(true);
			currentSlotPos = (0, 0);
			canvas.ChangeSelection(currentSlotPos.y, currentSlotPos.x);
			UpdateIcons();
		} else{
			canvas.EndSelection();
			inSelection = false;
			canvas.itemsObject.SetActive(false);
		}
		
		return isOpen;
	}
	
	public void MoveUp(){
		if(currentSlotPos.y > 0){
			(currentSlotPos.y)--;
			canvas.ChangeSelection(currentSlotPos.y, currentSlotPos.x);
		}
	}
	
	public void MoveDown(){
		if(currentSlotPos.y < inventory.height - 1){
			(currentSlotPos.y)++;
			canvas.ChangeSelection(currentSlotPos.y, currentSlotPos.x);
		}
	}
	
	public void MoveRight(){
		if(currentSlotPos.x < inventory.width - 1){
			(currentSlotPos.x)++;
			canvas.ChangeSelection(currentSlotPos.y, currentSlotPos.x);
		}
	}
	
	public void MoveLeft(){
		if(currentSlotPos.x > 0){
			(currentSlotPos.x)--;
			canvas.ChangeSelection(currentSlotPos.y, currentSlotPos.x);
		}
	}

	public void MenuSelect(){
		if(inSelection){
			if(selectionPos == currentSlotPos){
				inSelection = false;
				canvas.EndSelection();
			} else{
				inventory.SwapItem(selectionPos.x, selectionPos.y, currentSlotPos.x, currentSlotPos.y);
				UpdateIcons();
				canvas.EndSelection();
				inSelection = false;
			}
		} else{
			selectionPos = currentSlotPos;
			canvas.StartSelection(currentSlotPos.y, currentSlotPos.x);
			inSelection = true;
		}
	}
	
	public bool Pickup(){
		if(reachableItems.Count > 0){
			WorldItem pickup = reachableItems[0];
			Item pickupItem = pickup.item;
			reachableItems.RemoveAt(0);
			Destroy(pickup.gameObject);
			inventory.AddItem(pickupItem, 1);
			UpdateIcons();
			return true;
		}
		return false;
	}

	public void DropCurrentItem(){
		if(inventory.contents[currentSlotPos.y, currentSlotPos.x] != null){
			Item dropItem = inventory.contents[currentSlotPos.y, currentSlotPos.x].item;
			inventory.RemoveItem(currentSlotPos.x, currentSlotPos.y, 1);
			WorldItem droppedItem = Instantiate(itemPrefab, transform.position, Quaternion.identity).GetComponent<WorldItem>();
			droppedItem.item_id = dropItem.id;
			UpdateIcons();
		}
	}
	
	// TODO: MAKE FUNCTIONS TO ONLY CHANGE THE RELEVANT ICONS IN THE INVENTORY
	public void UpdateIcons(){
		for(int i = 0; i < inventory.height; i++){
			for(int j = 0; j < inventory.width; j++){
				if(inventory.contents[i, j] != null){
					canvas.SetIcon(i, j, inventory.contents[i, j].item.icon, inventory.contents[i, j].quantity);
				} else{
					canvas.SetIcon(i, j, null, 0);
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
