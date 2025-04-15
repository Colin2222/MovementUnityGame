using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitePulleyAnchor : Site
{
    protected override void EnterRange(){
        
    }

    protected override void ExitRange(){
        
    }

    public override void Interact(){
        Debug.Log("Interacting with pulley anchor");
    }

    public override void LeaveInteraction(){
        
    }

    public override void MenuUp(){
        
    }

    public override void MenuDown(){
        
    }

    public override void MenuLeft(){
        
    }

    public override void MenuRight(){

    }

    public override void MenuSelect(){
        
    }

    public override void LoadSite(SavedSite savedSite){
        
    }

    public override SavedSite SaveSite(){
        SavedSite savedSite = new SavedSite();
        savedSite.name = "pulleyanchor";
        savedSite.additional_data = new Dictionary<string, string>();
        savedSite.inventories = new List<SavedInventory>();
        return savedSite;
    }

    public override void ConstructSite(){
        
    }

    public override void MenuInteract()
    {
        
    }
}
