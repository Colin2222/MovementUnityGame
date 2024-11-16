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
	
	public void ChangeSelection(int y, int x){		
		itemSlots[selection.y, selection.x].GetComponent<Image>().color = defaultSlotColor;
		itemSlots[y, x].GetComponent<Image>().color = selectedSlotColor;
		selection = (y, x);
	}
	
	public void SetIcon(int y, int x, Sprite icon){
		itemSlots[y, x].transform.GetChild(0).gameObject.SetActive(true);
		itemSlots[y, x].transform.GetChild(0).GetComponent<Image>().sprite = icon;
	}
	
	public void SyncInventory(Inventory inventory){
		itemSlots = new GameObject[inventory.height, inventory.width];
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
	}
}
