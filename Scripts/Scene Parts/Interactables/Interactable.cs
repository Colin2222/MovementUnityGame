using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public abstract void Interact();
    public abstract void LeaveInteraction();
    abstract public void MenuUp();
    abstract public void MenuDown();
    abstract public void MenuLeft();
    abstract public void MenuRight();
    abstract public void MenuSelect();
}
