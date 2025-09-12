using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SiteSmelteryPanel : MonoBehaviour
{
    public Transform items;
    public GameObject itemsObject;

    GameObject[,] itemSlots;
    GameObject buttonObject;

    public float slotSize;
    public GameObject slotPrefab;
    public GameObject buttonPrefab;
    public Color defaultSlotColor;
    public Color currentSlotColor;

    (int y, int x) selection;
    GameObject selectionSlot;
    public Color selectionSlotColor;

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
			itemSlots[y, x].GetComponent<SpriteRenderer>().color = currentSlotColor;
			selection = (y, x);
		}
	}

	public void StartSelection(int y, int x){
		selectionSlot.transform.localPosition = new Vector3(x * slotSize, -y * slotSize, 0);
		selectionSlot.SetActive(true);
	}

	public void EndSelection(){
		selectionSlot.SetActive(false);
	}

    public void SetIcon(int y, int x, Sprite icon, int quantity){
		itemSlots[y, x].transform.GetChild(0).gameObject.SetActive(icon != null);
		itemSlots[y, x].transform.GetChild(1).gameObject.SetActive(icon != null);
		itemSlots[y, x].transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = icon == null ? "" : quantity.ToString();
		itemSlots[y, x].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = icon;
	}

    public void SyncInventory(Inventory inventory)
    {
        itemSlots = new GameObject[inventory.height + 1, inventory.width];
        AddConstructButton();
        for (int i = 0; i < inventory.height; i++)
        {
            for (int j = 0; j < inventory.width; j++)
            {
                itemSlots[i, j] = Instantiate(slotPrefab, items, false);
                itemSlots[i, j].GetComponent<Transform>().localScale = new Vector3(slotSize, slotSize, 1);
                itemSlots[i, j].GetComponent<SpriteRenderer>().color = defaultSlotColor;
                itemSlots[i, j].transform.localPosition = new Vector3(j * slotSize, -i * slotSize, 0);
                itemSlots[i, j].name = "Slot[" + i + "," + j + "]";
            }
        }

		// initiate selection slot
		selectionSlot = Instantiate(slotPrefab, items, false);
		selectionSlot.GetComponent<Transform>().localScale = new Vector3(slotSize, slotSize, 0);
		selectionSlot.GetComponent<SpriteRenderer>().color = selectionSlotColor;
		selectionSlot.SetActive(false);
	}
    
    public void AddConstructButton()
    {
        buttonObject = Instantiate(buttonPrefab, items, false);
        buttonObject.GetComponent<Transform>().localScale = new Vector3(slotSize, slotSize, 1);
        buttonObject.transform.localPosition = new Vector3(1, -slotSize, 0);
        buttonObject.transform.GetChild(0).GetComponent<TMP_Text>().text = "Smelt";
        buttonObject.name = "Button";

        itemSlots[1, 0] = buttonObject;
    }
}
