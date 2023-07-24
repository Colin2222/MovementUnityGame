using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class LevelRegistry
{
	static LevelRegistry inst;
	public static LevelRegistry Instance(){
		if(inst == null){
			inst = new LevelRegistry();
		}
		return inst;
	}
	LevelRegistry(){
		levels = new Dictionary<int, string>();
		levelIds = new List<int>();
	}
	
	Dictionary<int, string> levels;
	List<int> levelIds;
	bool loaded = false;
	
    public void LoadLevels(string registryXmlFilePath){
		if(!loaded){
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load("Assets/Resources/" + registryXmlFilePath + ".xml");
			XmlNodeList levelNodes = xmlDoc.SelectNodes("levels/level");
			
			foreach (XmlNode levelNode in levelNodes){
				int id = int.Parse(levelNode.SelectSingleNode("id").InnerText);
				string displayName = levelNode.SelectSingleNode("display_name").InnerText;
				
				levels.Add(id, displayName);
				levelIds.Add(id);
			}
			
			loaded = true;
		}
	}
	
	public static List<int> GetLevelIdList(){
		return Instance().levelIds;
	}
}
