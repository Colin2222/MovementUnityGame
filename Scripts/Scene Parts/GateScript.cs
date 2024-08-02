using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{
	SessionManager sessionManager;
	SceneManager sceneManager;
	
	// maybe make a singular class that includes cutscenename and all requisite data elements,
	// then i can have multiple loaded per gate and there can be an immediate branching (dont have to kluge overlapping gates with different requirements)
	public List<(string, bool)> dataTriggers; 
	public string cutsceneName;
	
    // Start is called before the first frame update
    void Start()
    {
        sessionManager = GameObject.FindWithTag("SessionManager").GetComponent<SessionManager>();
		sceneManager = GameObject.FindWithTag("SceneManager").GetComponent<SceneManager>();
		
		// get the cutscene manager to load the cutscene of this gate
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        sceneManager.cutsceneManager.LoadCutscene(cutsceneName);
		sceneManager.cutsceneManager.PlayCutscene(-1);
    }
}
