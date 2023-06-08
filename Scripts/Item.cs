using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	public string id;
	public string displayName;
	[Header("0=BasicItem, 1=Gem, 2=Cassette")]
	public int type;
	public Sprite sprite;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
