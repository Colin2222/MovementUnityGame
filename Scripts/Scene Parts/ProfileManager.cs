using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;
using TMPro;
using System.Linq;

public class ProfileManager : MonoBehaviour
{
	public string profileXmlFile;
	public float profileSpacing;
	public GameObject profileSelectorPrefab;
	public TextMeshProUGUI nameText;
	public PlayerHub player;
	
	[System.NonSerialized]
	public PlayerProfile currentProfile;
	Dictionary<int, PlayerProfile> profiles;
	List<int> profileIds;
	
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
	
	public void SetupProfileSelection(Transform starterLocation){
		profiles = new Dictionary<int, PlayerProfile>();
		profileIds = new List<int>();
		
		// Load the XML file
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load("Assets/Resources/" + profileXmlFile + ".xml");

        // Get the root element of the XML file
        XmlNodeList playerNodes = xmlDoc.SelectNodes("players/player");

        // Iterate through each player
		int index = 0;
        foreach (XmlNode playerNode in playerNodes)
        {
			// set name and id
			string displayName = playerNode.SelectSingleNode("display_name").InnerText;
			string spritesheetCode = playerNode.SelectSingleNode("spritesheet_code").InnerText;
			int id = int.Parse(playerNode.SelectSingleNode("id").InnerText);
			
			// set best times
			Dictionary<int, float> timeDict = new Dictionary<int, float>();
			XmlNodeList timeNodes = playerNode.SelectNodes("time_entry");
			foreach(XmlNode timeNode in timeNodes){
				int levelId = int.Parse(timeNode.SelectSingleNode("level_id").InnerText);
				float bestTime = float.Parse(timeNode.SelectSingleNode("time").InnerText);
				
				timeDict.Add(levelId, bestTime);
			}
			
			// set customization codes
			List<(int, int)> customizationCodes = new List<(int, int)>();
			XmlNode customizationNode = playerNode.SelectSingleNode("customization_codes");
			(int, int) headerInsertion = (int.Parse(customizationNode.SelectSingleNode("header_id").InnerText), int.Parse(customizationNode.SelectSingleNode("header_color_id").InnerText));
			(int, int) headInsertion = (int.Parse(customizationNode.SelectSingleNode("head_id").InnerText), int.Parse(customizationNode.SelectSingleNode("head_color_id").InnerText));
			(int, int) bodyInsertion = (int.Parse(customizationNode.SelectSingleNode("body_id").InnerText), int.Parse(customizationNode.SelectSingleNode("body_color_id").InnerText));
			customizationCodes.Add(headerInsertion);
			customizationCodes.Add(headInsertion);
			customizationCodes.Add(bodyInsertion);
			
            // create profile for interactable
			PlayerProfile newProfile = new PlayerProfile(id, displayName, spritesheetCode);
			newProfile.bestTimes = timeDict;
			newProfile.customizationCodes = customizationCodes;
			profiles.Add(id, newProfile);
			profileIds.Add(id);
			
			// create selectable player profile if relevant
			if(starterLocation != null){
				GameObject instance = Instantiate(profileSelectorPrefab);
				instance.transform.position = new Vector3(starterLocation.position.x + (profileSpacing * index), starterLocation.position.y, 0.0f);
				ProfileSelectionInteractable interactable = instance.GetComponent<ProfileSelectionInteractable>();
				interactable.profileManager = gameObject.GetComponent<ProfileManager>();
				interactable.profileId = id;
				CustomizerColorPalette colorManager = GameObject.FindWithTag("ColorPaletteManager").GetComponent<CustomizerColorPalette>();
				interactable.SetCustomization(customizationCodes, colorManager);
			}
			
			// update index for spacing purposes
			index++;
        }
	}
	
	public void SetCurrentProfile(int profileId){
		currentProfile = profiles[profileId];
		GameObject.FindWithTag("CustomizerManager").GetComponent<CustomizerManager>().StitchNewPlayerSpritesheet(profiles[profileId].customizationCodes);
		player.SwitchPlayerSpritesheet("currentplayer");
		nameText.text = currentProfile.displayName;
	}
	
	public void RegisterNewTime(float time){
		if(currentProfile == null){
			return;
		}
		
		
		int playerId = currentProfile.id;
		int levelId = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
		
		// Load the XML file
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.Load("Assets/Resources/" + profileXmlFile + ".xml");
		
		// check if new level time needs to be added to profile in xml
		if(!profiles[playerId].bestTimes.ContainsKey(levelId)){
			profiles[playerId].bestTimes.Add(levelId, time);
			
			// Get the root element of the XML file
			XmlNodeList playerNodes = xmlDoc.SelectNodes("players/player");

			// Iterate through each player
			foreach (XmlNode playerNode in playerNodes)
			{
				// Get the ID of the player
				XmlNode idNode = playerNode.SelectSingleNode("id");
				int currentPlayerId = int.Parse(idNode.InnerText);

				// Check if the current player has the given ID
				if (currentPlayerId == playerId)
				{
					XmlNode entryNode = xmlDoc.CreateNode("element", "time_entry", "");
					XmlNode timeNode = xmlDoc.CreateNode("element", "time", "");
					XmlNode levelIdNode = xmlDoc.CreateNode("element", "level_id", "");
					
					timeNode.InnerText = time.ToString();
					levelIdNode.InnerText = levelId.ToString();
					
					playerNode.AppendChild(entryNode);
					entryNode.AppendChild(levelIdNode);
					entryNode.AppendChild(timeNode);
					
					xmlDoc.Save("Assets/Resources/" + profileXmlFile + ".xml");
					break; // Break the loop once the player is found and updated
				}
			}
		} else if(time < profiles[playerId].bestTimes[levelId]){
			profiles[playerId].bestTimes[levelId] = time;
			
			// Get the root element of the XML file
			XmlNodeList playerNodes = xmlDoc.SelectNodes("players/player");

			// Iterate through each player
			foreach (XmlNode playerNode in playerNodes)
			{
				// Get the ID of the player
				XmlNode idNode = playerNode.SelectSingleNode("id");
				int currentPlayerId = int.Parse(idNode.InnerText);

				// Check if the current player has the given ID
				if (currentPlayerId == playerId)
				{
					// Get the time_entry node for the given level ID
					XmlNodeList timeEntryNodes = playerNode.SelectNodes("time_entry");
					foreach (XmlNode timeEntryNode in timeEntryNodes)
					{
						XmlNode levelIdNode = timeEntryNode.SelectSingleNode("level_id");
						int currentLevelId = int.Parse(levelIdNode.InnerText);

						// Check if the current time_entry node is for the given level ID
						if (currentLevelId == levelId)
						{
							// Update the time attribute for the given level
							XmlNode timeNode = timeEntryNode.SelectSingleNode("time");
							timeNode.InnerText = time.ToString();
							xmlDoc.Save("Assets/Resources/" + profileXmlFile + ".xml");
							break; // Break the loop once the level time is updated
						}
					}
					break; // Break the loop once the player is found and updated
				}
			}
		}
	}
	
