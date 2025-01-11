using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteCableCar : Site
{
    public SiteCableCarPanel sitePanel;
    public int anchorIndex;
    SessionManager sessionManager;
    GameSaveData saveData;
    RoomMappingData roomMappingData;
    Dictionary<string, PulleyAnchorPointMapping> anchor_points;

    bool cableCarPresent = false;
    public GameObject cableCarObj;
    public SceneTransition sceneTransition;

    void Start()
    {
        // get save data from sessionmanager
        sessionManager = SessionManager.Instance;
        saveData = sessionManager.saveData;
        roomMappingData = sessionManager.roomMappingData;
        anchor_points = roomMappingData.anchor_points;
        sceneTransition.entranceNumber = anchor_points[id.ToString()].entrance_index;
    }

    void ActivateCableCar(){
        cableCarPresent = true;
        cableCarObj.SetActive(true);
    }

    void DeactivateCableCar(){
        cableCarPresent = false;
        cableCarObj.SetActive(false);
    }

    public override void Interact(){
        if(cableCarPresent){
            this.hasMenu = true;
            // sync options
            sitePanel.SyncOptions(saveData.integer_markers["pulley_anchor_points"], id - 100000);

            // activate site panel
            sitePanel.gameObject.SetActive(true);

            // activate camera
            ActivateCamera();
        } else{
            this.hasMenu = false;
            ActivateCableCar();
        }
    }

    public override void LeaveInteraction(){
        // deactivate site panel
        sitePanel.gameObject.SetActive(false);
        GameObject.FindWithTag("Player").GetComponent<PlayerHub>().inputManager.LeaveDialogue();

        // reset camera
        DeactivateCamera();
    }

    public override void MenuUp(){
        sitePanel.AscendSelection();
    }

    public override void MenuDown(){
        sitePanel.DescendSelection();
    }

    public override void MenuLeft(){
        sitePanel.AscendSelection();
    }

    public override void MenuRight(){
        sitePanel.DescendSelection();
    }

    public override void MenuSelect(){
        int selection = sitePanel.SelectOption() + 100000;
        DeactivateCableCar();
        LeaveInteraction();

        // find the build index of the room to switch into
        int buildIndex = roomMappingData.anchor_points[selection.ToString()].build_index;
        int nextEntranceNumber = roomMappingData.anchor_points[selection.ToString()].entrance_index;

        // set game data to move cable car to new location
        SessionManager.Instance.SetCableCar(selection);

        // transition to scene with data set above
        SessionManager.Instance.TransitionScene(buildIndex, nextEntranceNumber, 0);
    }

    public override void LoadSite(SavedSite savedSite){
        if(SessionManager.Instance.CheckCableCar(id)){
            ActivateCableCar();
        }
    }

    public override SavedSite SaveSite(){
        SavedSite savedSite = new SavedSite();
        savedSite.name = "cablecar";
        if(cableCarPresent){
            SessionManager.Instance.SetCableCar(id);
        }
        return savedSite;
    }

    protected override void EnterRange(){
        
    }

    protected override void ExitRange(){
        
    }
}
