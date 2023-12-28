using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentState : MonoBehaviour
{
    public int entranceNumber;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
