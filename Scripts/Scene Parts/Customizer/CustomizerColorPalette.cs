using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizerColorPalette : MonoBehaviour
{
    public Color[] colorCollection;
	
	public Color GetColor(int colorId){
		return colorCollection[colorId];
	}
	
	public int GetColorCollectionSize(){
		return colorCollection.Length;
	}
}
