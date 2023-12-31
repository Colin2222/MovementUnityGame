using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalCanvasScript : MonoBehaviour
{
	public GameObject journalObject;
	public Image writingImage; 
	
	public void SetPageImages(Sprite writingSprite){
		writingImage.sprite = writingSprite;
	}
}
