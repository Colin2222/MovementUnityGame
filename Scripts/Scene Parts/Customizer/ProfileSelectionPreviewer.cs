using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProfileSelectionPreviewer : MonoBehaviour
{
    public SpriteRenderer headerPreviewer;
	public SpriteRenderer headPreviewer;
	public SpriteRenderer bodyPreviewer;
	public SpriteRenderer legsPreviewer;
	public SpriteRenderer armsPreviewer;
	
	public void SyncCustomizationData(List<(int type, int colorId)> customizationData, CustomizerColorPalette colorManager){
		Sprite[] headerSS = Resources.LoadAll<Sprite>("player_parts/previews/hair_previews");
		headerPreviewer.sprite = Array.Find(headerSS, item => item.name == ("hair_previews_" + customizationData[0].type.ToString()));
		headerPreviewer.color = colorManager.GetColor(customizationData[0].colorId);
		
		Sprite[] headSS = Resources.LoadAll<Sprite>("player_parts/previews/head_previews");
		headPreviewer.sprite = Array.Find(headSS, item => item.name == ("head_previews_" + customizationData[1].type.ToString()));
		headPreviewer.color = colorManager.GetColor(customizationData[1].colorId);
		
		Sprite[] bodySS = Resources.LoadAll<Sprite>("player_parts/previews/body_previews");
		bodyPreviewer.sprite = Array.Find(bodySS, item => item.name == ("body_previews_" + customizationData[2].type.ToString()));
		bodyPreviewer.color = colorManager.GetColor(customizationData[2].colorId);
		
		Sprite[] legsSS = Resources.LoadAll<Sprite>("player_parts/previews/legs_previews");
		legsPreviewer.sprite = Array.Find(legsSS, item => item.name == ("legs_previews_0"));
		
		Sprite[] armsSS = Resources.LoadAll<Sprite>("player_parts/previews/arms_previews");
		armsPreviewer.sprite = Array.Find(armsSS, item => item.name == ("arms_previews_0"));
	}
}
