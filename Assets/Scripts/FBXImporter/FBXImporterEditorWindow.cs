using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ImporterManager;

public class FBXImporterEditorWindow : EditorWindow
{
    private static bool deleteFBXAfterExtracting;
    private static float resampleCurveErrors;
    private static string loop;

    private static FBXImporterEditorWindow editor;
    private static int width = 350;
    private static int height = 300;
    private static int x = 0;
    private static int y = 0;
    private static List<string> files = new List<string>();

    [MenuItem("Window/Import Managers/FBX Importer")]
    private static void ShowEditor()
    {
        LoadAttributes();
        editor = EditorWindow.GetWindow<FBXImporterEditorWindow>();
        CenterWindow();
    }

    private void OnGUI()
    {
        GUILayout.Label("FBX Importer");
        
        EditorGUILayout.Toggle("Delete FBX after Extracting", deleteFBXAfterExtracting);
        resampleCurveErrors = EditorGUILayout.FloatField("Rules for resample curves", resampleCurveErrors);
        loop = EditorGUILayout.TextField("Loop Settings", loop);

        if (GUILayout.Button("Rename Animation Clips"))
        {
            if (files != null)
            {
                files.Clear();
            }
            FBXImporterManager.Rename();
        }

        if (GUILayout.Button("Extract Animation Clips"))
        {
            foreach (Object _object in Selection.objects)
            {
                if (AssetDatabase.GetAssetPath(_object).EndsWith(".FBX") || AssetDatabase.GetAssetPath(_object).EndsWith(".fbx"))
                {
                    FBXImporterManager.ExtractAnimationClips(_object, deleteFBXAfterExtracting);
                } 
            }
        }
        
        if (GUILayout.Button("Save config"))
        {
            SaveAttributes();//todo add save config
        }
    }

    private static void CenterWindow()
    {
        editor = EditorWindow.GetWindow<FBXImporterEditorWindow>();
        x = (Screen.currentResolution.width - width) / 2;
        y = (Screen.currentResolution.height - height) / 2;
        editor.position = new Rect(x, y, width, height);
        editor.maxSize = new Vector2(width, height);
        editor.minSize = editor.maxSize;
    }
    private static void LoadAttributes()
    {  
        //FBXImportSettings _settings = ScriptableObject.CreateInstance<FBXImportSettings>();
        //_settings = FindObjectOfType <FBXImportSettings>();
        //Debug.Log("Settings asset " + _settings + " loaded");
        resampleCurveErrors = FBXImporterManager.resampleCurveErrors;
        loop = FBXImporterManager.loop;
        deleteFBXAfterExtracting = FBXImporterManager.deleteFBXAfterExtracting;
    }
    private static void SaveAttributes()
    {
        Debug.Log("Note yet implemented");
    }
}