	// used by scene manager to sync profileManager to UI element to display profile name when a new scene is loaded in
	public void SetNameTextRef(TextMeshProUGUI newText){
		nameText = newText;
		if(currentProfile != null){
			nameText.text = currentProfile.displayName;
		}
	}
	
	public void SetPlayerRef(PlayerHub newPlayer){
		player = newPlayer;
	}
	
	public void ResetPlayerSpritesheet(){
		if(currentProfile != null){
			player.SwitchPlayerSpritesheet("currentplayer");
		}
	}
	
	public Queue<(string, float)> GetLevelLeaderboardData(int numEntries){
		Dictionary<int, float> times = new Dictionary<int, float>();
		int levelId = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
		
		foreach(KeyValuePair<int, PlayerProfile> entry in profiles){
			float timeInsertion;
			try{
				times.Add(entry.Key, entry.Value.bestTimes[levelId]);
			} catch(KeyNotFoundException e){
				Debug.Log(e);
			}
			
		}
		
		var topEntries = times.OrderByDescending(pair => pair.Value).Take(numEntries);
		List<int> bestTimeProfileIds = new List<int>();
		foreach (KeyValuePair<int,float> entry in times.OrderBy(key=> key.Value).Take(numEntries)){ 
			bestTimeProfileIds.Add(entry.Key);
		}
		
		Queue<(string, float)> data = new Queue<(string, float)>();
		foreach(int id in bestTimeProfileIds){
			(string, float) insertion = (profiles[id].displayName, profiles[id].bestTimes[levelId]);
			data.Enqueue(insertion);
		}
		
		return data;
	}
	
	public List<(int, int)> GetCustomizationData(){
		return currentProfile.customizationCodes;
	}
	
	public void SetCustomizationData(List<(int type, int colorId)> codes){
		int playerId = currentProfile.id;
		
		// Load the XML file
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.Load("Assets/Resources/" + profileXmlFile + ".xml");
		
		// Get the root element of the XML file
		XmlNodeList playerNodes = xmlDoc.SelectNodes("players/player");

		// Iterate through each player
		foreach (XmlNode playerNode in playerNodes)
		{
			// Get the ID of the player
			XmlNode idNode = playerNode.SelectSingleNode("id");
			int currentPlayerId = int.Parse(idNode.InnerText);

			// Check if the current player has the given ID
			if (currentPlayerId == playerId)
			{
				
				// Get the customization_codes node for the current player profile
				XmlNode customizationNode = playerNode.SelectSingleNode("customization_codes");
				
				customizationNode.SelectSingleNode("header_id").InnerText = codes[0].type.ToString();
				customizationNode.SelectSingleNode("header_color_id").InnerText = codes[0].colorId.ToString();
				customizationNode.SelectSingleNode("head_id").InnerText = codes[1].type.ToString();
				customizationNode.SelectSingleNode("head_color_id").InnerText = codes[1].colorId.ToString();
				customizationNode.SelectSingleNode("body_id").InnerText = codes[2].type.ToString();
				customizationNode.SelectSingleNode("body_color_id").InnerText = codes[2].colorId.ToString();
				
				currentProfile.customizationCodes[0] = (codes[0].type, codes[0].colorId);
				currentProfile.customizationCodes[1] = (codes[1].type, codes[1].colorId);
				currentProfile.customizationCodes[2] = (codes[2].type, codes[2].colorId);
				
				xmlDoc.Save("Assets/Resources/" + profileXmlFile + ".xml");
				break; // Break the loop once the player is found and updated
			}
		}
	}
	
	public void UpdateSelectionPoint(List<(int, int)> customizationData){
		if(currentProfile != null){
			GameObject[] selectionObjects = GameObject.FindGameObjectsWithTag("ProfileSelection");
			foreach(GameObject instance in selectionObjects){
				ProfileSelectionInteractable interactable = instance.GetComponent<ProfileSelectionInteractable>();
				if(interactable.profileId == currentProfile.id){
					interactable.SetCustomization(customizationData, GameObject.FindWithTag("ColorPaletteManager").GetComponent<CustomizerColorPalette>());
				}
			}
		}
	}
}
