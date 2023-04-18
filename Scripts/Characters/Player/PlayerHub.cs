using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHub : MonoBehaviour
{
	public Rigidbody2D rigidbody;
	public CharacterPhysicsChecker physics;
	public PlayerMover mover;
	public PlayerState state;
	public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
		state = new PlayerState();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
