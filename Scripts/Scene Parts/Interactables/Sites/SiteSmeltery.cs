using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteSmeltery : Site
{
    bool active;

    InventoryCanvasScript canvas;
    (int x, int y) currentSlotPos;
    (int x, int y) selectionPos;
    bool inSelection = false;
    Inventory playerInventory;
    public SiteSmelteryPanel sitePanel;
    public Inventory siteInventory;

    // can be player or site
    string currentInventory = "player";
    string currentSelection = "none";
    // Start is called before the first frame update
    void Start()
    {
        sitePanel.SyncInventory(siteInventory);
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void EnterRange()
    {

    }

    protected override void ExitRange()
    {

    }

    public override void Interact()
    {
        GameObject canvasObj = GameObject.FindWithTag("InventoryUI");
        if (canvasObj != null)
        {
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

    public override void LeaveInteraction()
    {
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
            // determine recipe result from item in the input slot
            Item inputItem = siteInventory.contents[0, 0].item;
            Item resultItem = SessionManager.Instance.recipeManager.GetFurnaceResult(inputItem);
            if(resultItem != null){
                // remove one of the input item
                int quantity = siteInventory.contents[0, 0].quantity;
                siteInventory.RemoveItem(0, 0, quantity);
                // add the result item to the output slot
                playerInventory.AddItem(resultItem, quantity);
                PlayerHub.Instance.inventoryHandler.UpdateIcons();
                UpdateIcons();
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

    public override void MenuInteract()
    {

    }
    
    void EndSelection(){
        UpdateIcons();
        PlayerHub.Instance.inventoryHandler.UpdateIcons();
        sitePanel.EndSelection();
        canvas.EndSelection();
        inSelection = false;
    }
    
    public void UpdateIcons()
    {
        for (int i = 0; i < siteInventory.height; i++)
        {
            for (int j = 0; j < siteInventory.width; j++)
            {
                if (siteInventory.contents[i, j] != null)
                {
                    sitePanel.SetIcon(i, j, siteInventory.contents[i, j].item.icon, siteInventory.contents[i, j].quantity);
                }
                else
                {
                    sitePanel.SetIcon(i, j, null, 0);
                }
            }
        }
    }

    public override void LoadSite(SavedSite savedSite)
    {
        // populate inventory
        SavedInventory inv = savedSite.inventories[0];
        SaveDataHelperMethods.LoadInventory(siteInventory, inv);
    }

    public override SavedSite SaveSite(){
        SavedSite savedSite = new SavedSite();
        savedSite.name = "smeltery";
        savedSite.additional_data = new Dictionary<string, string>();
        savedSite.inventories = new List<SavedInventory>();
        savedSite.inventories.Add(siteInventory.SaveInventory());
        return savedSite;
    }

    public override void ConstructSite(){
        siteInventory.ResetInventory(siteInventory.width, siteInventory.height);
    }
}
