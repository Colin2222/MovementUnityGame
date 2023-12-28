using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneActor : MonoBehaviour
{
    public Animator animator;
	public int id;
	public Rigidbody2D rb;
	public SpriteRenderer spriteRenderer;
	public bool deactivatedOnStart;
	
	float tempGrav;
	bool gravityPaused = false;
	Vector2 cutsceneVelocity;
	[System.NonSerialized]
	public CutsceneManager cutsceneManager;
	
	// Start is called before the first frame update
    void Start()
    {
        cutsceneVelocity = Vector2.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		if(cutsceneManager.inCutscene && rb != null){
			rb.velocity = cutsceneVelocity;
		}
    }
	
	public void animate(string animation){
		// TRY AND IMPLEMENT A TRY CATCH HERE, need to figure out type of error when invalid animation string given
		animator.Play(animation);
	}
	
	public void SetHorizontalVelocity(float velocity){
		cutsceneVelocity = new Vector2(velocity, cutsceneVelocity.y);
	}
	
	public void VerticalShift(float shift){
		Transform trans = rb.gameObject.transform;
		trans.position = new Vector3(trans.position.x, trans.position.y + shift, trans.position.z);
	}
	
	public void SetCoordinates(float x, float y){
		Transform trans = rb.gameObject.transform;
		trans.position = new Vector3(x, y, trans.position.z);
	}
	
	public void FaceLeft(){
		Transform trans = rb.gameObject.transform;
		trans.eulerAngles = new Vector2(0,180);
	}
	
	public void FaceRight(){
		Transform trans = rb.gameObject.transform;
		trans.eulerAngles = new Vector2(0,0);
	}
	
	public void ToggleActivation(){
		gameObject.SetActive(!(gameObject.activeInHierarchy));
	}
	
	public void ToggleGravity(){
		if(gravityPaused){
			rb.gravityScale = tempGrav;
			gravityPaused = false;
		} else{
			tempGrav = rb.gravityScale;
			rb.gravityScale = 0.0f;
			gravityPaused = true;
		}
	}
	
	public void SetSpriteOrder(float orderF){
		int order = (int)orderF;
		spriteRenderer.sortingOrder = order;
	}
}
