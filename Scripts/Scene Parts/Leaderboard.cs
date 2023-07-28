using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
	ProfileManager profileManager;
	public int numEntries;
	public float rowHeight;
	public Transform rowStartPos;
	public GameObject rowPrefab;
	List<LeaderboardRow> rows;
    // Start is called before the first frame update
    void Awake()
    {
        profileManager = GameObject.FindWithTag("ProfileManager").GetComponent<ProfileManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void ActivateLeaderboard(){
		rows = new List<LeaderboardRow>();
		
		Queue<(string, float)> data = profileManager.GetLevelLeaderboardData(numEntries);
		int numRows = 0;
		while(data.Count > 0){
			GameObject rowObject = Instantiate(rowPrefab, gameObject.transform);
			rowObject.transform.position = new Vector3(rowStartPos.position.x, rowStartPos.position.y - (rowHeight * numRows), 0.0f);
			LeaderboardRow rowInsertion = rowObject.GetComponent<LeaderboardRow>();
			(string name, float time) pair = data.Dequeue();
			rowInsertion.nameText.text = pair.name;
			rowInsertion.timeText.text = pair.time.ToString();
			rows.Add(rowInsertion);
			numRows++;
		}
	}
}
