using UnityEngine;

[CreateAssetMenu(fileName = "FBXImportSettingsAsset")]
public class FBXImporterSettings : ScriptableObject
{
    [Tooltip("Should delete FBX file when an asset will be validated")]
    public static bool DeleteFBXAfterExtracting = true;

    [Tooltip("Scale rules for resample curves errors")]
    public static float ResampleCurveErrors = 0.9f;

    [Tooltip("Sufixes will be looped by importing")]
    public static string[] LoopSufix = new string[] { "loop" };

    [Tooltip("Prefixes will be ignored by importing")]
    public static string[] IgnorePrefix = new string[] { "dont_"};
}