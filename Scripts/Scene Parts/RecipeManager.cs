using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.AddressableAssets;
using Newtonsoft.Json;

public class RecipeManager : MonoBehaviour
{
    public RecipeMappingData recipeMappingData;
    string recipeMappingAddress = "Assets/Data/recipe_mapping_data.txt";

    void Awake()
    {
        LoadRecipeData();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadRecipeData()
    {
        var operation = Addressables.LoadAssetAsync<TextAsset>(recipeMappingAddress);
        TextAsset txtAsset = operation.WaitForCompletion();

        JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
        recipeMappingData = JsonConvert.DeserializeObject<RecipeMappingData>(txtAsset.text, settings);

        Addressables.Release(operation);
    }
    
    public Item GetFurnaceResult(Item input)
    {
        if(recipeMappingData.furnace_recipes.ContainsKey(input.id))
        {
            string resultItemID = recipeMappingData.furnace_recipes[input.id];
            return ItemRegistry.Instance().GetItem(resultItemID);
        }
        return null;
    }
}

public class RecipeMappingData
{
    public Dictionary<string, string> furnace_recipes;
}
