using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Cutscene{
    public int id;
	public float duration;
	public Task[] tasks;
}

[System.Serializable]
public class Task{
	public int id;
	public float trigger_time;
	public string anim_name;
	public string custom_action;
}


