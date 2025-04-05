using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManagerInteractable : Interactable
{
    public DialogueManager dialogueManager;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        
    }

    public override void LeaveInteraction()
    {
        
    }

    public override void MenuUp()
    {
        
    }

    public override void MenuDown()
    {
        
    }

    public override void MenuLeft()
    {
        
    }

    public override void MenuRight()
    {
        
    }

    public override void MenuSelect()
    {
        dialogueManager.NextNode();
    }
}
