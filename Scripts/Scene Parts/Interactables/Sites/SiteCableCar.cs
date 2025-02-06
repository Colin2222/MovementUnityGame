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

    [System.NonSerialized]
    public bool cableCarPresent = false;
    public GameObject cableCarObj;
    public SceneTransition sceneTransition;
    public float transitionTime;
    float transitionTimer = 0;
    bool transitioning = false;
    int buildIndex;
    int nextEntranceNumber;
    public Transform walkTransform;

    public Transform aimPoint;
    public float aimPointSpeed;
    Vector3 aimPointVelocity;
    bool moving = false;
    public float moveStartTime;
    public Animator cableCarAnimator;
    int cableCarDirection;
    public Transform cableCarCenterTransform;
    public Transform cableCarTopTransform;
    bool entering = false;
    bool operational;

    public Vector3 cableCarOffset;

    public Animator pulleyAnchorAnimator;

    void Start()
    {
        // get save data from sessionmanager
        sessionManager = SessionManager.Instance;
        saveData = sessionManager.saveData;
        roomMappingData = sessionManager.roomMappingData;
        anchor_points = roomMappingData.anchor_points;
        sceneTransition.entranceNumber = anchor_points[id.ToString()].entrance_index;
    }

    void Update(){
        if(transitioning){
            transitionTimer += Time.deltaTime;
            if(transitionTimer >= transitionTime){
                transitioning = false;
                // transition to scene with data set previously
                SessionManager.Instance.TransitionScene(buildIndex, nextEntranceNumber, 0);
            } else if(transitionTimer >= moveStartTime){
                if(!moving){
                    this.pulleyAnchorAnimator.Play("pulleyanchor_spinning");
                    StartCableCarMovement();
                }

            }
        }
        if(moving || entering){
            cableCarObj.transform.position += aimPointVelocity * Time.deltaTime;
        }
        if(entering && Vector3.Distance(cableCarObj.transform.position, aimPoint.position - cableCarTopTransform.localPosition) < 0.1){
            this.pulleyAnchorAnimator.Play("pulleyanchor_idle");
            entering = false;
            cableCarPresent = true;
            cableCarObj.transform.localPosition = cableCarOffset;
            LoosenCableSprites();
        }
    }

    void ActivateCableCar(){
        cableCarObj.SetActive(true);
        cableCarPresent = true;
    }

    void DeactivateCableCar(){
        cableCarPresent = false;
        cableCarObj.SetActive(false);
    }

    public override void Interact(){
        if(!operational){
            HandleCableCarProgress();
            if(!operational){
                Debug.Log("Cable car not operational");
                this.hasMenu = false;
                return;
            }
        }
        if(cableCarPresent){
            this.hasMenu = true;
            // sync options
            sitePanel.SyncOptions(saveData.integer_markers["cable_car_progress"], id - 100000);

            // activate site panel
            sitePanel.gameObject.SetActive(true);

            // activate camera
            ActivateCamera();
        } else if(!entering && !moving){
            this.pulleyAnchorAnimator.Play("pulleyanchor_spinning");
            SessionManager.Instance.SetCableCar(id);
            this.hasMenu = false;
            ActivateCableCar();
            cableCarPresent = false;
            FindEntryPoint(sessionManager.currentEntranceDirection);
            StartCableCarEntry();
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
        cableCarPresent = false;
        LeaveInteraction();

        // find the build index of the room to switch into
        buildIndex = roomMappingData.anchor_points[selection.ToString()].build_index;
        nextEntranceNumber = roomMappingData.anchor_points[selection.ToString()].entrance_index;

        // set game data to move cable car to new location
        SessionManager.Instance.SetCableCar(selection);

        // start timer before transitioning
        transitioning = true;
        PlayerHub.Instance.overrideManager.WalkToPoint(walkTransform.position.x);

        // determine aim point for cabin movement
        cableCarDirection = selection > id ? -1 : 1;
        sessionManager.currentEntranceDirection = cableCarDirection;
        FindAimPoint(cableCarDirection);
    }

    public override void LoadSite(SavedSite savedSite){
        if(SessionManager.Instance.CheckCableCar(id)){
            ActivateCableCar();
        }
        HandleCableSprites();
    }

    public override SavedSite SaveSite(){
        SavedSite savedSite = new SavedSite();
        savedSite.name = "cablecar";
        if(cableCarPresent){
            SessionManager.Instance.SetCableCar(id);
        }
        return savedSite;
    }

    public override void ConstructSite(){
        HandleCableCarProgress();
    }

    void HandleCableCarProgress(){
        SessionManager sessionManager = SessionManager.Instance;
        int currentCableCarProgress = sessionManager.GetIntegerMarker("cable_car_progress");
        if(currentCableCarProgress == id - 1){
            sessionManager.SetIntegerMarker("cable_car_progress", id);
            operational = true;
        } else if(currentCableCarProgress >= id){
            operational = true;
        } else{
            operational = false;
        }
        HandleCableSprites();
    }

    void HandleCableSprites(){
        GameObject[] points = GameObject.FindGameObjectsWithTag("CableCarCable");
        foreach(GameObject point in points){
            SiteCableCarCable cablePoint = point.GetComponent<SiteCableCarCable>();
            if(cablePoint.siteId <= SessionManager.Instance.GetIntegerMarker("cable_car_progress") && cablePoint.cableObj != null){
                cablePoint.cableObj.SetActive(true);
            }
        }
    }

    void TightenCableSprites(){
        GameObject[] points = GameObject.FindGameObjectsWithTag("CableCarCable");
        foreach(GameObject point in points){
            SiteCableCarCable cablePoint = point.GetComponent<SiteCableCarCable>();
            if(cablePoint.cableObj.activeSelf && cablePoint.cableAnimator != null){
                cablePoint.cableAnimator.Play("cable_tighten");
            }
        }
    }

    void LoosenCableSprites(){
        GameObject[] points = GameObject.FindGameObjectsWithTag("CableCarCable");
        foreach(GameObject point in points){
            SiteCableCarCable cablePoint = point.GetComponent<SiteCableCarCable>();
            if(cablePoint.cableObj.activeSelf && cablePoint.cableAnimator != null){
                cablePoint.cableAnimator.Play("cable_loose");
            }
        }
    }

    protected override void EnterRange(){
        
    }

    protected override void ExitRange(){
        
    }

    void FindAimPoint(int direction){
        GameObject[] points = GameObject.FindGameObjectsWithTag("CableCarAimPoint");
        foreach(GameObject point in points){
            if(point.GetComponent<SiteCableCarAimPoint>().siteId == id - direction){
                aimPoint = point.transform;
                break;
            }
        }
    }

    void StartCableCarMovement(){
        moving = true;
        aimPointVelocity = (aimPoint.position - cableCarTopTransform.position).normalized * aimPointSpeed;
        cableCarAnimator.Play(cableCarDirection == 1 ? "cablecar_empty_moveright" : "cablecar_empty_moveleft");
        PlayerHub.Instance.stateManager.EnterTransformFollow(cableCarCenterTransform);
        TightenCableSprites();
    }

    void FindEntryPoint(int direction){
        GameObject[] points = GameObject.FindGameObjectsWithTag("CableCarAimPoint");
        foreach(GameObject point in points){
            if(point.GetComponent<SiteCableCarAimPoint>().siteId == id){
                aimPoint = point.transform;
            }
            if(point.GetComponent<SiteCableCarAimPoint>().siteId == id + direction){
                cableCarObj.transform.position = point.transform.position - cableCarTopTransform.localPosition;
            }
        }
    }

    void StartCableCarEntry(){
        entering = true;
        aimPointVelocity = (aimPoint.position - cableCarTopTransform.position).normalized * aimPointSpeed;
        ClearOtherCableCarsInScene();
        TightenCableSprites();
    }

    void ClearOtherCableCarsInScene(){
        GameObject[] siteObjs = GameObject.FindGameObjectsWithTag("SiteSlot");
        foreach(GameObject siteObj in siteObjs){
            GameObject child = siteObj.transform.GetChild(0).gameObject;
            if(child != null && child.GetComponent<SiteCableCar>() != null && child.GetComponent<SiteCableCar>().id != id){
                child.GetComponent<SiteCableCar>().cableCarObj.SetActive(false);
                child.GetComponent<SiteCableCar>().cableCarPresent = false;
            }
        }
    }

}
