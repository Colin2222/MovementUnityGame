using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHub : MonoBehaviour
{
    public string npcName;
    public Rigidbody2D rigidbody;
    public NPCReskinManager reskinner;
    public Animator animator;
    public CutsceneActor cutsceneActor;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDirection(int direction){
		if(direction > 0){
			if(transform.eulerAngles.y == 180){
				transform.eulerAngles = new Vector2(0,0);
			}
		}
		if(direction < 0){
			if(transform.eulerAngles.y == 0){
				transform.eulerAngles = new Vector2(0,180);
			}
		}
	}
}
