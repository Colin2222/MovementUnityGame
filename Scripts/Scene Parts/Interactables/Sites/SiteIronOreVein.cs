using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteIronOreVein : Site
{
    public SpriteRenderer spriteRenderer;
    public Sprite minedSprite;
    public Sprite unminedSprite;
    public Transform leftSwingPoint;
    public Transform rightSwingPoint;

    bool inSwing = false;
    bool inPostSwing = false;
    float swingTimer;
    public float swingTime;
    float postSwingTimer;
    public float postSwingTime;
    public float swingForgetTime;
    float swingForgetTimer;
    int numSwings = 0;
    bool mined = false;
    public GameObject itemPrefab;
    int numOrePieces;
    public float oreSpreadDistance;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!mined)
        {
            if (numSwings > 0)
            {
                swingForgetTimer += Time.deltaTime;
                if (swingForgetTimer >= swingForgetTime)
                {
                    numSwings = 0;
                }
            }

            if (inPostSwing)
            {
                postSwingTimer += Time.deltaTime;
                if (postSwingTimer >= postSwingTime)
                {
                    inPostSwing = false;
                    inSwing = false;
                    PlayerHub.Instance.inputManager.UnlockPlayer();
                    PlayerHub.Instance.stateManager.ResetPlayer();
                }
                return;
            }

            if (inSwing)
            {
                swingTimer += Time.deltaTime;
                if (swingTimer >= swingTime)
                {
                    numSwings++;
                    swingForgetTimer = 0.0f;
                    ResetTimers();
                    inSwing = false;
                    inPostSwing = true;

                    if (numSwings >= 5)
                    {
                        for (int i = 0; i < numOrePieces; i++)
                        {
                            float xOffset = Random.Range(-oreSpreadDistance, oreSpreadDistance);
                            Vector3 dropPosition = new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z);
                            WorldItem droppedItem = Instantiate(itemPrefab, dropPosition, Quaternion.identity).GetComponent<WorldItem>();
                            droppedItem.item_id = "iron_ore";
                        }
                        mined = true;
                        inSwing = false;
                        hasMenu = false;
                        spriteRenderer.sprite = minedSprite;
                        PlayerHub.Instance.inputManager.UnlockPlayer();
                        PlayerHub.Instance.stateManager.ResetPlayer();
                    }
                }
                return;
            }
        }
    }

    void ResetTimers()
    {
        swingTimer = 0.0f;
        postSwingTimer = 0.0f;
    }

    protected override void EnterRange(){
        
    }

    protected override void ExitRange(){
        
    }

    public override void Interact()
    {
        if (mined)
        {
            hasMenu = false;
            PlayerHub.Instance.inputManager.UnlockPlayer();
            PlayerHub.Instance.stateManager.ResetPlayer();
            return;
        }

        if (!(Mathf.Abs(PlayerHub.Instance.transform.position.x - rightSwingPoint.position.x) <= 0.05f ||
           Mathf.Abs(PlayerHub.Instance.transform.position.x - leftSwingPoint.position.x) <= 0.05f))
        {
            if (PlayerHub.Instance.transform.position.x > transform.position.x)
            {
                PlayerHub.Instance.overrideManager.WalkToPoint(rightSwingPoint.position.x, transform, false);
            }
            else
            {
                PlayerHub.Instance.overrideManager.WalkToPoint(leftSwingPoint.position.x, transform, false);
            }
        }
        else
        {
            PlayerHub.Instance.animator.Play("PlayerAxeSwingRepeat");
            ResetTimers();
            inSwing = true;
            inPostSwing = false;
        }
    }
    
    public override void LeaveInteraction(){
        
    }

    public override void MenuUp(){
        
    }

    public override void MenuDown(){
        
    }

    public override void MenuLeft(){
        
    }

    public override void MenuRight(){
        
    }

    public override void MenuSelect(){
        
    }

    public override void MenuInteract()
    {
        if(!mined && inPostSwing){
            PlayerHub.Instance.animator.Play("PlayerAxeSwingRepeat", 0, 0.0f);
            ResetTimers();
            inSwing = true;
            inPostSwing = false;
            return;
        }
    }

    public override void LoadSite(SavedSite savedSite)
    {
        numOrePieces = int.Parse(savedSite.additional_data["num_ore_pieces"]);
        mined = bool.Parse(savedSite.additional_data["mined"]);
        
        if (mined)
        {
            hasMenu = false;
            spriteRenderer.sprite = minedSprite;
        }
        else
        {
            spriteRenderer.sprite = unminedSprite;
        }
    }

    public override SavedSite SaveSite(){
        SavedSite savedSite = new SavedSite();
        savedSite.name = "iron_ore_vein";
        savedSite.additional_data = new Dictionary<string, string>();
        savedSite.additional_data.Add("num_ore_pieces", numOrePieces.ToString());
        savedSite.additional_data.Add("mined", mined.ToString());
        return savedSite;
    }

    public override void ConstructSite(){
        
    }
}
