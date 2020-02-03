using UnityEngine;
using UnityEditor;

public class FBXAttributesGUI
{
    [MenuItem("Window/FBX Importer/FindAssets")]

    static void ExampleScript()
    {
        string[] guids2 = AssetDatabase.FindAssets("t:Mesh");

        foreach (string guid2 in guids2)
        {
            Debug.Log(AssetDatabase.GUIDToAssetPath(guid2));
        }
    }
}