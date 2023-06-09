using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class ItemRegistry
{
	static ItemRegistry inst;
	public static ItemRegistry Instance(){
		if(inst == null){
			inst = new ItemRegistry();
		}
		return inst;
	}
	ItemRegistry(){
		items = new Dictionary<string, Item>();
	}
	
	Dictionary<string, Item> items;
	
	public void LoadItems(string registryXmlFilePath){
		XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load("Assets/Resources/" + registryXmlFilePath + ".xml");
		XmlNode currentNode = xmlDoc.FirstChild.NextSibling.FirstChild;
		
		while(currentNode != null){
			string insertionId = currentNode.Attributes["id"].Value;
			string insertionName = currentNode.Attributes["name"].Value;
			int insertionType = int.Parse(currentNode.Attributes["type"].Value);
			Sprite insertionIcon = Resources.Load<Sprite>("ItemSprites/" + currentNode.Attributes["icon"].Value);
			Item insertion = new Item(insertionId, insertionName, insertionType, insertionIcon, new List<string>());
			items.Add(insertion.id, insertion);
			
			currentNode = currentNode.NextSibling;
		}
	}
	
	public Item GetItem(string key){
		return items[key];
	}
}
