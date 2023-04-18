using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
	public GameObject playerPrefab;
	
	[System.NonSerialized]
    public PlayerHub player;
	
    GameObject playerStateObjectTest;
    GameObject playerObjectTest;
	
    // Start is called before the first frame update
    void Start()
    {
        // check if there is a DontDestroyOnLoad player, create a new one if there isnt
        playerObjectTest = GameObject.FindWithTag("Player");
        if(playerObjectTest == null){
            player = Instantiate(playerPrefab,new Vector3(0,0,0),Quaternion.identity).GetComponent<PlayerHub>();
        }
        else{
            player = playerObjectTest.GetComponent<PlayerHub>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
