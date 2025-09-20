using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBreakScript : MonoBehaviour
{
    public Animator crackAnimator;
    public SpriteRenderer originalSprite;
    public AudioSource breakSound;
    public int breakDirection;
    public float breakClearTime;
    float breakTimer;
    bool breaking = false;
    public GameObject[] pieces;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (breaking)
        {
            breakTimer -= Time.deltaTime;
            if (breakTimer < 0.0f)
            {
                breakTimer = 0.0f;
                breaking = false;
                foreach (GameObject piece in pieces)
                {
                    Destroy(piece);
                }
            }
        }
    }

    public void BreakWall()
    {
        breaking = true;
        breakTimer = breakClearTime;
        breakSound.Play();
        for (int i = 0; i < pieces.Length; i++)
        {
            GameObject piece = pieces[i];
            piece.SetActive(true);
            piece.transform.GetChild(0).GetComponent<Animator>().Play("break_rotate_" + (i + 1));
            Rigidbody2D rb = piece.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.simulated = true;
                rb.AddForce(new Vector2(Random.Range(0.5f, 1.0f) * breakDirection, Random.Range(0.0f, 1.0f)) * 2.0f, ForceMode2D.Impulse);
            }
        }
        originalSprite.enabled = false;
    }
}
