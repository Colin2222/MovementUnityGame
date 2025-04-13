using UnityEditor;
using UnityEngine;
using SceneImport=UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.IO;
using System.Collections.Generic;
using System;

public class TreeSpritesheetGenerator : EditorWindow
{
    const int NUM_KEYFRAMES = 12;
    const int NUM_FRAMESTATES = 4;
    const int NUM_FRAMETYPES = 4;
    const int NUM_SIZES = 4;
    const int ATLAS_RIGHT_VERT = 0;
    const int ATLAS_LEFT_VERT = 0;
    const int ATLAS_X_OFFSET = 0;
    const int ATLAS_Y_OFFSET = 32;
    const int ATLAS_X_INTERVAL = 40;
    const int ATLAS_Y_INTERVAL = 32;
    const int ATLAS_PROBE_WIDTH = 32;
    const int ATLAS_PROBE_HEIGHT = 15;
    const int ATLAS_HEIGHT = 799;


    private TextField outputField;
    private TextField inputAtlasField;
    private TextField inputBaseField;
    private Button buildButton;
    private ColorField inputSize0Color;
    private ColorField inputSize1Color;
    private ColorField inputSize2Color;
    private ColorField inputSize3Color;
    private ColorField inputSizeF0Color;
    private ColorField inputSizeF1Color;

    [MenuItem("Automation/TreeSpritesheetGenerator")]
    public static void ShowExample()
    {
        TreeSpritesheetGenerator wnd = GetWindow<TreeSpritesheetGenerator>();
        wnd.titleContent = new GUIContent("TreeSpritesheetGenerator");
		
		wnd.position = new Rect(wnd.position.x, wnd.position.y, 800, 400);
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label();
        root.Add(label);

        inputAtlasField = new TextField();
		inputAtlasField.label = "Treebranch atlas image (include extension)";
		inputAtlasField.value = "Assets\\Art\\AutomationReference\\treebranch_atlas_highlights.png";
		root.Add(inputAtlasField);

        inputBaseField = new TextField();
		inputBaseField.label = "Tree base image (include extension)";
		inputBaseField.value = "Assets\\Art\\AutomationReference\\tree_bases\\tree_base_1.png";
		root.Add(inputBaseField);
		
		outputField = new TextField();
		outputField.label = "Output file path (include extension)";
		outputField.value = "Assets\\Art\\sp_art\\environment_stuff\\trees\\tree_test.png";
		root.Add(outputField);

        inputSize0Color = new ColorField();
		inputSize0Color.label = "Size 1 color";
		inputSize0Color.value = new Color(1.0f, 0.0f, 0.0f, 1.0f);
		root.Add(inputSize0Color);

        inputSize1Color = new ColorField();
        inputSize1Color.label = "Size 2 color";
        inputSize1Color.value = new Color(0.0f, 1.0f, 0.0f, 1.0f);
        root.Add(inputSize1Color);

        inputSize2Color = new ColorField();
        inputSize2Color.label = "Size 3 color";
        inputSize2Color.value = new Color(0.0f, 0.0f, 1.0f, 1.0f);
        root.Add(inputSize2Color);

        inputSize3Color = new ColorField();
        inputSize3Color.label = "Size 4 color";
        inputSize3Color.value = new Color(1.0f, 1.0f, 0.0f, 1.0f);
        root.Add(inputSize3Color);

        inputSizeF0Color = new ColorField();
        inputSizeF0Color.label = "Size F1 color";
        inputSizeF0Color.value = new Color(1.0f, 0.0f, 1.0f, 1.0f);
        root.Add(inputSizeF0Color);

        inputSizeF1Color = new ColorField();
        inputSizeF1Color.label = "Size F2 color";
        inputSizeF1Color.value = new Color(0.0f, 1.0f, 1.0f, 1.0f);
        root.Add(inputSizeF1Color);
		
		buildButton = new Button();
        buildButton.text = "Generate Tree Spritesheet";
		buildButton.clicked += OnGenerateTreeSpritesheet;
        root.Add(buildButton);
    }
    
