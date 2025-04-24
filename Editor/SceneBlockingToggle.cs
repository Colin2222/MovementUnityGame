using UnityEditor;
using UnityEngine;
using SceneImport=UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.IO;

public class SceneBlockingToggle : EditorWindow
{
    private Button onButton;
    private Button offButton;

    [MenuItem("Automation/SceneBlockingToggle")]
    public static void ShowExample()
    {
        SceneBlockingToggle wnd = GetWindow<SceneBlockingToggle>();
        wnd.titleContent = new GUIContent("SceneBlockingToggle");
		
		wnd.position = new Rect(wnd.position.x, wnd.position.y, 500, 400);
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Blocking toggle");
        root.Add(label);

        onButton = new Button();
        onButton.text = "ON";
		onButton.clicked += OnToggleOn;
        root.Add(onButton);

        offButton = new Button();
        offButton.text = "OFF";
		offButton.clicked += OnToggleOff;
        root.Add(offButton);
    }

    private void OnToggleOn(){
        Toggle(true);
    }

    private void OnToggleOff(){
        Toggle(false);
    }

    private void Toggle(bool setting){
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("PhysicsBlock");

        foreach(GameObject parentObj in blocks){
			Transform parentTrans = parentObj.transform;
			for(int i = 0; i < parentTrans.childCount; i++){
				Transform currentTrans = parentTrans.GetChild(i);
				GameObject currentObj = currentTrans.gameObject;
                SpriteRenderer currentSprite = currentObj.GetComponent<SpriteRenderer>();
                if(currentSprite != null){
                    currentSprite.enabled = setting;
                }
			}
		}
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
