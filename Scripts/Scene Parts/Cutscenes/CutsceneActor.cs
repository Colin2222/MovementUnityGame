using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneActor : MonoBehaviour
{
    public Animator animator;
	public int id;
	public Rigidbody2D rb;
	
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
}
