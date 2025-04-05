using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Cutscene{
    public int id;
	public bool lock_player;
	public float duration;
	public CutsceneTask[] tasks;
	public bool active;
}

[System.Serializable]
public class CutsceneTask{
	public int id;
	public float trigger_time;
	public string anim_name;
	public CustomAction[] custom_actions;
}

[System.Serializable]
public class CustomAction{
	public string name;
	public float[] parameters;
	public string[] string_parameters;
}


