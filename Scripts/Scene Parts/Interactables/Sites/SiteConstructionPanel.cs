using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SiteConstructionPanel : MonoBehaviour
{
    public Transform items;
    public GameObject itemsObject;
	
	GameObject[,] itemSlots;
	
	public float slotSize;
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
        itemSlots[selection.y, selection.x].GetComponent<SpriteRenderer>().color = defaultSlotColor;
		if(y >= 0 && x >= 0){
			itemSlots[y, x].GetComponent<SpriteRenderer>().color = selectedSlotColor;
			selection = (y, x);
		}
	}

    public void SetIcon(int y, int x, Sprite icon){
		itemSlots[y, x].transform.GetChild(0).gameObject.SetActive(true);
		itemSlots[y, x].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = icon;
	}

    public void SyncInventory(Inventory inventory){
		itemSlots = new GameObject[inventory.height, inventory.width];
		for(int i = 0; i < inventory.height; i++){
			for(int j = 0; j < inventory.width; j++){
				itemSlots[i, j] = Instantiate(slotPrefab, items, false);
				itemSlots[i, j].GetComponent<Transform>().localScale = new Vector3(slotSize, slotSize, 1);
				itemSlots[i, j].GetComponent<SpriteRenderer>().color = defaultSlotColor;
				itemSlots[i, j].transform.localPosition = new Vector3(j * slotSize, -i * slotSize, 0);
				itemSlots[i, j].name = "Slot[" + i + "," + j + "]";
			}
		}
	}
}
