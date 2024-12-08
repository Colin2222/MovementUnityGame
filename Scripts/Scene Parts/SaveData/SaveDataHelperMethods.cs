using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataHelperMethods
{
    public static void LoadInventory(Inventory inventory, SavedInventory savedInventory){
        inventory.ResetInventory(savedInventory.width, savedInventory.height);
        int i = 0;
        foreach (SavedInventoryItem item in savedInventory.contents){
            if(item != null && item.quantity > 0){
                inventory.contents[(int)(i / savedInventory.width), (i % savedInventory.width)] = new InventoryItem(ItemRegistry.Instance().GetItem(item.item_name), item.quantity);
            }
            i++;
        }
    }
}
