using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


public static class FBXImportUtility
{
    public static void Validate(GameObject[] assets, bool interactable = false)
    {
        if (interactable)
            Debug.Log("Validation started");

        var settings = FBXImportSettings.Instance;
        var importScale = settings.ImportScale;
        var ignorePrefix = settings.IgnorePrefix;
        // var validateOnImport = settings.ValidateOnImport;
        // var deleteFBXAfterValidate = settings.DeleteFBXAfterValidate;
        foreach (GameObject asset in assets)
        {
            if (ignorePrefix.Any(asset.ToString().StartsWith))
            {
                Debug.LogError(asset + "INGORED");
                continue;
            }
            else
            {
                Debug.Log(asset + "Validated");
            }


        }
        Debug.Log("Validation done");
    }
}