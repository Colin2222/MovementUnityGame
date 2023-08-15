using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionManager : MonoBehaviour
{
    public float levelSelectSpacing;
	public GameObject levelSelectionPrefab;
	
	public void SetupLevelSelection(Transform starterLocation){
		int index = 0;
		foreach(int id in LevelRegistry.GetLevelIdList()){
			GameObject instance = Instantiate(levelSelectionPrefab);
			instance.transform.position = new Vector3(starterLocation.position.x - (levelSelectSpacing * index), starterLocation.position.y, 0.0f);
			LevelSelectInteractable interactable = instance.GetComponent<LevelSelectInteractable>();
			interactable.levelId = id;
			
			index++;
		}
	}
}
