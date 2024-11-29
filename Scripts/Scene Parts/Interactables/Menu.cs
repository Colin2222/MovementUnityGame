using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    string menu_type;
    List<MenuElement> free_elements;
    MenuElement[,] menu_elements;
    int menuWidth;
    int menuHeight;
    (int x, int y) currentPos;
	(int x, int y) selectionPos;
    List<Inventory> inventories;
    void Awake(){
        
    }

    public Menu(){
        
    }

    public void MoveUp(){
		if(currentPos.y > 0){
			(currentPos.y)--;
			//canvas.ChangeSelection(currentPos.y, currentPos.x);
		}
	}
	
	public void MoveDown(){
		if(currentPos.y < menuHeight - 1){
			(currentPos.y)++;
			//canvas.ChangeSelection(currentPos.y, currentPos.x);
		}
	}
	
	public void MoveRight(){
		if(currentPos.x < menuWidth - 1){
			(currentPos.x)++;
			//canvas.ChangeSelection(currentPos.y, currentPos.x);
		}
	}
	
	public void MoveLeft(){
		if(currentPos.x > 0){
			(currentPos.x)--;
			//canvas.ChangeSelection(currentPos.y, currentPos.x);
		}
	}
}

[System.Serializable]
class MenuElement{
    int screenX;
    int screenY;
    List<MenuElementPointer> extensions;
}

[System.Serializable]
class MenuElementPointer : MenuElement{
    MenuElement coreElement;
}

[System.Serializable]
class MenuSlot : MenuElement{
	
}

[System.Serializable]
class MenuButton : MenuElement{
    
}

[System.Serializable]
class MenuText : MenuElement{
    
}
