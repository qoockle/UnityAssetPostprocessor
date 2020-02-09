using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "FBXImportSettingsAsset")]
public class FBXImportSettings : ScriptableObject
{
    [Tooltip("Should run validations when an asset is imported")]
    public bool ValidateOnImport = true;

    [Tooltip("Should delete FBX file when an asset will be validated")]
    public bool DeleteFBXAfterValidate = true;

    [Tooltip("Scale rules for reimport")]
    public float ImportScale = 1f;

    [Tooltip("Scale rules for resample curves errors")]
    public float ResampleCurveErrors = 0.9f;

    [Tooltip("Sufixes will be looped by importing")]
    public string[] LoopSufix = new string[] { "Loop", "loop" };

    [Tooltip("Prefixes will be ignored by importing")]
    public string[] IgnorePrefix = new string[] { "dont_", "Dont_" };

    static FBXImportSettings _instance;

    private static FBXImportSettings asset;

    public static FBXImportSettings Instance => _instance ?? (_instance = LoadAsset());

    private static FBXImportSettings LoadAsset()
    {
        foreach (GameObject obj in Selection.objects)
        {
            var path = AssetDatabase.GetAssetPath(obj);
            {
                asset = AssetDatabase.LoadAssetAtPath<FBXImportSettings>(path);
                if (asset == null)
                {

                    asset = CreateInstance<FBXImportSettings>();
                    Debug.Log(asset + "LOADED");
                    //  AssetDatabase.CreateAsset(asset, path);
                    //  AssetDatabase.SaveAssets();
                }
            }
        }
       // Debug.Log("LoadAsset(" + asset + ")");
        return asset;
    }
}