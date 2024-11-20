using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IInteractable : MonoBehaviour
{
    public float cameraDistance;

    public abstract void Interact();
    public abstract void LeaveInteraction();
}
