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
	
	public Color invertedEdge;
	public Color noninvertedEdge;
	public Color invertedSkin;
	public Color noninvertedSkin;
	
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
			spriteSheetCollection = Resources.LoadAll<Sprite>(spritesheetCode);
			reskinActive = true;
		} else{
			reskinActive = false;
		}
	}
	
	public void SetPlayerInvertSkin(bool inverted){
		string spritesheetName = "currentplayer";
		Texture2D originalTexture = Resources.Load<Texture2D>(spritesheetName);
		
		if(inverted){
			for(int i = 0; i < originalTexture.width; i++){
				for(int j = 0; j < originalTexture.height; j++){
					Color currentPixel = originalTexture.GetPixel(i, j);
					if(currentPixel.a != 0.0f){
						if(CheckColorEquality(currentPixel, noninvertedEdge)){
							currentPixel = invertedEdge;
						} else if(CheckColorEquality(currentPixel, noninvertedSkin)){
							currentPixel = invertedSkin;
						}
						originalTexture.SetPixel(i, j, currentPixel);
					}
				}
			}
		} else {
			for(int i = 0; i < originalTexture.width; i++){
				for(int j = 0; j < originalTexture.height; j++){
					Color currentPixel = originalTexture.GetPixel(i, j);
					if(currentPixel.a != 0.0f){
						if(CheckColorEquality(currentPixel, invertedEdge)){
							currentPixel = noninvertedEdge;
						} else if(CheckColorEquality(currentPixel, invertedSkin)){
							currentPixel = noninvertedSkin;
						}
						originalTexture.SetPixel(i, j, currentPixel);
					}
				}
			}
		}
		
		originalTexture.Apply();
	}
	
	bool CheckColorEquality(Color a, Color b){
		return Mathf.Approximately(a.r, b.r) && Mathf.Approximately(a.g, b.g) && Mathf.Approximately(a.b, b.b);
	}
}
