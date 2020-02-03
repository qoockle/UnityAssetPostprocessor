using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FBXImportProcessor : AssetPostprocessor
{
    void OnPreprocessModel()
    {
        // FBXImportUtility.Validate(assets);

        ModelImporter modelImporter = assetImporter as ModelImporter;
        AssetImporter importer = assetImporter as AssetImporter;
        if (FBXImportSettings.Instance.ValidateOnImport)
        {
            Debug.Log("ValidateOnImport == TRUE");

            modelImporter.animationCompression = ModelImporterAnimationCompression.Optimal;
            modelImporter.animationPositionError = FBXImportSettings.Instance.ResampleCurveErrors;
            modelImporter.animationRotationError = FBXImportSettings.Instance.ResampleCurveErrors;
            modelImporter.animationScaleError = FBXImportSettings.Instance.ResampleCurveErrors;
            modelImporter.globalScale = FBXImportSettings.Instance.ImportScale;
            //FBXImportUtility.Validate(_assets);
        }
        else
        {
            Debug.Log("ValidateOnImport == FALSE");
        }
        if (FBXImportSettings.Instance.DeleteFBXAfterValidate)
        {
            //todo delete fbx after validation
        }
        // ModelImporterAnimationCompression.Optimal;  
        // importer.materialImportMode = ModelImporterMaterialImportMode.None;
    }
   /* public void OnPreprocessAnimation(GameObject gameobject)
    {
        ModelImporter modelImporter = assetImporter as ModelImporter;
        ModelImporterClipAnimation[] clipAnimations = modelImporter.defaultClipAnimations;

       // modelImporter.generateAnimations = UnityEditor.ModelImporterGenerateAnimations.GenerateAnimations;

        //Modify/Rename animation clips?

        for (int i = 0; i < clipAnimations.Length; i++)
        {
            clipAnimations[i].name = gameobject.name;
            foreach (string loopSufix in FBXImportSettings.Instance.LoopSufix)
            {
                if (gameobject.name.Contains(loopSufix))
                {
                    Debug.Log(clipAnimations[i].name);
                    //clipAnimations[i].loopTime = true;
                }
            }
        }
        modelImporter.clipAnimations = clipAnimations;
    }*/
} 