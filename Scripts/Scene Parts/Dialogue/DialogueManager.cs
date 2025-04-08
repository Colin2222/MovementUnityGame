using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DialogueDataClasses;

public class DialogueManager : MonoBehaviour
{
    public CutsceneManager cutsceneManager;
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public Image dialogueImage;
    public Image nameImage;

    bool inDialogue = false;
    DialogueTree currentDialogue;
    DialogueBranch currentBranch;
    DialogueNode currentNode;
    int currentNodeIndex = 0;

    SessionManager sessionManager;

    void Start(){
        sessionManager = GameObject.FindWithTag("SessionManager").GetComponent<SessionManager>();
    }
    
    public void StartDialogue(string dialogueCode)
    {
        currentDialogue = sessionManager.GetDialogueTree(dialogueCode);
        if(currentDialogue == null){
            Debug.Log("Dialogue not found");
            EndDialogue();
            return;
        }
        currentBranch = FindValidBranch(currentDialogue.branches);
        if(currentBranch == null){
            Debug.Log("No valid branch found");
            EndDialogue();
            return;
        }
        currentNodeIndex = 0;
        EvaluateNode(currentBranch.nodes[currentNodeIndex]);

        dialogueText.gameObject.SetActive(true);
        nameText.gameObject.SetActive(true);
        dialogueImage.gameObject.SetActive(true);
        nameImage.gameObject.SetActive(true);

        inDialogue = true;
        if(cutsceneManager.inCutscene){
            cutsceneManager.EnterDialogue();
        }
    }

    public void EndDialogue()
    {
        dialogueText.gameObject.SetActive(false);
        nameText.gameObject.SetActive(false);
        dialogueImage.gameObject.SetActive(false);
        nameImage.gameObject.SetActive(false);

        inDialogue = false;
        GameObject.FindWithTag("Player").GetComponent<PlayerHub>().inputManager.LeaveDialogue();
        if(cutsceneManager.inCutscene){
            cutsceneManager.ExitDialogue();
        }
    }

    DialogueBranch FindValidBranch(List<DialogueBranch> branches){
        foreach(DialogueBranch branch in branches){
            bool valid = true;
            foreach(DialogueCheck check in branch.conditions){
                if(check.type == "progress_marker"){
                    if(sessionManager.GetData(check.key) != bool.Parse(check.value)){
                        valid = false;
                        break;
                    }
                }
            }
            if(valid){
                return branch;
            }
        }
        return null;
    }

    void EvaluateNode(DialogueNode node){
        if(node.GetType() == typeof(DialogueNodeText)){
            DialogueNodeText textNode = (DialogueNodeText)node;
            dialogueText.text = textNode.text;
            nameText.text = textNode.speaker;
        }
        else if(node.GetType() == typeof(DialogueNodeChoice)){
            DialogueNodeChoice choiceNode = (DialogueNodeChoice)node;
            dialogueText.text = choiceNode.text;
            nameText.text = choiceNode.speaker;
        }
        else if(node.GetType() == typeof(DialogueNodeDataChange)){
            DialogueNodeDataChange dataChangeNode = (DialogueNodeDataChange)node;
            sessionManager.SetData(dataChangeNode.data_key,dataChangeNode.data_value);
        }
        else if(node.GetType() == typeof(DialogueNodeEnd)){
            EndDialogue();
        }
    }

    public void NextNode(){
        currentNodeIndex++;
        if(currentNodeIndex >= currentBranch.nodes.Count){
            EndDialogue();
            return;
        }
        EvaluateNode(currentBranch.nodes[currentNodeIndex]);
    }
}
