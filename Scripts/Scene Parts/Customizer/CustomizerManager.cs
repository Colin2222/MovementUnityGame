using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizerManager : MonoBehaviour
{
	PlayerHub player;
	public CustomizerPreviewController previewer;
	public CustomizerColorPalette colorManager;
	public Color replacementColor;
	
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerHub>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void ActivatePreviewer(){
		PlayerHub player = GameObject.FindWithTag("Player").GetComponent<PlayerHub>();
		player.LockPlayer();
		previewer.gameObject.SetActive(true);
		previewer.InitializePreviewer();
	}
	
	public void StitchNewPlayerSpritesheet(List<(int type, int color)> customizationData){
		string spritesheetName = "currentplayer";
		string legsSpritesheetName = "player_parts/legs_0";
		string armsSpritesheetName = "player_parts/arms_0";
		string headerSpritesheetName = "player_parts/hair_" + customizationData[0].type.ToString();
		string headSpritesheetName = "player_parts/head_" + customizationData[1].type.ToString();
		string bodySpritesheetName = "player_parts/body_" + customizationData[2].type.ToString();
		Texture2D originalTexture = Resources.Load<Texture2D>(spritesheetName);
		Texture2D legsTexture = Resources.Load<Texture2D>(legsSpritesheetName);
		Texture2D armsTexture = Resources.Load<Texture2D>(armsSpritesheetName);
		Texture2D headerTexture = Resources.Load<Texture2D>(headerSpritesheetName);
		Texture2D headTexture = Resources.Load<Texture2D>(headSpritesheetName);
		Texture2D bodyTexture = Resources.Load<Texture2D>(bodySpritesheetName);
		
		ClearTexture(originalTexture);
		OverlayTexture(originalTexture, legsTexture, Color.black);
		OverlayTexture(originalTexture, bodyTexture, colorManager.GetColor(customizationData[2].color));
		OverlayTexture(originalTexture, headTexture, colorManager.GetColor(customizationData[1].color));
		OverlayTexture(originalTexture, armsTexture, Color.black);
		OverlayTexture(originalTexture, headerTexture, colorManager.GetColor(customizationData[0].color));
		
		GameObject.FindWithTag("Player").GetComponent<PlayerHub>().SwitchPlayerSpritesheet(spritesheetName);
	}
	
	void OverlayTexture(Texture2D original, Texture2D addition, Color injectedColor){
		for(int i = 0; i < addition.width; i++){
			for(int j = 0; j < addition.height; j++){
				Color currentPixel = addition.GetPixel(i, j);
				if(currentPixel.a != 0.0f){
					if(CheckColorEquality(currentPixel, replacementColor)){
						currentPixel = injectedColor;
					}
					original.SetPixel(i, j, currentPixel);
				}
			}
		}
		
		original.Apply();
	}
	
	bool CheckColorEquality(Color a, Color b){
		return Mathf.Approximately(a.r, b.r) && Mathf.Approximately(a.g, b.g) && Mathf.Approximately(a.b, b.b);
	}
	
	void ClearTexture(Texture2D texture){
		for(int i = 0; i < texture.width; i++){
			for(int j = 0; j < texture.height; j++){
				texture.SetPixel(i, j, Color.clear);
			}
		}
		
		texture.Apply();
	}
}
