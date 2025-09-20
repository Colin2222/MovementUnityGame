using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrashableCornerScript : MonoBehaviour
{
    public FillScript fill;
    public int thrashCount = 3;
    int currentThrash = 0;
    public FadeSpriteScript coverSprites;
    public WallBreakScript wallBreak;
    
    // Start is called before the first frame update
    void Start()
    {
        if (fill.active == false)
        {
            Destroy(wallBreak.gameObject);
            Destroy(coverSprites.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool Thrash()
    {
        if(fill.active == false){
            return false;
        }

        currentThrash++;
        if (currentThrash >= thrashCount)
        {
            SceneManager.Instance.fillManager.SetFill(fill.id, false);
            coverSprites.StartFadeOut();
            wallBreak.BreakWall();
            return true;
        }
        else
        {
            wallBreak.crackAnimator.Play("breakable_wall_1_stage_" + currentThrash);
        }
        return false;
    }
}
