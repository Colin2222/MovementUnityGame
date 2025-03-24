using UnityEditor;
using UnityEngine;
using SceneImport=UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.IO;


public class SceneArtWaterGenerator : EditorWindow
{
	private TextField outputField;
	private TextField inputReferenceField;
	private TextField inputWidthField;
	private TextField inputHeightField;
    private TextField inputSliceWidthField;
    private TextField inputNumFrames;
    private TextField inputFrameBuffer;
	private Button buildButton;
	private ColorField templateColorField;
	
    [MenuItem("Automation/SceneArtWaterGenerator")]
    public static void ShowExample()
    {
        SceneArtWaterGenerator wnd = GetWindow<SceneArtWaterGenerator>();
        wnd.titleContent = new GUIContent("SceneArtWaterGenerator");
		
		wnd.position = new Rect(wnd.position.x, wnd.position.y, 500, 400);
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label();
        root.Add(label);

        inputReferenceField = new TextField();
		inputReferenceField.label = "Water reference image (include extension)";
		inputReferenceField.value = "Assets\\Art\\AutomationReference\\water_reference.png";
		root.Add(inputReferenceField);
		
		outputField = new TextField();
		outputField.label = "Output file path (include extension)";
		outputField.value = "Assets\\Art\\sp_art\\rooms\\test_water.png";
		root.Add(outputField);
		
		inputWidthField = new TextField();
		inputWidthField.label = "Water width (pixels)";
		inputWidthField.value = "100";
		root.Add(inputWidthField);

        inputHeightField = new TextField();
        inputHeightField.label = "Water height (pixels)";
        inputHeightField.value = "51";
        root.Add(inputHeightField);

        inputSliceWidthField = new TextField();
		inputSliceWidthField.label = "Sample slice width from reference";
		inputSliceWidthField.value = "20";
		root.Add(inputSliceWidthField);

        inputNumFrames = new TextField();
		inputNumFrames.label = "Number of frames";
		inputNumFrames.value = "3";
		root.Add(inputNumFrames);

        inputFrameBuffer = new TextField();
		inputFrameBuffer.label = "Pixel buffer between frames";
		inputFrameBuffer.value = "20";
		root.Add(inputFrameBuffer);
		
		buildButton = new Button();
        buildButton.text = "Generate Water Spritesheet";
		buildButton.clicked += OnGenerateWater;
        root.Add(buildButton);
    }
	
	private void OnGenerateWater(){
        int width = int.Parse(inputWidthField.value);
        int height = int.Parse(inputHeightField.value);
        int sliceWidth = int.Parse(inputSliceWidthField.value);
        int numFrames = int.Parse(inputNumFrames.value);
        int frameBuffer = int.Parse(inputFrameBuffer.value);
        int currentX = 0;

        // Load the reference texture
        string referencePath = inputReferenceField.value;
        Texture2D referenceTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(referencePath);

        Texture2D texture = new Texture2D(width, ((numFrames * height) + ((numFrames - 1) * frameBuffer)), TextureFormat.RGBA32, false);
        // transparent fill (why does default texture creation fill with some transparent gray that I have to do this?)
        Color[] transparentPixels = new Color[texture.width * texture.height];
        for (int i = 0; i < transparentPixels.Length; i++)
        {
            transparentPixels[i] = Color.clear;
        }
        texture.SetPixels(transparentPixels);

        for(int currentFrame = 0; currentFrame < int.Parse(inputNumFrames.value); currentFrame++){
            currentX = 0;
            int currentY = currentFrame * (height + frameBuffer);
            while(currentX < width - 1){
                int randomX = Random.Range(0, referenceTexture.width - sliceWidth);
                Color[] sliceColors = referenceTexture.GetPixels(randomX, 0, sliceWidth, referenceTexture.height);
                texture.SetPixels(currentX, currentY, sliceWidth, height, sliceColors);
                currentX += sliceWidth;
            }
        }
        texture.Apply();
        byte[] bytes = texture.EncodeToPNG();
        string path = outputField.value;
		if(path.Length >= 5 && path.Substring(path.Length - 4) == ".png"){
			File.WriteAllBytes(path, bytes);
			AssetDatabase.Refresh();
			Debug.Log("PNG saved to: " + path);
		} else{
			Debug.Log("Did not save due to incomplete output filepath");
		}
	}
}
