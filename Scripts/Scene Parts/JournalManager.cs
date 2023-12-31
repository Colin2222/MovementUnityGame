using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalManager : MonoBehaviour
{
	public GameObject journalUI;
	int currentPage;
	
	[System.NonSerialized]
	public bool active = false;
	
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void Activate(){
		journalUI.SetActive(true);
	}
	
	public void Deactivate(){
		journalUI.SetActive(false);
	}
	
	public void SeekUI(){
		journalUI = GameObject.FindWithTag("JournalUI").GetComponent<JournalCanvasScript>().journalObject;
	}
}

public class JournalEntry{
	string textImageLocation;
	string drawingImageLocation;
}
