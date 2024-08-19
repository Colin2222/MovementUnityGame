using UnityEditor;
using UnityEngine;
using SceneImport=UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.IO;


public class SceneArtTemplateBuilder : EditorWindow
{
	private TextField outputField;
	private TextField inputHeightField;
	private TextField inputWidthField;
	private TextField inputPixelsPerUnitField;
	private Button buildButton;
	private ColorField templateColorField;
	
	private float minXWorldPos;
	private float minYWorldPos;
	private float maxXWorldPos;
	private float maxYWorldPos;
	private int pixelsPerUnit;
	
    [MenuItem("Automation/SceneArtTemplateBuilder")]
    public static void ShowExample()
    {
        SceneArtTemplateBuilder wnd = GetWindow<SceneArtTemplateBuilder>();
        wnd.titleContent = new GUIContent("SceneArtTemplateBuilder");
		
		wnd.position = new Rect(wnd.position.x, wnd.position.y, 500, 400);
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);
		
		outputField = new TextField();
		outputField.label = "Output file path (include extension)";
		outputField.value = "Assets\\Art\\sp_art\\rooms\\test_room.png";
		root.Add(outputField);
		
		inputHeightField = new TextField();
		inputHeightField.label = "File size vertical buffer";
		inputHeightField.value = "10";
		root.Add(inputHeightField);
		
		inputWidthField = new TextField();
		inputWidthField.label ="File size horizontal buffer";
		inputWidthField.value = "10";
		root.Add(inputWidthField);
		
		inputPixelsPerUnitField = new TextField();
		inputPixelsPerUnitField.label = "Pixels per unit";
		inputPixelsPerUnitField.value = "16";
		root.Add(inputPixelsPerUnitField);
		
		templateColorField = new ColorField();
		templateColorField.label = "Template box color";
		templateColorField.value = Color.magenta;
		root.Add(templateColorField);
		
		buildButton = new Button();
        buildButton.text = "Build Template";
		buildButton.clicked += OnCreateTemplate;
        root.Add(buildButton);

        // Import UXML
		/*
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/SceneArtTemplateBuilder.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);
		*/

