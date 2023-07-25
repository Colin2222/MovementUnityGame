using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using TMPro;

public class ProfileManager : MonoBehaviour
{
	public string profileXmlFile;
	public float profileSpacing;
	public GameObject profileSelectorPrefab;
	public TextMeshProUGUI nameText;
	
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
			GameObject instance = Instantiate(profileSelectorPrefab);
			instance.transform.position = new Vector3(starterLocation.position.x + (profileSpacing * index), starterLocation.position.y, 0.0f);
			ProfileSelectionInteractable interactable = instance.GetComponent<ProfileSelectionInteractable>();
			interactable.profileManager = gameObject.GetComponent<ProfileManager>();
			
			// set name and id
			string displayName = playerNode.SelectSingleNode("display_name").InnerText;
			int id = int.Parse(playerNode.SelectSingleNode("id").InnerText);
			
			// set best times
			XmlNodeList timeNodes = playerNode.SelectNodes("time_entry");
			foreach(XmlNode timeNode in timeNodes){
				int levelId = int.Parse(timeNode.SelectSingleNode("level_id").InnerText);
				float bestTime = float.Parse(timeNode.SelectSingleNode("time").InnerText);
			}
			
            // create profile for interactable
			PlayerProfile newProfile = new PlayerProfile(id, displayName);
			profiles.Add(id, newProfile);
			profileIds.Add(id);
			interactable.profileId = id;
			
			// update index for spacing purposes
			index++;
        }
	}
	
	public void SetCurrentProfile(int profileId){
		currentProfile = profiles[profileId];
		nameText.text = currentProfile.displayName;
	}
	
	public void RegisterNewTime(int playerId, int levelId, float time){
		if(time < profiles[playerId].bestTimes[levelId]){
			profiles[playerId].bestTimes[levelId] = time;
			
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
}
