using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteChest : Site
{
    public GameObject rangeEffect;
    bool active;

    InventoryCanvasScript canvas;
    (int x, int y) currentSlotPos;
    (int x, int y) selectionPos;
	bool inSelection = false;
    Inventory playerInventory;
    public SiteChestPanel sitePanel;
    public Inventory siteInventory;

    // can be player or site
    string currentInventory = "player";
    string currentSelection = "none";

    void Awake(){
        
    }

    void Start(){
        sitePanel.SyncInventory(siteInventory);
    }

    protected override void EnterRange(){
        rangeEffect.SetActive(true);
    }

    protected override void ExitRange(){
        rangeEffect.SetActive(false);
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
            if(currentSlotPos.y < siteInventory.height - 1){
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
        if(inSelection){
            if(currentInventory == "player"){
                if(selectionPos == currentSlotPos && currentSelection == "player"){
                    inSelection = false;
                    canvas.EndSelection();
			    } else{
                    if(currentSelection == "site"){
                        Debug.Log("Trading item site to player");
                        playerInventory.TradeItem(selectionPos.x, selectionPos.y, siteInventory, currentSlotPos.x, currentSlotPos.y);
                    } else{
                        playerInventory.SwapItem(selectionPos.x, selectionPos.y, currentSlotPos.x, currentSlotPos.y);
                    }
                    UpdateIcons();
                    PlayerHub.Instance.inventoryHandler.UpdateIcons();
                    canvas.EndSelection();
                    sitePanel.EndSelection();
                    inSelection = false;
                }
			} else if (currentInventory == "site"){
                if(selectionPos == currentSlotPos && currentSelection == "site"){
                    inSelection = false;
                    sitePanel.EndSelection();
                } else{
                    if(currentSelection == "player"){
                        Debug.Log("Trading item player to site");
                        siteInventory.TradeItem(selectionPos.x, selectionPos.y, playerInventory, currentSlotPos.x, currentSlotPos.y);
                    } else{
                        siteInventory.SwapItem(selectionPos.x, selectionPos.y, currentSlotPos.x, currentSlotPos.y);
                    }
                    UpdateIcons();
                    PlayerHub.Instance.inventoryHandler.UpdateIcons();
                    sitePanel.EndSelection();
                    canvas.EndSelection();
                    inSelection = false;
                }
            }
		} else{
            if(currentInventory == "site"){
                currentSelection = "site";
                selectionPos = currentSlotPos;
                sitePanel.StartSelection(currentSlotPos.y, currentSlotPos.x);
                inSelection = true;
            } else if(currentInventory == "player"){
                currentSelection = "player";
                selectionPos = currentSlotPos;
                canvas.StartSelection(currentSlotPos.y, currentSlotPos.x);
                inSelection = true;
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
}
