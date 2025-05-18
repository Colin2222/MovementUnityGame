using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
	public PlayerHub player;
	public Transform raycastPoint;
	public float raycastDistance = 1f;
	LayerMask mask;

	void Start()
	{
		mask = LayerMask.GetMask("Interactable");
	}
	
    public Interactable Interact()
	{
		RaycastHit2D raycastHit = Physics2D.Raycast(raycastPoint.position, Vector2.right * Mathf.Sign(player.stateManager.GetDirection()), raycastDistance, mask);
		if (raycastHit.collider != null)
		{
			Interactable iFace = raycastHit.collider.GetComponent<Interactable>();
			iFace.Interact();
			return iFace;
		}
		return null;
	}
}
