using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteConstruction : Site
{
    public GameObject rangeEffect;
    bool active;

    InventoryCanvasScript canvas;
    (int x, int y) currentSlotPos;
    (int x, int y) selectionPos;
	bool inSelection = false;
    Inventory playerInventory;
    public SiteConstructionPanel sitePanel;
    public Inventory siteInventory;

    public string siteName;
    ConstructionRequirement[] requirements;
    GameObject constructedSitePrefab;


    // can be player or site
    string currentInventory = "player";
    string currentSelection = "none";

    void Awake(){
        
    }

    void Start(){
        
    }

    protected override void EnterRange(){
        if(rangeEffect != null){
            rangeEffect.SetActive(true);
        }
    }

    protected override void ExitRange(){
        if(rangeEffect != null){
            rangeEffect.SetActive(false);
        }
    }

    public override void Interact(){
        GameObject canvasObj = GameObject.FindWithTag("InventoryUI");
        if(canvasObj != null){
            canvas = canvasObj.GetComponent<InventoryCanvasScript>();
            canvas.openBagObject.SetActive(true);
            canvas.itemsObject.SetActive(true);
            currentSelection = "player";
            currentInventory = "player";
			currentSlotPos = (0, 0);
			canvas.ChangeSelection(currentSlotPos.y, currentSlotPos.x);
            PlayerHub.Instance.inventoryHandler.UpdateIcons();
            UpdateIcons();
            playerInventory = PlayerHub.Instance.inventoryHandler.inventory;

            // activate site panel
            sitePanel.gameObject.SetActive(true);

            // activate camera
            ActivateCamera();
        }
        active = true;
    }

    public override void LeaveInteraction(){
        currentSlotPos = (0, 0);
        canvas.ChangeSelection(currentSlotPos.y, currentSlotPos.x);
        sitePanel.ChangeSelection(-1, -1);
        currentSelection = "player";
        inSelection = false;
        sitePanel.EndSelection();
        canvas.EndSelection();
        canvas.itemsObject.SetActive(false);
        canvas.openBagObject.SetActive(false);
        sitePanel.gameObject.SetActive(false);

        // deactivate camera
        DeactivateCamera();
    }

    public override void MenuUp(){
        if(currentInventory == "player"){
            if(currentSlotPos.y > 0){
                (currentSlotPos.y)--;
                canvas.ChangeSelection(currentSlotPos.y, currentSlotPos.x);
            }
        } else if(currentInventory == "site"){
            if(currentSlotPos.y > 0){
                (currentSlotPos.y)--;
                sitePanel.ChangeSelection(currentSlotPos.y, currentSlotPos.x);
            }
        }
    }

    public override void MenuDown(){
        if(currentInventory == "player"){
            if(currentSlotPos.y < playerInventory.height - 1){
                (currentSlotPos.y)++;
                canvas.ChangeSelection(currentSlotPos.y, currentSlotPos.x);
            }
        } else if(currentInventory == "site"){
            // LEAVE AN EXTRA SLOT FOR THE BUTTON AT THE BOTTOM
            if(currentSlotPos.y < siteInventory.height){
                (currentSlotPos.y)++;
                sitePanel.ChangeSelection(currentSlotPos.y, currentSlotPos.x);
            }
        }
    }

    public override void MenuLeft(){
        if(currentInventory == "player"){
            if(currentSlotPos.x > 0){
                (currentSlotPos.x)--;
                canvas.ChangeSelection(currentSlotPos.y, currentSlotPos.x);
            }
        } else if(currentInventory == "site"){
            if(currentSlotPos.x > 0){
                (currentSlotPos.x)--;
                sitePanel.ChangeSelection(currentSlotPos.y, currentSlotPos.x);
            } else{
                currentInventory = "player";
                sitePanel.ChangeSelection(-1, -1);
                if(currentSlotPos.y >= playerInventory.height){
                    currentSlotPos.y = playerInventory.height - 1;
                }
                canvas.ChangeSelection(currentSlotPos.y, playerInventory.width - 1);
                currentSlotPos.x = playerInventory.width - 1;
            }
        }
    }

    public override void MenuRight(){
        if(currentInventory == "player"){
            if(currentSlotPos.x < playerInventory.width - 1){
                (currentSlotPos.x)++;
                canvas.ChangeSelection(currentSlotPos.y, currentSlotPos.x);
            } else{
                currentInventory = "site";
                canvas.ChangeSelection(-1, -1);
                if(currentSlotPos.y >= siteInventory.height){
                    currentSlotPos.y = siteInventory.height - 1;
                }
                sitePanel.ChangeSelection(currentSlotPos.y, 0);
                currentSlotPos.x = 0;
            }
        }  else if(currentInventory == "site"){
            if(currentSlotPos.x < siteInventory.width - 1){
                (currentSlotPos.x)++;
                sitePanel.ChangeSelection(currentSlotPos.y, currentSlotPos.x);
            }
        }
    }

    public override void MenuSelect(){
        Inventory selectedInv = currentSelection == "player" ? playerInventory : siteInventory;
        Inventory currentInv = currentInventory == "player" ? playerInventory : siteInventory;

        // check if construct button is pressed
        if(currentInventory == "site" && currentSlotPos.y == siteInventory.height){
            bool canConstruct = true;
            for(int i = 0; i < siteInventory.height; i++){
                if(siteInventory.contents[i, 0] == null || siteInventory.contents[i, 0].quantity < siteInventory.maxQuantities[i, 0]){
                    canConstruct = false;
                    break;
                }
            }

            if(canConstruct){
                GameObject constructedObj = Instantiate(constructedSitePrefab, transform.position, Quaternion.identity);
                constructedObj.GetComponent<Site>().id = id;
                constructedObj.transform.SetParent(transform.parent);
                constructedObj.GetComponent<Site>().ConstructSite();
                SessionManager.Instance.SetSite(constructedObj.GetComponent<Site>().SaveSite(), id);
                siteInventory = null;
                PlayerHub.Instance.inputManager.LeaveInteraction();
                Destroy(gameObject);
            }
            return;
        }

        if(inSelection){
            if(selectedInv != currentInv || selectionPos != currentSlotPos){
                currentInv.InsertItem(selectedInv, selectionPos.x, selectionPos.y, currentSlotPos.x, currentSlotPos.y);
            }
            EndSelection();
		} else if (currentInv.contents[currentSlotPos.y, currentSlotPos.x] != null){
            currentSelection = currentInventory;
            selectionPos = currentSlotPos;
            inSelection = true;
            if(currentInventory == "site"){
                sitePanel.StartSelection(currentSlotPos.y, currentSlotPos.x);
            } else if(currentInventory == "player"){
                canvas.StartSelection(currentSlotPos.y, currentSlotPos.x);
            }
		}
    }

    public void UpdateIcons(){
		for(int i = 0; i < siteInventory.height; i++){
			for(int j = 0; j < siteInventory.width; j++){
				if(siteInventory.contents[i, j] != null){
					sitePanel.SetIcon(i, j, siteInventory.contents[i, j].item.icon, siteInventory.contents[i, j].quantity);
				} else{
					sitePanel.SetIcon(i, j, null, 0);
				}
			}
		}
	}

    void EndSelection(){
        UpdateIcons();
        PlayerHub.Instance.inventoryHandler.UpdateIcons();
        sitePanel.EndSelection();
        canvas.EndSelection();
        inSelection = false;
    }

    public override void LoadSite(SavedSite savedSite){
        // set construction target site
        siteName = savedSite.additional_data["site_construction"];

        // populate inventory
        SavedInventory inv = savedSite.inventories[0];
        SaveDataHelperMethods.LoadInventory(siteInventory, inv);

        // populate requirements
        SitePrefabRegistry registry = GameObject.FindWithTag("SitePrefabRegistry").GetComponent<SitePrefabRegistry>();
        SitePrefabEntry entry = registry.GetEntry(siteName);
        requirements = entry.requirements;
        constructedSitePrefab = entry.prefab;
        int i = 0;
        foreach(ConstructionRequirement req in requirements){
            siteInventory.maxQuantities[i, 0] = req.quantity;
            siteInventory.slotTypes[i, 0] = req.item;
            sitePanel.AddRequirementSlot(req.item, req.quantity, siteInventory);

            i++;
        }
        sitePanel.AddConstructButton();
    }

    public override SavedSite SaveSite(){
        SavedSite savedSite = new SavedSite();
        savedSite.name = "construction";
        savedSite.additional_data = new Dictionary<string, string>();
        savedSite.additional_data.Add("site_construction", siteName);
        savedSite.inventories = new List<SavedInventory>();
        savedSite.inventories.Add(siteInventory.SaveInventory());
        return savedSite;
    }

    public override void ConstructSite(){
        
    }
}

[System.Serializable]
public class ConstructionRequirement{
    public string item;
    public int quantity;
    public ConstructionRequirement(string item, int quantity){
        this.item = item;
        this.quantity = quantity;
    }
}
