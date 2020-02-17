using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ImporterManager
{
    public class FBXImporterManager
    {
        public static bool deleteFBXAfterExtracting = true;
        public static float resampleCurveErrors = 0.9f;
        public static string loop =  "loop";
        
        public static string _targetExtension = ".anim";
        private static List<string> files = new List<string>();
        
        public static void Rename()
        {
            FileSearch(); //validate files
            if (files != null)
            {
                //Debug.Log("Rename >" + files.Count.ToString() + "< FBX files");
                foreach (string file in files)
                {
                     int idx = file.IndexOf("Assets"); 
                     string asset = file.Substring(idx);
                     var fileName = Path.GetFileNameWithoutExtension(file);
                     var importer = (ModelImporter)AssetImporter.GetAtPath(asset);
                     
                     RenameAndImport(importer, fileName);
                }
            }
        }

        public static void RenameAndImport(ModelImporter asset, string name)
        {
            ModelImporter modelImporter = asset as ModelImporter;
            ModelImporterClipAnimation[] clipAnimations = modelImporter.defaultClipAnimations;
            AssetImporter assetImporter = asset as AssetImporter;
            
            modelImporter.animationCompression = ModelImporterAnimationCompression.Optimal;
            modelImporter.animationPositionError = resampleCurveErrors;
            modelImporter.animationRotationError = resampleCurveErrors;
            modelImporter.animationScaleError = resampleCurveErrors;

            for (int i = 0; i < clipAnimations.Length; i++)
            {
                //Debug.Log("ClipAnimation (" + clipAnimations[i].name + ") RENAMED");
                clipAnimations[i].name = name;
                if (clipAnimations[i].name.Contains(loop))
                {
                    clipAnimations[i].loopTime = true;
                }
            }

            modelImporter.clipAnimations = clipAnimations;
            modelImporter.SaveAndReimport();
            //Debug.Log("Rename done");
        }

        static void FileSearch()
        {
            if (Selection.gameObjects != null)
            {
                foreach (GameObject go in Selection.gameObjects)
                {
                    if (AssetDatabase.GetAssetPath(go).EndsWith(".FBX") || AssetDatabase.GetAssetPath(go).EndsWith(".fbx"))
                    {
                        files.Add(AssetDatabase.GetAssetPath(go));
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
        public static void ExtractAnimationClips(Object selectedObject, bool delete)
        {
            string selectedObjectPath = AssetDatabase.GetAssetPath(selectedObject);
            string parentfolderPath = selectedObjectPath.Substring(0, selectedObjectPath.Length - (selectedObject.name.Length + 5));
            
            //Create AnimationClips
            Object[] objects = AssetDatabase.LoadAllAssetsAtPath(selectedObjectPath);
            foreach (Object _object in objects)
            {
                if (_object is AnimationClip && !_object.name.Contains("__preview__"))
                {
                    AnimationClip clip = Object.Instantiate(_object) as AnimationClip;
                    AssetDatabase.CreateAsset(clip, parentfolderPath + "/"  + _object.name + _targetExtension);
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            DeleteSelectedAsset(selectedObject, delete);
        }

        public static void DeleteSelectedAsset(Object selectedObject, bool delete)
        {
            if (delete)
            {
                Debug.LogWarning(selectedObject + " DELETED");
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(selectedObject));
            }
        }
    }
}