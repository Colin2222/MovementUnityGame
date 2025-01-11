using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteCableCarPanel : MonoBehaviour
{
    public GameObject[] optionSlots;
    public GameObject selectionSlot;
    public GameObject playerSlot;

    int numAnchorPoints;
    int currentSelection = 0;
    int playerLocation = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AscendSelection(){
        int checkIndex = (currentSelection + 1 == playerLocation) ? currentSelection + 2 : currentSelection + 1;
        if(checkIndex < numAnchorPoints){
            currentSelection = checkIndex;
            selectionSlot.transform.localPosition = optionSlots[currentSelection].transform.localPosition;
        }
    }

    public void DescendSelection(){
        int checkIndex = (currentSelection - 1 == playerLocation) ? currentSelection - 2 : currentSelection - 1;
        if(checkIndex >= 0){
            currentSelection = checkIndex;
            selectionSlot.transform.localPosition = optionSlots[currentSelection].transform.localPosition;
        }
    }

    public int SelectOption(){
        return currentSelection;
    }

    public void SyncOptions(int numAnchorPoints, int playerLocation){
        // activate the option slots
        this.numAnchorPoints = numAnchorPoints;
        for(int i = 0; i < numAnchorPoints; i++){
            optionSlots[i].SetActive(true);
        }

        // activate the player location
        this.playerLocation = playerLocation;
        playerSlot.SetActive(true);
        playerSlot.transform.localPosition = optionSlots[playerLocation].transform.localPosition;
        optionSlots[playerLocation].SetActive(false);

        // set the selection slot
        currentSelection = (playerLocation == 0) ? 1 : 0;
        selectionSlot.transform.localPosition = optionSlots[currentSelection].transform.localPosition;
    }
}
