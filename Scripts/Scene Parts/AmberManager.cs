using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject amberRunUICanvasPrefab;
    public GameObject returnOfferObject;
    public Image amberImage;
    float[] crackTimes = new float[3];
    int currentCrackStage = 0;
    float nextCrackTime;
    public Animator amberCrackAnimator;
    public float postShatterTime;
    public List<AmberColorMapping> amberColorMappings;
    // Start is called before the first frame update
    void Start()
    {
        amberRunUICanvasPrefab.SetActive(false);
        returnOfferObject.SetActive(false);
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
                amberCrackAnimator.Play("AmberShatter");
                returnOfferObject.SetActive(true);
            }
            else if (amberRunTimer <= nextCrackTime)
            {
                HandleCrack();
            }
        }
        else if (inAmberRunCooldown)
        {
            amberRunCooldownTimer -= Time.deltaTime;
            if (amberRunCooldownTimer <= 0)
            {
                inAmberRunCooldown = false;
                returnOfferObject.SetActive(false);
                amberRunUICanvasPrefab.SetActive(false);
            }
        }
    }

    public void StartAmberRun(string amberType, float runTime)
    {
        inAmberRun = true;
        amberRunTimer = runTime;
        currentHeldAmber = amberType;
        originalAmberPoolRoomIndex = SceneManager.Instance.sceneBuildIndex;
        SetAmberUIColor(amberType);
        SetCrackTimes(runTime);
        amberRunUICanvasPrefab.SetActive(true);
        amberCrackAnimator.Play("AmberStage0");
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
        amberRunUICanvasPrefab.SetActive(false);
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
        returnOfferObject.SetActive(false);
        amberRunUICanvasPrefab.SetActive(false);
    }

    void SetAmberUIColor(string amberType)
    {
        foreach (AmberColorMapping mapping in amberColorMappings)
        {
            if (mapping.amberType == amberType)
            {
                amberImage.color = mapping.amberColor;
                return;
            }
        }
    }

    void SetCrackTimes(float runTime)
    {
        crackTimes[0] = runTime * 0.66f;
        crackTimes[1] = runTime * 0.33f;
        crackTimes[2] = runTime * 0.1f;
        nextCrackTime = crackTimes[0];
        currentCrackStage = 0;
    }

    void HandleCrack()
    {
        currentCrackStage++;
        amberCrackAnimator.Play("AmberStage" + currentCrackStage);

        if (currentCrackStage >= crackTimes.Length)
        {
            nextCrackTime = -1;
        }
        else
        {
            nextCrackTime = crackTimes[currentCrackStage];
        }
    }
}

[System.Serializable]
public class AmberColorMapping
{
    public string amberType;
    public Color amberColor;
}
