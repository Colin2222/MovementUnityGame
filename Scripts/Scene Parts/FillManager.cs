using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillManager : MonoBehaviour
{
    Dictionary<string, FillScript> fills;

    // Start is called before the first frame update
    void Start()
    {
        LoadFills(SceneManager.Instance.GetCurrentRoomSave().fills);
    }

    void DetectFills(){
        fills = new Dictionary<string, FillScript>();
        GameObject[] fillObjectArray = GameObject.FindGameObjectsWithTag("Fill");
        foreach(GameObject fillObj in fillObjectArray){
            FillScript fill = fillObj.GetComponent<FillScript>();
            fills.Add(fill.id.ToString(), fill);
        }
    }

    public Dictionary<string, SavedFill> SaveFills(){
        DetectFills();
        Dictionary<string, SavedFill> savedFills = new Dictionary<string, SavedFill>();
        foreach(KeyValuePair<string, FillScript> entry in fills){
            SavedFill savedFill = new SavedFill();
            savedFill.id = entry.Value.id.ToString();
            savedFill.active = entry.Value.active;
            savedFills.Add(entry.Key, savedFill);
        }
        return savedFills;
    }

    public void LoadFills(Dictionary<string, SavedFill> savedFills){
        DetectFills();
        foreach(KeyValuePair<string, SavedFill> entry in savedFills){
            if(fills.ContainsKey(entry.Key)){
                FillScript fill = fills[entry.Key];
                fill.transform.GetChild(0).gameObject.SetActive(entry.Value.active);
                fill.active = entry.Value.active;
            } else{
                Debug.Log("Fill with ID " + entry.Key + " not found in scene.");
            }
        }
    }

    public void SetFill(string id, bool active){
        if(fills.ContainsKey(id)){
            FillScript fill = fills[id];
            fill.transform.GetChild(0).gameObject.SetActive(active);
            fill.active = active;
        } else{
            Debug.Log("Fill with ID " + id + " not found in scene.");
        }
    }
}
