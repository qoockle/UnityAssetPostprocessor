using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "FBXImporterSettingsAsset")]
public class ImporterSettings : UnityEngine.ScriptableObject
{
    /*
    [Header("Model Settings")]
    public float fileScale = 1f;
    public bool importBlendShapes = false;
    public bool importVisibility = false;
    public bool importCameras = false;
    public bool importLights = false;
    public bool preserveHierarchy = false;
    public ModelImporterMeshCompression meshCompression = ModelImporterMeshCompression.High;
    public bool readWriteEnabled = false;
    public bool optimizeMesh = false;
    public bool generateColliders = false;
    public bool keepQuads = false;
    public bool weldVertices = true;
    public ModelImporterIndexFormat indexFormat = ModelImporterIndexFormat.UInt16;
    public ModelImporterNormals normals = ModelImporterNormals.None;
    public bool swapUvs = false;
    public bool generateLightmapUvs = false;
    */
    
    [Header("Animation Settings")]
    [Tooltip("Detele FBX asset after animation clip extract")]
    public bool deleteFBXAfterExtracting = false;
    
    [Tooltip("Scale rules for resample curves errors")]
    public float resampleCurveErrors = 0.9f;
    
    [Tooltip("Sufix that will be looped by importing")]
    public string stringLoopSufix = "loop";
    
    [Tooltip("Animation compression properties")]
    public ModelImporterAnimationCompression animationCompression = ModelImporterAnimationCompression.Optimal;
    
}
