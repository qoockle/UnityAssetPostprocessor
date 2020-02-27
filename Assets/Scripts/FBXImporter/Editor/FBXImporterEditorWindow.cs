using System;
using UnityEditor;
using UnityEngine;
using ImporterManager;
using Object = UnityEngine.Object;

public class FBXImporterEditorWindow : EditorWindow
{
    private static bool deleteFBXAfterExtracting;
    private static float resampleCurveErrors;
    private float scaleFbx;
    private static string loop;
    public ImporterSettings settings;
    private static FBXImporterEditorWindow editor;
    private static int width = 350;
    private static int height = 300;
    private static int x = 0;
    private static int y = 0;

    [MenuItem("Tools/Import Managers/FBX Importer")]
    private static void ShowEditor()
    {
        
        editor = EditorWindow.GetWindow<FBXImporterEditorWindow>();
        CenterWindow();
    }

    private void OnEnable()
    {
        LoadSettings();
    }
    
    private void OnGUI()
    {
        GUILayout.Label("FBX Importer");
        EditorGUILayout.Toggle("Delete FBX after Extracting", deleteFBXAfterExtracting);
        resampleCurveErrors = EditorGUILayout.FloatField("Rules for resample curves", resampleCurveErrors);
        loop = EditorGUILayout.TextField("Loop Settings", loop);
        scaleFbx = EditorGUILayout.FloatField("ScaleFBX", scaleFbx);
        /*
        if (GUILayout.Button("Apply model import settings"))
        {
            if (FBXImporterManager.files != null)
            {
                FBXImporterManager.files.Clear();
            }
            //FBXImporterManager.ModelImportSettings();
            Debug.Log("Not yet implemented");
        }
        */
        if (GUILayout.Button("Rename Animation Clips"))
        {
            if (FBXImporterManager.files != null)
            {
                FBXImporterManager.files.Clear();
            }
            FBXImporterManager.RenameAnimationClip();
        }

        if (GUILayout.Button("Extract Animation Clips"))
        {
            foreach (Object _object in Selection.objects)
            {
                if (AssetDatabase.GetAssetPath(_object).EndsWith(".FBX") || AssetDatabase.GetAssetPath(_object).EndsWith(".fbx"))
                {
                    FBXImporterManager.ExtractAnimationClip(_object, deleteFBXAfterExtracting);
                } 
            }
        }
        
        if (GUILayout.Button("Save config"))
        {
             SaveSettings();//todo add save config
        }
        
        if (GUILayout.Button("Load config"))
        {
            LoadSettings();
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
    private void LoadAttributes(ImporterSettings settings)
    {  
        //FBXImportSettings _settings = ScriptableObject.CreateInstance<FBXImportSettings>();
        //_settings = FindObjectOfType <FBXImportSettings>();
        //Debug.Log("Settings asset " + _settings + " loaded");
        resampleCurveErrors = FBXImporterManager.resampleCurveErrors;
        loop = FBXImporterManager.loop;
        deleteFBXAfterExtracting = FBXImporterManager.deleteFBXAfterExtracting;
        scaleFbx = settings.someVar;
        
        Debug.Log("LoadAttributes()");
    }

    private void LoadSettings()
    {
        Debug.Log("LoadSettings()");
        settings = (ImporterSettings)AssetDatabase.LoadAssetAtPath("Assets/Scripts/FBXImporter/Editor/SettingsAsset.asset", typeof(ImporterSettings));
        if (settings != null)
            LoadAttributes(settings);
        else
            Debug.LogError("settings == null");
    }

    private void SaveSettings()
    {
        settings.someVar = scaleFbx;
        // Debug.Log("Note yet implemented");
    }
}