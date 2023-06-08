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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void SyncInventory(Inventory inventory){
		gemSlots = new GameObject[inventory.gemInventoryHeight, inventory.gemInventoryWidth];
		for(int i = 0; i < inventory.gemInventoryHeight; i++){
			for(int j = 0; j < inventory.gemInventoryWidth; j++){
				gemSlots[i, j] = Instantiate(slotPrefab, gems, false);
				gemSlots[i, j].transform.localPosition = new Vector3(j * slotSize, -i * slotSize, 0);
				gemSlots[i, j].name = "Slot[" + i + "," + j + "]";
			}
		}
	}
}
