using UnityEditor;
using UnityEngine;

public class FBXImporterEditorWindow : EditorWindow
{
    public FBXImportSettings _settings;
    float scale = 1f;
    public bool validateOnImport;
    bool deleteAfterReimport;


    [MenuItem("Window/FBX Importer/Importer")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<FBXImporterEditorWindow>("FBX Importer");
    }

    void OnGUI() 
    {
        GUILayout.Label("FBX Import Settings");

        scale = EditorGUILayout.FloatField("Import Scale", _settings.ImportScale);
        validateOnImport = EditorGUILayout.Toggle("Validate On Import", _settings.ValidateOnImport);

        if (_settings.DeleteFBXAfterValidate)
        {
            deleteAfterReimport = EditorGUILayout.Toggle("Reimport and delete", _settings.DeleteFBXAfterValidate);
            if (GUILayout.Button("Reimport and delete"))
            {
                SelectedGameObjects();
            }
        }
        else
        {
            deleteAfterReimport = EditorGUILayout.Toggle("Reimport", _settings.DeleteFBXAfterValidate);
            if (GUILayout.Button("Reimport"))
            {
                SelectedGameObjects();
            }
        }

    }
    void SelectedGameObjects()
    {
        foreach (GameObject gameObject in Selection.gameObjects)
        {
            Debug.Log(gameObject.name);
            AssetDatabase.Refresh();

            if (deleteAfterReimport)
            {
                Debug.Log(gameObject.name + " DELETED");
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(gameObject));
            }
        }
    }
}
