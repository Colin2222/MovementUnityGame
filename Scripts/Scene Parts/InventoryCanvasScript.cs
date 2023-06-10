using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCanvasScript : MonoBehaviour
{
	public GameObject closedBagObject;
	public GameObject openBagObject;
	public RectTransform gems;
	public GameObject gemsObject;
	public RectTransform items;
	public GameObject itemsObject;
	public RectTransform cassettes;
	public GameObject cassettesObject;
	
	GameObject[,] gemSlots;
	GameObject[,] itemSlots;
	GameObject[,] cassetteSlots;
	
	public int slotSize;
	public GameObject slotPrefab;
	public Color defaultSlotColor;
	public Color selectedSlotColor;
	(int y, int x) selection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void ChangeSelection(int y, int x, int id){
		GameObject[,] currentSlots;
		switch(id){
			case 0:
				currentSlots = itemSlots;
				break;
			case 1:
				currentSlots = gemSlots;
				break;
			case 2:
				currentSlots = cassetteSlots;
				break;
			default:
				currentSlots = itemSlots;
				break;
		}
		
		currentSlots[selection.y, selection.x].GetComponent<Image>().color = defaultSlotColor;
		currentSlots[y, x].GetComponent<Image>().color = selectedSlotColor;
		selection = (y, x);
	}
	
	public void ChangePage(int prevId, int newId){
		GameObject[,] oldSlots;
		switch(prevId){
			case 0:
				oldSlots = itemSlots;
				break;
			case 1:
				oldSlots = gemSlots;
				break;
			case 2:
				oldSlots = cassetteSlots;
				break;
			default:
				oldSlots = itemSlots;
				break;
		}
		oldSlots[selection.y, selection.x].GetComponent<Image>().color = defaultSlotColor;
		
		GameObject[,] currentSlots;
		switch(newId){
			case 0:
				currentSlots = itemSlots;
				break;
			case 1:
				currentSlots = gemSlots;
				break;
			case 2:
				currentSlots = cassetteSlots;
				break;
			default:
				currentSlots = itemSlots;
				break;
		}
		
		currentSlots[0, 0].GetComponent<Image>().color = selectedSlotColor;
		selection = (0, 0);
	}
	
	public void SetIcon(int x, int y, int id, Sprite icon){
		// THIS currentSlots code is repeated in ChangeSelection above, make a helper method to cut this down
		GameObject[,] currentSlots;
		switch(id){
			case 0:
				currentSlots = itemSlots;
				break;
			case 1:
				currentSlots = gemSlots;
				break;
			case 2:
				currentSlots = cassetteSlots;
				break;
			default:
				currentSlots = itemSlots;
				break;
		}
		
		currentSlots[y, x].transform.GetChild(0).gameObject.SetActive(true);
		currentSlots[y, x].transform.GetChild(0).GetComponent<Image>().sprite = icon;
	}
	
	public void SyncInventory(Inventory inventory){
		gemSlots = new GameObject[inventory.gemInventoryHeight, inventory.gemInventoryWidth];
		for(int i = 0; i < inventory.gemInventoryHeight; i++){
			for(int j = 0; j < inventory.gemInventoryWidth; j++){
				gemSlots[i, j] = Instantiate(slotPrefab, gems, false);
				gemSlots[i, j].GetComponent<RectTransform>().sizeDelta = new Vector2(slotSize, slotSize);
				gemSlots[i, j].GetComponent<Image>().color = defaultSlotColor;
				gemSlots[i, j].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(slotSize, slotSize);
				gemSlots[i, j].transform.localPosition = new Vector3(j * slotSize, -i * slotSize, 0);
				gemSlots[i, j].name = "Slot[" + i + "," + j + "]";
			}
		}
		
		itemSlots = new GameObject[inventory.itemInventoryHeight, inventory.itemInventoryWidth];
		for(int i = 0; i < inventory.itemInventoryHeight; i++){
			for(int j = 0; j < inventory.itemInventoryWidth; j++){
				itemSlots[i, j] = Instantiate(slotPrefab, items, false);
				itemSlots[i, j].GetComponent<RectTransform>().sizeDelta = new Vector2(slotSize, slotSize);
				itemSlots[i, j].GetComponent<Image>().color = defaultSlotColor;
				itemSlots[i, j].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(slotSize, slotSize);
				itemSlots[i, j].transform.localPosition = new Vector3(j * slotSize, -i * slotSize, 0);
				itemSlots[i, j].name = "Slot[" + i + "," + j + "]";
			}
		}
		
		cassetteSlots = new GameObject[inventory.cassetteInventoryHeight, inventory.cassetteInventoryWidth];
		for(int i = 0; i < inventory.cassetteInventoryHeight; i++){
			for(int j = 0; j < inventory.cassetteInventoryWidth; j++){
				cassetteSlots[i, j] = Instantiate(slotPrefab, cassettes, false);
				cassetteSlots[i, j].GetComponent<RectTransform>().sizeDelta = new Vector2(slotSize, slotSize);
				cassetteSlots[i, j].GetComponent<Image>().color = defaultSlotColor;
				cassetteSlots[i, j].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(slotSize, slotSize);
				cassetteSlots[i, j].transform.localPosition = new Vector3(j * slotSize, -i * slotSize, 0);
				cassetteSlots[i, j].name = "Slot[" + i + "," + j + "]";
			}
		}
	}
}
