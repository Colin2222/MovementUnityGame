using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{
	SessionManager sessionManager;
	SceneManager sceneManager;
	
	// maybe make a singular class that includes cutscenename and all requisite data elements,
	// then i can have multiple loaded per gate and there can be an immediate branching (dont have to kluge overlapping gates with different requirements)
	public List<DataTrigger> dataTriggers; 
	public string cutsceneName;
	
    // Start is called before the first frame update
    void Start()
    {
        sessionManager = GameObject.FindWithTag("SessionManager").GetComponent<SessionManager>();
		sceneManager = GameObject.FindWithTag("SceneManager").GetComponent<SceneManager>();
		
		// get the cutscene manager to load the cutscene of this gate
    }

	// MAYBE FOR EFFICIENCY'S SAKE, DO THIS CALCULATION AT SCENE START AND REDO LOGIC ONLY IF THERE IS A SAVE DATA CHANGE
    void OnTriggerEnter2D(Collider2D other)
    {
		bool executeAction = true;
		foreach(DataTrigger dataTrigger in dataTriggers){
			foreach(DataElement element in dataTrigger.dataToCheck){
				if(sessionManager.GetData(element.dataName) != element.dataValue){
					executeAction = false;
					break;
				}
			}
			
			if(executeAction){
				foreach(DataElement setElement in dataTrigger.dataToSet){
					sessionManager.SetData(setElement.dataName, setElement.dataValue);
				}
				sceneManager.cutsceneManager.LoadCutscene(cutsceneName);
				sceneManager.cutsceneManager.PlayCutscene(-1);
				Destroy(gameObject);
				break;
			}
		}
    }
}

[System.Serializable]
public class DataTrigger{
	public string cutsceneName;
	public List<DataElement> dataToCheck;
	public List<DataElement> dataToSet;
}

[System.Serializable]
public class DataElement{
	public string dataName;
	public bool dataValue;
}
