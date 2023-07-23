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
	
    // Start is called before the first frame update
    void Start()
    {
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
			instance.transform.position = new Vector3(gameObject.transform.position.x + (profileSpacing * index), gameObject.transform.position.y, 0.0f);
			ProfileSelectionInteractable interactable = instance.GetComponent<ProfileSelectionInteractable>();
			interactable.profileManager = gameObject.GetComponent<ProfileManager>();
			
			// set name and id
			string displayName = playerNode.SelectSingleNode("display_name").InnerText;
			int id = int.Parse(playerNode.SelectSingleNode("id").InnerText);
			
			// set best times
			
            // create profile for interactable
			PlayerProfile newProfile = new PlayerProfile(id, displayName);
			interactable.profile = newProfile;
			
			// update index for spacing purposes
			index++;
        }
    }
	
	public void SetProfileText(string name){
		nameText.text = name;
	}
}
