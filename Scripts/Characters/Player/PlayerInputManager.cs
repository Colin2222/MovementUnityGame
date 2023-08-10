using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
	public PlayerStateManager stateManager;
	
	// joystick movement
	float horizontal;
	float vertical;
	
	// button pressing
    private bool jumpPressed = false;
    private bool jumpJustPressed = false;
	private bool bracePressed = false;
	private bool braceJustPressed = false;
	private bool inventoryPressed = false;
	private bool inventoryJustPressed = false;
	private bool itemGrabPressed = false;
	private bool itemGrabJustPressed = false;
	private bool menuUpPressed = false;
	private bool menuUpJustPressed = false;
	private bool menuDownPressed = false;
	private bool menuDownJustPressed = false;
	private bool menuRightPressed = false;
	private bool menuRightJustPressed = false;
	private bool menuLeftPressed = false;
	private bool menuLeftJustPressed = false;
	private bool menuPageRightPressed = false;
	private bool menuPageRightJustPressed = false;
	private bool menuPageLeftPressed = false;
	private bool menuPageLeftJustPressed = false;
	private bool interactPressed = false;
	private bool interactJustPressed = false;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		stateManager.Move(horizontal, vertical);
		
        jumpJustPressed = false;
		braceJustPressed = false;
		inventoryJustPressed = false;
		itemGrabJustPressed = false;
		menuUpJustPressed = false;
		menuDownJustPressed = false;
		menuRightJustPressed = false;
		menuLeftJustPressed = false;
		menuPageRightJustPressed = false;
		menuPageLeftJustPressed = false;
		interactJustPressed = false;
    }
	
	// controller button methods
    private void OnMove(InputValue value){
        Vector2 vector = value.Get<Vector2>();

        horizontal = vector.x;
        vertical = vector.y;
    }
	
	private void OnJump(){
        jumpPressed = !jumpPressed;
        jumpJustPressed = jumpPressed;
    }
	
	private void OnBrace(){
        bracePressed = !bracePressed;
        braceJustPressed = bracePressed;
    }
	
	private void OnInventory(){
		inventoryPressed = !inventoryPressed;
		inventoryJustPressed = inventoryPressed;
	}
	
	private void OnItemGrab(){
		itemGrabPressed = !itemGrabPressed;
		itemGrabJustPressed = itemGrabPressed;
	}
	
	private void OnMenuUp(){
		menuUpPressed = !menuUpPressed;
		menuUpJustPressed = menuUpPressed;
	}
	
	private void OnMenuDown(){
		menuDownPressed = !menuDownPressed;
		menuDownJustPressed = menuDownPressed;
	}
	
	private void OnMenuRight(){
		menuRightPressed = !menuRightPressed;
		menuRightJustPressed = menuRightPressed;
	}
	
	private void OnMenuLeft(){
		menuLeftPressed = !menuLeftPressed;
		menuLeftJustPressed = menuLeftPressed;
	}
	
	private void OnMenuPageRight(){
		menuPageRightPressed = !menuPageRightPressed;
		menuPageRightJustPressed = menuPageRightPressed;
	}
	
	private void OnMenuPageLeft(){
		menuPageLeftPressed = !menuPageLeftPressed;
		menuPageLeftJustPressed = menuPageLeftPressed;
	}
	
	private void OnInteract(){
		interactPressed = !interactPressed;
		interactJustPressed = interactPressed;
	}
}
