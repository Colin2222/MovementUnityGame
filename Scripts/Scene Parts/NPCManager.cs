using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public SceneManager sceneManager;
    Dictionary<string, NPCHub> npcs;

    // Start is called before the first frame update
    void Start()
    {
        LoadNPCs(sceneManager.GetCurrentRoomSave().npcs);
        DetectNPCs();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DetectNPCs()
    {
        npcs = new Dictionary<string, NPCHub>();
        GameObject[] npcObjectArray = GameObject.FindGameObjectsWithTag("NPC");
        foreach (GameObject npcObj in npcObjectArray)
        {
            NPCHub hub = npcObj.GetComponent<NPCHub>();
            npcs.Add(hub.npcName, hub);
        }
    }

    public List<SavedNPC> SaveNPCs()
    {
        DetectNPCs();
        List<SavedNPC> savedNPCs = new List<SavedNPC>();
        foreach (KeyValuePair<string, NPCHub> entry in npcs)
        {
            SavedNPC savedNPC = new SavedNPC();
            savedNPC.name = entry.Key;
            if (entry.Value.defaultPos != null && sceneManager.cutsceneManager.inCutscene)
            {
                savedNPC.x_pos = entry.Value.defaultPos.x;
                savedNPC.y_pos = entry.Value.defaultPos.y;
            }
            else
            {
                savedNPC.x_pos = entry.Value.transform.position.x;
                savedNPC.y_pos = entry.Value.transform.position.y;
            }
            savedNPC.direction = entry.Value.transform.eulerAngles.y == 0 ? 1 : -1;
            savedNPC.default_animation = entry.Value.cutsceneActor.defaultAnim == null? "" : entry.Value.cutsceneActor.defaultAnim;
            savedNPC.inventories = null;
            savedNPCs.Add(savedNPC);
        }
        return savedNPCs;
    }

    public void LoadNPCs(List<SavedNPC> savedNPCs)
    {
        NPCPrefabRegistry registry = sceneManager.npcPrefabRegistry;
        foreach (SavedNPC npc in savedNPCs)
        {
            GameObject npcObj = Instantiate(registry.GetPrefab(npc.name), new Vector3(npc.x_pos, npc.y_pos, 0), Quaternion.identity);
            NPCHub hub = npcObj.GetComponent<NPCHub>();
            hub.cutsceneActor.defaultAnim = npc.default_animation;
            hub.SetDirection(npc.direction);
            if (npc.default_animation != "")
            {
                hub.cutsceneActor.animator.Play(npc.default_animation);
            }
        }
    }

    public void SpawnNPC(string npcName, Vector3 position)
    {
        NPCPrefabRegistry registry = sceneManager.npcPrefabRegistry;
        GameObject npcObj = Instantiate(registry.GetPrefab(npcName), position, Quaternion.identity);
        if (npcs.ContainsKey(npcName))
        {
            NPCHub hubToDelete = npcs[npcName];
            npcs.Remove(npcName);
            Destroy(hubToDelete.gameObject);

        }
        NPCHub hub = npcObj.GetComponent<NPCHub>();
        npcs.Add(npcName, hub);

        if (hub.cutsceneActor != null)
        {
            sceneManager.cutsceneManager.AddActor(hub.cutsceneActor);
        }
    }

    public void SetNPCDefaultAnimation(string npcName, string animationName, string roomName = null)
    {
        if (roomName != null && roomName != sceneManager.sceneName)
        {
            sceneManager.sessionManager.SetNPCDefaultAnimation(npcName, animationName, roomName);
        }
        else
        {
            sceneManager.sessionManager.SetNPCDefaultAnimation(npcName, animationName);
            if (npcs.ContainsKey(npcName))
            {
                NPCHub hub = npcs[npcName];
                hub.cutsceneActor.defaultAnim = animationName;
            }
            else
            {
                Debug.Log("NPC " + npcName + " not found in NPCManager.");
            }
        }
    }

    public void SetNPCPosition(string npcName, Vector2 position, string roomName = null)
    {
        if (roomName != null && roomName != sceneManager.sceneName)
        {
            SessionManager.Instance.SetNPCPosition(npcName, position, roomName);
        }
        else
        {
            SessionManager.Instance.SetNPCPosition(npcName, position);
            if (npcs.ContainsKey(npcName))
            {
                NPCHub hub = npcs[npcName];
                hub.transform.position = position;
            }
            else
            {
                Debug.Log("NPC " + npcName + " not found in NPCManager.");
            }
        }
    }

    public void SetNPCDirection(string npcName, int direction, string roomName = null)
    {
        if (roomName != null && roomName != sceneManager.sceneName)
        {
            SessionManager.Instance.SetNPCDirection(npcName, direction, roomName);
        }
        else
        {
            SessionManager.Instance.SetNPCDirection(npcName, direction);
            if (npcs.ContainsKey(npcName))
            {
                NPCHub hub = npcs[npcName];
                hub.SetDirection(direction);
            }
            else
            {
                Debug.Log("NPC " + npcName + " not found in NPCManager.");
            }
        }
    }
    
    public void SetDefaultNPCPosition(string npcName, Vector2 position)
    {
        if (npcs.ContainsKey(npcName))
        {
            NPCHub hub = npcs[npcName];
            hub.defaultPos = position;
        }
        else
        {
            Debug.Log("NPC " + npcName + " not found in NPCManager.");
        }
    }
}
