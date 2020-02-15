using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;


public class ImportManager : EditorWindow
{
    // public FBXImportSettings _settings;

    bool deleteAfterReimport = false;
    float importScale = 1f;
    float resampleCurveErrors = 0.9f;
    string loop =  "loop";
    
   

    private static ImportManager editor;
    private static int width = 350;
    private static int height = 300;
    private static int x = 0;
    private static int y = 0;
    private static List<string> files = new List<string>();
    private GameObject[] selectedGO;

    [MenuItem("Window/Import Managers/FBX Importer")]
    static void ShowEditor()
    {
        editor = EditorWindow.GetWindow<ImportManager>();
        CenterWindow();
    }

    private void OnGUI()
    {
        GUILayout.Label("FBX Importer");

        importScale = EditorGUILayout.FloatField("Import Scale", importScale);
        deleteAfterReimport = EditorGUILayout.Toggle("Delete On Import", deleteAfterReimport);
        resampleCurveErrors = EditorGUILayout.FloatField("Rules for resample curves", resampleCurveErrors);
        loop = EditorGUILayout.TextField("Loop Settings", loop);

        if (GUILayout.Button("Rename"))
        {
            if (files != null)
            {
                files.Clear();
               // Debug.LogWarning("Selected List Cleared");
            }
            
            Rename();
            
            if (deleteAfterReimport)
            {
                foreach (GameObject gameObject in selectedGO)
                {
                    Debug.LogWarning(gameObject + " DELETED");
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(gameObject));
                }
            }


        }
    }

    public void Rename()
    {
        FileSearch();

        if (files != null)
        {
            Debug.Log("Rename >" + files.Count.ToString() + "< FBX files");

            foreach (string file in files)
            {
                 //Debug.Log(files.Count.ToString());
                 int idx = file.IndexOf("Assets"); 
                 //Debug.Log(idx.ToString() + " IDX INDEX");
                 string asset = file.Substring(idx);
                 var fileName = Path.GetFileNameWithoutExtension(file);
                 //Debug.Log("FILENAME ---> (" + fileName.ToString() + ")");
                 var importer = (ModelImporter)AssetImporter.GetAtPath(asset);
                 //Debug.Log("IMPORTER ---> (" + importer.ToString() + ")");
                 RenameAndImport(importer, fileName);

            }
        }
    }

    private void RenameAndImport(ModelImporter asset, string name)
    {
        //Debug.Log("Rename And Reimport ()");
        ModelImporter modelImporter = asset as ModelImporter;
        ModelImporterClipAnimation[] clipAnimations = modelImporter.defaultClipAnimations;
        AssetImporter assetImporter = asset as AssetImporter;
        
        
        
        modelImporter.animationCompression = ModelImporterAnimationCompression.Optimal;
        modelImporter.animationPositionError = resampleCurveErrors;
        modelImporter.animationRotationError = resampleCurveErrors;
        modelImporter.animationScaleError = resampleCurveErrors;
        modelImporter.globalScale = importScale;

        for (int i = 0; i < clipAnimations.Length; i++)
        {
            //Debug.Log("ClipAnimation (" + clipAnimations[i].name + ") RENAMED");
            clipAnimations[i].name = name;
            if (clipAnimations[i].name.Contains(loop))
            {
                clipAnimations[i].loopTime = true;
            }
            
            //ExtrudeAnimationClip(clipAnimations[i]);
        }

        modelImporter.clipAnimations = clipAnimations;

        modelImporter.SaveAndReimport();
        
        Debug.Log("Reimport done");
    }

    private void ExtrudeAnimationClip(AnimationClip sourceClip) //todo duplicate animation clip from fbx file
    {
        if (sourceClip != null)
        {
            string path = AssetDatabase.GetAssetPath(sourceClip);
            path = Path.Combine(Path.GetDirectoryName(path), sourceClip.name) + ".anim";
            string newPath = AssetDatabase.GenerateUniqueAssetPath (path);
            AnimationClip newClip = new AnimationClip();
            EditorUtility.CopySerialized(sourceClip, newClip);
            AssetDatabase.CreateAsset(newClip, newPath);
        }
        else
        {
            Debug.LogError("No animation clips to extrude");
        }
   
        Debug.Log("Extrude  AnimationClips Done");
    }

    private static void CenterWindow()
    {
        editor = EditorWindow.GetWindow<ImportManager>();
        x = (Screen.currentResolution.width - width) / 2;
        y = (Screen.currentResolution.height - height) / 2;
        editor.position = new Rect(x, y, width, height);
        editor.maxSize = new Vector2(width, height);
        editor.minSize = editor.maxSize;
    }

    static void FileSearch()
    {
        if (Selection.gameObjects != null)
        {
            foreach (GameObject go in Selection.gameObjects)
            {
                //Debug.Log("Object (" + go.name + ") SELECTED");
                if (AssetDatabase.GetAssetPath(go).EndsWith(".FBX") || AssetDatabase.GetAssetPath(go).EndsWith(".fbx"))
                {
                    files.Add(AssetDatabase.GetAssetPath(go));
                   // Debug.Log(go.name + " ADDED to array");
                }
                else
                {
                    Debug.LogError("NO FBX FILES SELECTED");
                }
                
            }
        }
        else
        {
            Debug.LogError("NO FILES SELECTED");
        }
    }
}