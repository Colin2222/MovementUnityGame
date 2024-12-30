using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.AddressableAssets;

public class NPCReskinManager : MonoBehaviour
{
    public string spritesheetCode;
    public SpriteRenderer spriteRenderer;
	
	public Color invertedEdge;
	public Color noninvertedEdge;
	public Color invertedSkin;
	public Color noninvertedSkin;

    string addressHeader = "Assets/Art/NPCs/";

    public bool inverted;

    // Start is called before the first frame update
    void Start()
    {
        SetPlayerInvertSkin(inverted);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerInvertSkin(bool inverted){
		var operation = Addressables.LoadAssetAsync<Texture2D>(addressHeader + spritesheetCode + ".png");
		Texture2D originalTexture = operation.WaitForCompletion();
		bool firstFound = false;
		
		if(inverted){
			for(int i = 0; i < originalTexture.width; i++){
				for(int j = 0; j < originalTexture.height; j++){
					Color currentPixel = originalTexture.GetPixel(i, j);
					if(currentPixel.a != 0.0f){
						if(!firstFound && CheckColorEquality(currentPixel, invertedEdge)){
							return;
						} else{
							firstFound = true;
						}
						
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
						if(!firstFound && CheckColorEquality(currentPixel, noninvertedEdge)){
							return;
						} else{
							firstFound = true;
						}
						
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
