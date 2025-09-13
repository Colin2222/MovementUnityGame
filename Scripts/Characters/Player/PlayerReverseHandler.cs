using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReverseHandler : MonoBehaviour
{
    public Transform reversibleCheckPoint;
    public float reversibleCheckDistance;
    Transform lastReversible;
    LayerMask reversibleLayer;
    // Start is called before the first frame update
    void Start()
    {
        reversibleLayer = LayerMask.GetMask("Reversibles");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool ReversiblePresent(int direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(reversibleCheckPoint.position, Vector2.right * direction, reversibleCheckDistance, reversibleLayer);
        Debug.DrawRay(reversibleCheckPoint.position, Vector2.right * direction * reversibleCheckDistance, Color.red, 1.0f);
        if (hit.collider != null)
        {
            lastReversible = hit.collider.transform;
            return true;
        }
        return false;
    }

    public void ResetReversibleReference()
    {
        lastReversible = null;
    }

    public Transform GetLastReversible()
    {
        Debug.Log("Getting Last Reversible: " + lastReversible.name + " " + lastReversible.position);
        return lastReversible;
    }
}
