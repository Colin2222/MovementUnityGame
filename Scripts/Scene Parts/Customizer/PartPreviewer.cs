using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartPreviewer : MonoBehaviour
{
	public CustomizerColorPalette colorManager;
	public SpriteRenderer rightArrow;
	public SpriteRenderer leftArrow;
	public SpriteRenderer renderer;
    public Sprite[] previews;
	int currentSelection = 0;
	int currentColor = 0;
	
	public (int, int) GetSelection(){
		return (currentSelection, currentColor);
	}
	
	public void SetSelection((int type, int color) dataTuple){
		currentSelection = dataTuple.type;
		currentColor = dataTuple.color;
	}
	
	public void MoveSelectionRight(){
		currentSelection++;
		if(currentSelection >= previews.Length){
			currentSelection = 0;
		}
		UpdateSelectionImage();
	}
	
	public void MoveSelectionLeft(){
		currentSelection--;
		if(currentSelection < 0){
			currentSelection = previews.Length - 1;
		}
		UpdateSelectionImage();
	}
	
	public void UpdateSelectionImage(){
		if(previews.Length != 0){
			renderer.sprite = previews[currentSelection];
			renderer.color = colorManager.GetColor(currentColor);
		}
	}
	
	public void MoveColorRight(){
		currentColor++;
		if(currentColor >= colorManager.GetColorCollectionSize()){
			currentColor = 0;
		}
		UpdateSelectionImage();
	}
	
	public void MoveColorLeft(){
		currentColor--;
		if(currentColor < 0){
			currentColor = colorManager.GetColorCollectionSize() - 1;
		}
		UpdateSelectionImage();
	}
}
