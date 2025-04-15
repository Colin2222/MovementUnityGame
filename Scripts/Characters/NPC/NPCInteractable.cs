using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : Interactable
{
    DialogueManager dialogueManager;
    public NPCHub npcHub;
    public bool hasDialogue;
    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = GameObject.FindWithTag("DialogueManager").GetComponent<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        dialogueManager.StartDialogue(npcHub.npcName);
    }

    public override void LeaveInteraction()
    {
        dialogueManager.EndDialogue();
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

    public override void MenuInteract()
    {
        
    }
}
