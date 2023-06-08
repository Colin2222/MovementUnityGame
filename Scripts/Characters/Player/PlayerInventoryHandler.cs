using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryHandler : MonoBehaviour
{	
    public InventoryCanvasScript canvas;
	public Inventory inventory;
	[Header("0=Item, 1=Gem, 2=Cassette")]
	public int currentInventory = 0;
	bool isOpen = false;
	
	// Start is called before the first frame update
    void Start()
    {
        canvas.SyncInventory(inventory);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void ToggleInventory(){
		isOpen = !isOpen;
		canvas.openBagObject.SetActive(isOpen);
		
		if(isOpen){
			switch(currentInventory){
				case 0:
					canvas.gemsObject.SetActive(true);
					break;
				case 1:
					canvas.itemsObject.SetActive(true);
					break;
				case 2:
					canvas.cassettesObject.SetActive(true);
					break;
			}
		} else{
			canvas.gemsObject.SetActive(false);
			canvas.itemsObject.SetActive(false);
			canvas.cassettesObject.SetActive(false);
		}
	}
}