    private void OnGenerateTreeSpritesheet(){
        // size color dictionary
        Color size0Color = inputSize0Color.value;
        Color size1Color = inputSize1Color.value;
        Color size2Color = inputSize2Color.value;
        Color size3Color = inputSize3Color.value;
        Color sizeF0Color = inputSizeF0Color.value;
        Color sizeF1Color = inputSizeF1Color.value;
        Color size0ColorTransparent = new Color(size0Color.r, size0Color.g, size0Color.b, 0.4f);
        Color size1ColorTransparent = new Color(size1Color.r, size1Color.g, size1Color.b, 0.4f);
        Color size2ColorTransparent = new Color(size2Color.r, size2Color.g, size2Color.b, 0.4f);
        Color size3ColorTransparent = new Color(size3Color.r, size3Color.g, size3Color.b, 0.4f);
        Color sizeF0ColorTransparent = new Color(sizeF0Color.r, sizeF0Color.g, sizeF0Color.b, 0.4f);
        Color sizeF1ColorTransparent = new Color(sizeF1Color.r, sizeF1Color.g, sizeF1Color.b, 0.4f);
        
        // Load the tree base texture
        string basePath = inputBaseField.value;
        Texture2D baseTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(basePath);

        // scan base to get branch data
        List<BranchData> branchDataList = new List<BranchData>();
        for (int y = 0; y < baseTexture.height; y++){
            for (int x = 0; x < baseTexture.width; x++){
                Color pixelColor = baseTexture.GetPixel(x, y);
                if (pixelColor.a > 0.0f){
                    int sizeFound = -1;
                    if(pixelColor == size0Color || pixelColor == size0ColorTransparent){
                        sizeFound = 0;
                    } else if(pixelColor == size1Color || pixelColor == size1ColorTransparent){
                        sizeFound = 1;
                    } else if(pixelColor == size2Color || pixelColor == size2ColorTransparent){
                        sizeFound = 2;
                    } else if(pixelColor == size3Color || pixelColor == size3ColorTransparent){
                        sizeFound = 3;
                    } else if(pixelColor == sizeF0Color || pixelColor == sizeF0ColorTransparent){
                        sizeFound = 4;
                    } else if(pixelColor == sizeF1Color || pixelColor == sizeF1ColorTransparent){
                        sizeFound = 5;
                    }

                    if(sizeFound != -1){
                        BranchData branchData = new BranchData();
                        branchData.xPos = x;
                        branchData.yPos = y;
                        branchData.direction = pixelColor.a < 1.0f ? -1 : 1;
                        branchData.currentFrame = UnityEngine.Random.Range(0, NUM_KEYFRAMES / NUM_FRAMESTATES);
                        if(branchData.direction == -1){
                            branchData.currentFrame = (branchData.currentFrame + (NUM_KEYFRAMES / 2)) % NUM_KEYFRAMES;
                        }
                        branchData.sizeType = UnityEngine.Random.Range(0, NUM_FRAMETYPES);
                        branchData.size = sizeFound;
                        branchDataList.Add(branchData);
                    }
                }
            }
        }
        ShuffleList(branchDataList);

        // Load the atlas texture
        string atlasPath = inputAtlasField.value;
        Texture2D atlasTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(atlasPath);

        // Create the output texture
        int keyframeWidth = baseTexture.width;
        int keyframeHeight = baseTexture.height;
        Texture2D outputTexture = new Texture2D(keyframeWidth * NUM_KEYFRAMES, keyframeHeight, TextureFormat.RGBA32, false);
        // transparent fill
        Color[] transparentPixels = new Color[outputTexture.width * outputTexture.height];
        for (int i = 0; i < transparentPixels.Length; i++)
        {
            transparentPixels[i] = Color.clear;
        }
        outputTexture.SetPixels(transparentPixels);

        // copy base texture to output texture
        for(int i = 0; i < NUM_KEYFRAMES; i++){
            for(int baseX = 0; baseX < baseTexture.width; baseX++){
                for(int baseY = 0; baseY < baseTexture.height; baseY++){
                    Color pixelColor = baseTexture.GetPixel(baseX, baseY);
                    if(pixelColor.a > 0.0f){
                        outputTexture.SetPixel(baseX + (i * keyframeWidth), baseY, pixelColor);
                    }
                }
            }
        }

        // fill in each branch
        foreach(BranchData branchData in branchDataList){
            for(int i = 0; i < NUM_KEYFRAMES; i++){
                int currentFrame = branchData.currentFrame;
                int frameState = currentFrame / (NUM_KEYFRAMES / NUM_FRAMESTATES);
                int branchAnchorX = branchData.xPos + (i * keyframeWidth);
                int branchAnchorY = branchData.yPos;
                int atlasAnchorX = ATLAS_X_OFFSET + (frameState * ATLAS_X_INTERVAL);
                int atlasAnchorY = ATLAS_HEIGHT - (ATLAS_Y_OFFSET + (((branchData.size * NUM_FRAMETYPES) + branchData.sizeType) * ATLAS_Y_INTERVAL));

                for(int x = 0; x < ATLAS_PROBE_WIDTH; x++){
                    for(int y = 0; y < ATLAS_PROBE_HEIGHT; y++){
                        Color pixelColor = atlasTexture.GetPixel(atlasAnchorX + x, (atlasAnchorY - (ATLAS_PROBE_HEIGHT / 2)) + y);
                        if(pixelColor.a > 0.0f){
                            outputTexture.SetPixel(branchAnchorX + (x * branchData.direction), (branchAnchorY - (ATLAS_PROBE_HEIGHT / 2)) + y, pixelColor);
                        }
                    }
                }

                if(currentFrame < NUM_KEYFRAMES - 1){
                    branchData.currentFrame++;
                } else{
                    branchData.currentFrame = 0;
                }
            }


        }

        outputTexture.Apply();
        byte[] bytes = outputTexture.EncodeToPNG();
        string path = outputField.value;
		if(path.Length >= 5 && path.Substring(path.Length - 4) == ".png"){
			File.WriteAllBytes(path, bytes);
			AssetDatabase.Refresh();
			Debug.Log("PNG saved to: " + path);
		} else{
			Debug.Log("Did not save due to incomplete output filepath");
		}
    }

    void ShuffleList<T>(List<T> list) {
        System.Random random = new System.Random();
        int n = list.Count;

        // Start from the end and swap elements with a random one
        for (int i = n - 1; i > 0; i--) {
            int j = random.Next(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}

class BranchData{
    public int xPos;
    public int yPos;
    public int currentFrame;
    public int size;
    public int sizeType;
    public int direction;
}
