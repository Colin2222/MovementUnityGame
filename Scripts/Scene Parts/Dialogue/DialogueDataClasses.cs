using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueDataClasses{
    [System.Serializable]
    public class GameDialogueData
    {
        public Dictionary<string, DialogueTree> npc_dialogues;
    }

    [System.Serializable]
    public class DialogueNode
    {
        
    }

    [System.Serializable]
    public class DialogueTree
    {
        public List<DialogueBranch> branches;
    }

    [System.Serializable]
    public class DialogueBranch : DialogueNode
    {
        public List<DialogueCheck> conditions;
        public List<DialogueNode> nodes;
    }

    // type:
    // - progress_marker - check if a progress marker is set to a certain value
    [System.Serializable]
    public class DialogueCheck
    {
        public string type;
        public string key;
        public string value;
    }

    [System.Serializable]
    public class DialogueNodeText : DialogueNode
    {
        public string text;
        public string speaker;
    }

    [System.Serializable]
    public class DialogueNodeChoice : DialogueNode
    {
        public string text;
        public string speaker;
        public float answer_time;
    }

    [System.Serializable]
    public class DialogueNodeDataChange : DialogueNode
    {
        public string data_key;
        public bool data_value;
    }

    [System.Serializable]
    public class DialogueNodeEnd : DialogueNode
    {
        
    }
}
