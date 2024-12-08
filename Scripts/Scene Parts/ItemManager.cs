using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public SceneManager sceneManager;
    public GameObject worldItemPrefab;

    void Start(){
        LoadWorldItems(sceneManager.GetCurrentRoomSave().world_items);
    }

    public void LoadWorldItems(List<SavedWorldItem> savedItems){
        foreach(SavedWorldItem item in savedItems){
			GameObject obj = Instantiate(worldItemPrefab, new Vector3(item.x_pos, item.y_pos, 0), Quaternion.identity);
			obj.GetComponent<WorldItem>().item_id = item.item_id;
		}
    }

    public List<SavedWorldItem> GetWorldItems(){
        List<SavedWorldItem> items = new List<SavedWorldItem>();
		GameObject[] objs = GameObject.FindGameObjectsWithTag("Item");
		foreach(GameObject obj in objs){
            SavedWorldItem savedItem = new SavedWorldItem();
            WorldItem worldItem = obj.GetComponent<WorldItem>();
            savedItem.item_id = worldItem.item_id;
            savedItem.x_pos = obj.transform.position.x;
            savedItem.y_pos = obj.transform.position.y;
			items.Add(savedItem);
		}
        return items;
    }
}
