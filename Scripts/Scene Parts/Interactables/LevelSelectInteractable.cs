using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectInteractable : Interactable
{
	[System.NonSerialized]
	public int levelId;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public override void Interact(){
		GameObject.FindWithTag("SceneTransitionManager").GetComponent<SceneTransitionManager>().ExitTransition(levelId);
	}

    public override void LeaveInteraction(){
        
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
        
    }
}
