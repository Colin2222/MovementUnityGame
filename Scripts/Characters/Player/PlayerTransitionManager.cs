using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransitionManager : MonoBehaviour
{
    public PlayerHub player;
    public Transform raycastPoint;
    public float raycastDistance = 1f;
    LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
        mask = LayerMask.GetMask("Transition");
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public bool CheckForUpDownTransition(){
        RaycastHit2D raycastHit = Physics2D.Raycast(raycastPoint.position, Vector2.up, raycastDistance, mask);
        if (raycastHit.collider != null)
        {
            SceneTransition transition = raycastHit.collider.GetComponent<SceneTransition>();
            if (transition != null && transition.isUpDownTransition)
            {
                player.inputManager.TimeLockPlayer(1.0f);
                return transition.UpDownTransition();
            }
        }
        return false;
    }
}
