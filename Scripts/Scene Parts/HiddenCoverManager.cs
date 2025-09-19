using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenCoverManager : MonoBehaviour
{
    Dictionary<string, HiddenCoverScript> hiddenCovers;

    // Start is called before the first frame update
    void Start()
    {
        LoadHiddenCovers(SceneManager.Instance.GetCurrentRoomSave().hidden_covers);
    }

    void DetectHiddenCovers(){
        hiddenCovers = new Dictionary<string, HiddenCoverScript>();
        GameObject[] hiddenCoverObjectArray = GameObject.FindGameObjectsWithTag("HiddenCover");
        foreach(GameObject hiddenCoverObj in hiddenCoverObjectArray){
            HiddenCoverScript hiddenCover = hiddenCoverObj.GetComponent<HiddenCoverScript>();
            hiddenCovers.Add(hiddenCover.id.ToString(), hiddenCover);
        }
    }

    public Dictionary<string, SavedHiddenCover> SaveHiddenCovers(){
        DetectHiddenCovers();
        Dictionary<string, SavedHiddenCover> savedHiddenCovers = new Dictionary<string, SavedHiddenCover>();
        foreach(KeyValuePair<string, HiddenCoverScript> entry in hiddenCovers){
            SavedHiddenCover savedHiddenCover = new SavedHiddenCover();
            savedHiddenCover.id = entry.Value.id.ToString();
            savedHiddenCover.discovered = entry.Value.discovered;
            savedHiddenCovers.Add(entry.Key, savedHiddenCover);
        }
        return savedHiddenCovers;
    }

    public void LoadHiddenCovers(Dictionary<string, SavedHiddenCover> savedHiddenCovers){
        DetectHiddenCovers();
        foreach(KeyValuePair<string, SavedHiddenCover> entry in savedHiddenCovers){
            if(hiddenCovers.ContainsKey(entry.Key)){
                HiddenCoverScript hiddenCover = hiddenCovers[entry.Key];
                hiddenCover.discovered = entry.Value.discovered;
            } else{
                Debug.Log("HiddenCover with ID " + entry.Key + " not found in scene.");
            }
        }
    }

    public void SetHiddenCover(string id, bool discovered){
        if(hiddenCovers.ContainsKey(id)){
            HiddenCoverScript hiddenCover = hiddenCovers[id];
            hiddenCover.discovered = discovered;
        } else{
            Debug.Log("HiddenCover with ID " + id + " not found in scene.");
        }
    }
}
