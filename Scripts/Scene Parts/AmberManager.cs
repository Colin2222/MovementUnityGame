using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmberManager : MonoBehaviour
{
    [System.NonSerialized] public bool inAmberRun;
    [System.NonSerialized] public float amberRunTimer;
    [System.NonSerialized] public string currentHeldAmber;
    [System.NonSerialized] public bool timerPaused;
    [System.NonSerialized] public bool inAmberRunCooldown;
    float amberRunCooldownTimer;
    public float amberRunCooldownTime;
    int originalAmberPoolRoomIndex;
    public SessionManager sessionManager;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (inAmberRun && !timerPaused)
        {
            amberRunTimer -= Time.deltaTime;
            if (amberRunTimer <= 0)
            {
                inAmberRun = false;
                inAmberRunCooldown = true;
                amberRunCooldownTimer = amberRunCooldownTime;
            }
        }
        else if (inAmberRunCooldown)
        {
            amberRunCooldownTimer -= Time.deltaTime;
            if (amberRunCooldownTimer <= 0)
            {
                inAmberRunCooldown = false;
            }
        }
    }

    public void StartAmberRun(string amberType, float runTime)
    {
        inAmberRun = true;
        amberRunTimer = runTime;
        currentHeldAmber = amberType;
        originalAmberPoolRoomIndex = SceneManager.Instance.sceneBuildIndex;
    }

    public void CheckAmberRunCompletion(string destinationAmber)
    {
        if (currentHeldAmber == destinationAmber && inAmberRun)
        {
            return;
        }
        inAmberRun = false;
        inAmberRunCooldown = false;
        timerPaused = false;
        Item item = ItemRegistry.Instance().GetItem("amberpure_" + currentHeldAmber);
        PlayerHub.Instance.inventoryHandler.inventory.AddItem(item, 1);
    }

    public void PauseAmberRunTimer()
    {
        timerPaused = true;
    }

    public void UnpauseAmberRunTimer()
    {
        timerPaused = false;
    }
    
    public void ReturnToOriginalAmberPool()
    {
        inAmberRunCooldown = false;
        inAmberRun = false;
        timerPaused = false;
        sessionManager.TransitionScene(originalAmberPoolRoomIndex, 1000, 0);
    }
}
