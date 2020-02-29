using UnityEditor;
using UnityEngine;
using ImporterManager;
using Object = UnityEngine.Object;

public class FBXImporterEditorWindow : EditorWindow
{
    private ImporterSettings settings;
    private static FBXImporterEditorWindow editor;
    
    private bool deleteFBXAfterExtracting;
    private float resampleCurveErrors;
    private string loop;
    private ModelImporterAnimationCompression animationCompression;

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
        GUILayout.Space(25f);
        deleteFBXAfterExtracting = EditorGUILayout.Toggle("Delete FBX after Extracting", deleteFBXAfterExtracting);
        resampleCurveErrors = EditorGUILayout.FloatField("Rules for resample curves", resampleCurveErrors);
        loop = EditorGUILayout.TextField("Loop Settings", loop);
        animationCompression = (ModelImporterAnimationCompression)EditorGUILayout.EnumPopup("Animation Compression", animationCompression);

        /*
        if (GUILayout.Button("Apply model import settings"))
        {
            if (FBXImporterManager.files != null)
            {
                FBXImporterManager.files.Clear();
            }
            FBXImporterManager.ApplyModelImportSettings();
           // Debug.Log("Not yet implemented");
        }
        */
        if (GUILayout.Button("Rename Animation Clips"))
        {
            if (FBXImporterManager.files != null)
            {
                FBXImporterManager.files.Clear();
            }
            CastAttributes();
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
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save config"))
        {
            SaveSettings();
        }
        GUILayout.Space(50f);
        if (GUILayout.Button("Load config"))
        {
            LoadSettings();
        }
        GUILayout.EndHorizontal();
    }
    
    private static void CenterWindow()
    {
        editor = EditorWindow.GetWindow<FBXImporterEditorWindow>();
        x = (Screen.currentResolution.width - width) / 2;
        y = (Screen.currentResolution.height - height) / 2;
        editor.position = new Rect(x, y, width, height);
        editor.maxSize = new Vector2(width, height);
    }
    
    private void LoadAttributes(ImporterSettings settings)
    {
        deleteFBXAfterExtracting = settings.deleteFBXAfterExtracting;
        loop = settings.stringLoopSufix;
        resampleCurveErrors = settings.resampleCurveErrors;
        animationCompression = settings.animationCompression;

        CastAttributes();
        Debug.Log("FBX Import Settings Loaded");
    }

    private void CastAttributes()
    {
        FBXImporterManager.loop = loop;
        FBXImporterManager.resampleCurveErrors = resampleCurveErrors;
        FBXImporterManager.animationCompression = animationCompression;
    }

    private void LoadSettings()
    {
        settings = (ImporterSettings)AssetDatabase.LoadAssetAtPath("Assets/Scripts/FBXImporter/Editor/FBXImporterSettingsAsset.asset", typeof(ImporterSettings));
        if (settings != null)
            LoadAttributes(settings);
        else
            Debug.LogError("FBX Import Settings Asset not Founded");
    }

    private void SaveSettings()
    {
        settings.resampleCurveErrors = resampleCurveErrors;
        settings.deleteFBXAfterExtracting = deleteFBXAfterExtracting;
        settings.stringLoopSufix = loop;
        settings.animationCompression = animationCompression;
        Debug.Log("FBX Import Settings Saved");
    }
}