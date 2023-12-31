using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalManager : MonoBehaviour
{
	public JournalCanvasScript journalUI;
	int currentPage;
	
	[System.NonSerialized]
	public bool active = false;
	
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
		
		// load in persistent journal data
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void Activate(){
		journalUI.journalObject.SetActive(true);
	}
	
	public void Deactivate(){
		journalUI.journalObject.SetActive(false);
	}
	
	public void SeekUI(){
		journalUI = GameObject.FindWithTag("JournalUI").GetComponent<JournalCanvasScript>();
	}
}

public class JournalEntry{
	int number;
	string textImageLocation;
	string drawingImageLocation;
	bool found;
	bool completed;
}
