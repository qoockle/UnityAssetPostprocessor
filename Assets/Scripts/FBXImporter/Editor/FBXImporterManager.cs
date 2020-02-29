using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ImporterManager
{
    public class FBXImporterManager
    {
        public static float resampleCurveErrors;
        public static string loop;
        public static ModelImporterAnimationCompression animationCompression;
        /*
        private static float fileScale = 1f;
        private static bool importBlendShapes = false;
        private static bool importVisibility = false;
        private static bool importCameras = false;
        private static bool importLights = false;
        private static bool preserveHierarchy = false;
        private static ModelImporterMeshCompression meshCompression = ModelImporterMeshCompression.High;
        private static bool readWriteEnabled = false;
        private static bool optimizeMesh = false;
        private static bool generateColliders = false;
        private static bool keepQuads = false;
        private static bool weldVertices = true;
        private static ModelImporterIndexFormat indexFormat = ModelImporterIndexFormat.UInt16;
        private static ModelImporterNormals normals = ModelImporterNormals.None;
        private static bool swapUvs = false;
        private static bool generateLightmapUvs = false;
        */
        private static string _targetExtension = ".anim";
        
        public static List<string> files = new List<string>();

        public static void RenameAnimationClip()
        {
            FileSearch(); //validate files
            if (files != null)
            {
                //Debug.Log("Rename >" + files.Count.ToString() + "< FBX files");
                foreach (string file in files)
                {
                     int idx = file.IndexOf("Assets"); 
                     string asset = file.Substring(idx);
                     string fileName = Path.GetFileNameWithoutExtension(file);
                     ModelImporter importer = (ModelImporter)AssetImporter.GetAtPath(asset);
                     
                     RenameAndImportAnimationClip(importer, fileName);
                }
            }
        }

        private static void RenameAndImportAnimationClip(ModelImporter asset, string name)
        {
            ModelImporter modelImporter = asset as ModelImporter;
            ModelImporterClipAnimation[] clipAnimations = modelImporter.defaultClipAnimations;

            modelImporter.animationCompression = animationCompression;
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

        private static void FileSearch()
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
        public static void ExtractAnimationClip(Object selectedObject, bool delete)
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

        private static void DeleteSelectedAsset(Object selectedObject, bool delete)
        {
            if (delete)
            {
                Debug.LogWarning(selectedObject + " DELETED");
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(selectedObject));
            }
        }
        /*
        private static void ModelImportSettings(ModelImporter asset)
        {
            ModelImporter modelImporter = asset as ModelImporter;
            modelImporter.globalScale = fileScale;
            modelImporter.importBlendShapes = importBlendShapes;
            modelImporter.importVisibility = importVisibility;
            modelImporter.importCameras = importCameras;
            modelImporter.importLights = importLights;
            modelImporter.preserveHierarchy = preserveHierarchy;
            modelImporter.meshCompression = meshCompression;
            modelImporter.isReadable = readWriteEnabled;
            modelImporter.optimizeMesh = optimizeMesh; //todo refactor
            modelImporter.addCollider = generateColliders;
            modelImporter.keepQuads = keepQuads;
            modelImporter.weldVertices = weldVertices;
            modelImporter.meshCompression = meshCompression;
            modelImporter.indexFormat = indexFormat;
            modelImporter.importNormals = normals;
            modelImporter.swapUVChannels = swapUvs;
            modelImporter.generateSecondaryUV = generateLightmapUvs;
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        public static void RigImportSettings(ModelImporter asset)
        {
            //todo add rig import settings
        }
        */
    }
}