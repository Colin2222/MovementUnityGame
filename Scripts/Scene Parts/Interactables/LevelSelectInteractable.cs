using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectInteractable : MonoBehaviour, IInteractable
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
	
	public void Interact(){
		GameObject.FindWithTag("SceneTransitionManager").GetComponent<SceneTransitionManager>().ExitTransition(levelId);
	}
}
