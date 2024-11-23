using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCanvasScript : MonoBehaviour
{
	public GameObject closedBagObject;
	public GameObject openBagObject;
	public RectTransform items;
	public GameObject itemsObject;
	
	GameObject[,] itemSlots;
	GameObject selectionSlot;
	
	
	public int slotSize;
	public GameObject slotPrefab;
	public Color defaultSlotColor;
	public Color currentSlotColor;
	public Color selectionSlotColor;
	(int y, int x) selection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void ChangeSelection(int y, int x){
		itemSlots[selection.y, selection.x].GetComponent<Image>().color = defaultSlotColor;
		if(y >= 0 && x >= 0){
			itemSlots[y, x].GetComponent<Image>().color = currentSlotColor;
			selection = (y, x);
		}
	}

	public void StartSelection(int y, int x){
		selectionSlot.transform.localPosition = new Vector3(x * slotSize, -y * slotSize, 1);
		selectionSlot.SetActive(true);
	}

	public void EndSelection(){
		selectionSlot.SetActive(false);
	}
	
	public void SetIcon(int y, int x, Sprite icon){
		itemSlots[y, x].transform.GetChild(0).gameObject.SetActive(icon != null);
		itemSlots[y, x].transform.GetChild(0).GetComponent<Image>().sprite = icon;
	}
	
	public void SyncInventory(Inventory inventory){
		// create matrix of slot gameobjects
		itemSlots = new GameObject[inventory.height, inventory.width];

		// initiate item slots
		for(int i = 0; i < inventory.height; i++){
			for(int j = 0; j < inventory.width; j++){
				itemSlots[i, j] = Instantiate(slotPrefab, items, false);
				itemSlots[i, j].GetComponent<RectTransform>().sizeDelta = new Vector2(slotSize, slotSize);
				itemSlots[i, j].GetComponent<Image>().color = defaultSlotColor;
				itemSlots[i, j].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(slotSize, slotSize);
				itemSlots[i, j].transform.localPosition = new Vector3(j * slotSize, -i * slotSize, 0);
				itemSlots[i, j].name = "Slot[" + i + "," + j + "]";
			}
		}

		// initiate selection slot
		selectionSlot = Instantiate(slotPrefab, items, false);
		selectionSlot.GetComponent<RectTransform>().sizeDelta = new Vector2(slotSize, slotSize);
		selectionSlot.GetComponent<Image>().color = selectionSlotColor;
		selectionSlot.SetActive(false);
	}
}
