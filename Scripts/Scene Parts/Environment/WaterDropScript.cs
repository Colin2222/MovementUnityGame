using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDropScript : MonoBehaviour
{
    bool isSplashing = false;
    public Animator animator;
    public AudioSource splashSound;
    public Rigidbody2D rigidbody;
    public float splashTime;
    float splashTimer;
    // Start is called before the first frame update
    void Start()
    {
        animator.Play("initial_fall");
    }

    // Update is called once per frame
    void Update()
    {
        if(isSplashing)
        {
            splashTimer -= Time.deltaTime;
            if(splashTimer <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(!isSplashing)
        {
            rigidbody.velocity = Vector2.zero;
            animator.Play("initial_splash");
            isSplashing = true;
            splashTimer = splashTime;
            splashSound.pitch = Random.Range(0.8f, 1.0f);
            splashSound.Play();
        }
    }
}
