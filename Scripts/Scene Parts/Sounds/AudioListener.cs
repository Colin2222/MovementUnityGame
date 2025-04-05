using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListener : MonoBehaviour
{
    public GameObject targetObject;
    public float speed = 4.0f;
    // Start is called before the first frame update
    void Awake()
    {
        if(targetObject == null)
        {
            targetObject = GameObject.FindWithTag("Player");
        }
        transform.position = targetObject.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, targetObject.transform.position, speed * Time.deltaTime);
    }
}
