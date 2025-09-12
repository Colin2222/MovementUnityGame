using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteAmberPool : Site
{
    string amberColor = "none";
    float runTime = 5f;
    AmberManager amberManager;
    // Start is called before the first frame update
    void Start()
    {
        amberManager = SessionManager.Instance.amberManager;
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
    
    public override void Interact(){
        if (!amberManager.inAmberRun)
        {
            amberManager.StartAmberRun(amberColor, runTime);
        }
        else
        {
            amberManager.CheckAmberRunCompletion(amberColor);
        }
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

    public override void MenuInteract()
    {
        
    }

    public override void LoadSite(SavedSite savedSite)
    {
        amberColor = savedSite.additional_data["amber_color"];
        runTime = float.Parse(savedSite.additional_data["run_time"]);
    }

    public override SavedSite SaveSite(){
        SavedSite savedSite = new SavedSite();
        savedSite.name = "amberpool";
        savedSite.additional_data = new Dictionary<string, string>();
        savedSite.additional_data.Add("amber_color", amberColor);
        savedSite.additional_data.Add("run_time", runTime.ToString());
        return savedSite;
    }

    public override void ConstructSite(){
        
    }
}