		/*
        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/SceneArtTemplateBuilder.uss");
        VisualElement labelWithStyle = new Label("Hello World! With Style");
        labelWithStyle.styleSheets.Add(styleSheet);
        root.Add(labelWithStyle);
		*/
    }
	
	private void OnCreateTemplate(){
		SceneImport.Scene scene = SceneImport.SceneManager.GetActiveScene();
        Debug.Log("Active Scene is '" + scene.name + "'.");
		
		GameObject[] blocks = GameObject.FindGameObjectsWithTag("PhysicsBlock");
		CalibrateWorldSpaceBounds(blocks);
		
		pixelsPerUnit = int.Parse(inputPixelsPerUnitField.value);
		int width = (int)(maxXWorldPos - minXWorldPos) * pixelsPerUnit;
        int height = (int)(maxYWorldPos - minYWorldPos) * pixelsPerUnit;
		
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
		int mipCount = Mathf.Min(3, texture.mipmapCount);
        for (int mip = 0; mip < mipCount; ++mip)
        {
            Color[] cols = texture.GetPixels(mip);
            for (int i = 0; i < cols.Length; ++i)
            {
                cols[i] = Color.clear;
            }
            texture.SetPixels(cols, mip);
        }
		
		foreach(GameObject parentObj in blocks){
			Transform parentTrans = parentObj.transform;
			for(int i = 0; i < parentTrans.childCount; i++){
				Transform currentTrans = parentTrans.GetChild(i);
				GameObject currentObj = currentTrans.gameObject;
				if(currentObj.TryGetComponent<BoxCollider2D>(out BoxCollider2D box) && currentTrans.rotation == Quaternion.identity){
					float rectangleX = currentTrans.position.x;
					float rectangleY = currentTrans.position.y;
					float rectangleWidth = currentTrans.localScale.x;
					float rectangleHeight = currentTrans.localScale.y;
					DrawRectangle(texture, rectangleX, rectangleY, rectangleWidth, rectangleHeight);
				}
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
	
	private void CalibrateWorldSpaceBounds(GameObject[] objs){
		minXWorldPos = float.MaxValue;
		minYWorldPos = float.MaxValue;
		maxXWorldPos = float.MinValue;
		maxYWorldPos = float.MinValue;
		
		foreach(GameObject parentObj in objs){
			Transform parentTrans = parentObj.transform;
			for(int i = 0; i < parentTrans.childCount; i++){
				Transform currentTrans = parentTrans.GetChild(i);
				GameObject currentObj = currentTrans.gameObject;
				if(currentObj.TryGetComponent<BoxCollider2D>(out BoxCollider2D box) && currentTrans.rotation == Quaternion.identity){
					Vector2 size = new Vector2(currentTrans.localScale.x, currentTrans.localScale.y);
					Vector2 offset = new Vector2(currentTrans.localPosition.x, currentTrans.localPosition.y);
					
					float leftEdge = parentTrans.position.x - (size.x / 2) + offset.x;
					float rightEdge = parentTrans.position.x + (size.x / 2) + offset.x;
					float upperEdge = parentTrans.position.y + (size.y / 2) + offset.y;
					float lowerEdge = parentTrans.position.y - (size.y / 2) + offset.y;

					if(leftEdge < minXWorldPos){
						minXWorldPos = leftEdge;
					}
					if(rightEdge > maxXWorldPos){
						maxXWorldPos = rightEdge;
					}
					if(upperEdge > maxYWorldPos){
						maxYWorldPos = upperEdge;
					}
					if(lowerEdge < minYWorldPos){
						minYWorldPos = lowerEdge;
					}
				}
			}
		}
		
		minXWorldPos -= float.Parse(inputWidthField.value);
		maxXWorldPos += float.Parse(inputWidthField.value);
		minYWorldPos -= float.Parse(inputHeightField.value);
		maxYWorldPos += float.Parse(inputHeightField.value);
	}
	
	private void DrawRectangle(Texture2D texture, float worldX, float worldY, float worldWidth, float worldHeight){
		float leftEdgePixel = ((worldX - (worldWidth / 2)) - minXWorldPos) * pixelsPerUnit;
		float rightEdgePixel = ((worldX + (worldWidth / 2)) - minXWorldPos) * pixelsPerUnit;
		float upperEdgePixel = ((worldY + (worldHeight / 2)) - minYWorldPos) * pixelsPerUnit;
		float lowerEdgePixel = ((worldY - (worldHeight / 2)) - minYWorldPos) * pixelsPerUnit;
					
		DrawLine(texture, new Vector2(leftEdgePixel, upperEdgePixel), new Vector2(rightEdgePixel, upperEdgePixel));
		DrawLine(texture, new Vector2(leftEdgePixel, lowerEdgePixel), new Vector2(rightEdgePixel, lowerEdgePixel));
		DrawLine(texture, new Vector2(leftEdgePixel, lowerEdgePixel), new Vector2(leftEdgePixel, upperEdgePixel));
		DrawLine(texture, new Vector2(rightEdgePixel, lowerEdgePixel), new Vector2(rightEdgePixel, upperEdgePixel));
	}
	
	private void DrawLine(Texture2D texture, Vector2 startPoint, Vector2 endPoint){
		bool horizontalLine = Mathf.Abs(endPoint.x - startPoint.x) > Mathf.Abs(endPoint.y - startPoint.y);
		Color lineColor = templateColorField.value;
		if(horizontalLine){
			int verticalPixel = (int)startPoint.y;
			for(int horizontalPixel = (int)startPoint.x; horizontalPixel <= (int)endPoint.x; horizontalPixel++){
				// TODO ADD CALCULATION FOR UNEVEN LINES
				
				texture.SetPixel(horizontalPixel, verticalPixel, lineColor);
			}
		} else{
			int horizontalPixel = (int)startPoint.x;
			for(int verticalPixel = (int)startPoint.y; verticalPixel <= (int)endPoint.y; verticalPixel++){
				// TODO ADD CALCULATION FOR UNEVEN LINES
				
				texture.SetPixel(horizontalPixel, verticalPixel, lineColor);
			}
		}
	}
}