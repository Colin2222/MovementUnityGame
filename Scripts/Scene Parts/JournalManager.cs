using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Newtonsoft.Json;

public class JournalManager : MonoBehaviour
{
	public JournalCanvasScript journalUI;
	int currentPage = 0;
	JournalEntrySet es;
	AsyncOperationHandle<Sprite> writingSpriteOp;
	Sprite writingSprite;
	
	[System.NonSerialized]
	public bool active = false;
	
	TextAsset currentDataTxt;
	
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
		
		// load in json of journal data
		var operation = Addressables.LoadAssetAsync<TextAsset>("Assets/Data/journal_data.json");
		currentDataTxt = operation.WaitForCompletion();
		
		// parse json into cutscene object
		es = JsonConvert.DeserializeObject<JournalEntrySet>(currentDataTxt.text);
		
		// done parsing json, release asset out of memory
		Addressables.Release(operation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void Activate(){
		journalUI.journalObject.SetActive(true);
		writingSpriteOp = Addressables.LoadAssetAsync<Sprite>("Assets/Data/JournalPages/" + es.entries[0].writingImageLocation + ".png");
		writingSprite = writingSpriteOp.WaitForCompletion();
		
		// set sprite
		journalUI.SetPageImages(writingSprite);
	}
	
	public void Deactivate(){
		Addressables.Release(writingSpriteOp);
		
		journalUI.journalObject.SetActive(false);
	}
	
	public void SeekUI(){
		journalUI = GameObject.FindWithTag("JournalUI").GetComponent<JournalCanvasScript>();
	}
}

public class JournalEntry{
	public int number;
	public string writingImageLocation;
	public string drawingImageLocation;
	public bool found;
	public bool completed;
}

public class JournalEntrySet{
	public JournalEntry[] entries;
}
