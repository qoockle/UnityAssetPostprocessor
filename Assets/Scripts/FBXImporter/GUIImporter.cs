using UnityEditor;
using UnityEngine;

public class GUIImporter : EditorWindow
{
    public FBXImportSettings _settings;
    float scale = 1f;
    bool validateOnImport;
    public string objectName;


    [MenuItem("Window/FBX Importer/Importer")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<GUIImporter>("FBX Importer");
    }

    void OnGUI() 
    {
        GUILayout.Label("FBX Import Settings");

        scale = EditorGUILayout.FloatField("Import Scale", _settings.ImportScale);
        validateOnImport = EditorGUILayout.Toggle("Validate On Import", _settings.ValidateOnImport);

        if (GUILayout.Button("Rescale"))
        {
            Debug.Log("Rescale Button Pressed");
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                AssetDatabase.Refresh();
            }

        }
    }
}
