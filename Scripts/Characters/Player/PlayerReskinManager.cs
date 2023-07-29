using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerReskinManager : MonoBehaviour
{
	public string spritesheetCode;
	public string originalSpritesheetCode;
	public SpriteRenderer spriteRenderer;
	Sprite[] spriteSheetCollection;
	bool reskinActive = false;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
		if(reskinActive){
			string spriteName = spriteRenderer.sprite.name.Replace(originalSpritesheetCode, spritesheetCode);
			spriteRenderer.sprite = Array.Find(spriteSheetCollection, item => item.name == spriteName);
		}
    }
	
	public void SetNewSpritesheet(string spritesheetCode){
		if(spritesheetCode != originalSpritesheetCode && spritesheetCode != ""){
			this.spritesheetCode = spritesheetCode;
			spriteSheetCollection = Resources.LoadAll<Sprite>("PlayerSpritesheets/" + spritesheetCode);
			reskinActive = true;
		} else{
			reskinActive = false;
		}
	}
}
