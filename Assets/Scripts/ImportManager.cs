﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;


public class ImportManager : EditorWindow
{
    private bool deleteAfterReimport = false;
    private static ImportManager editor;
    private static int width = 350;
    private static int height = 300;
    private static int x = 0;
    private static int y = 0;
    private static List<string> files = new List<string>();
    private GameObject[] selectedGO;

    [MenuItem("Window/FBX Importer/Import Manager")]
    static void ShowEditor()
    {
        editor = EditorWindow.GetWindow<ImportManager>();
        CenterWindow();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Rename"))
        {
            selectedGO = Selection.gameObjects;
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
            Debug.Log("Rename ()");
            for (int i = 0; i < files.Count; i++)
            {
                int idx = files[i].IndexOf("Assets");      
                //string filename = Path.GetFileName(files[i]);
                string asset = files[i].Substring(idx);
                AnimationClip orgClip = (AnimationClip)AssetDatabase.LoadAssetAtPath(asset, typeof(AnimationClip));

                var fileName = Path.GetFileNameWithoutExtension(files[i]);
                
                var importer = (ModelImporter)AssetImporter.GetAtPath(asset);

                RenameAndImport(importer, fileName);
            }
        }
    }

    private void RenameAndImport(ModelImporter asset, string name)
    {
        Debug.Log("Rename And Reimport ()");
        ModelImporter modelImporter = asset as ModelImporter;
        ModelImporterClipAnimation[] clipAnimations = modelImporter.defaultClipAnimations;

        for (int i = 0; i < clipAnimations.Length; i++)
        {
            clipAnimations[i].name = name;
        }

        modelImporter.clipAnimations = clipAnimations;
        modelImporter.SaveAndReimport();
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
                if (AssetDatabase.GetAssetPath(go).EndsWith(".fbx"))
                {
                    files.Add(AssetDatabase.GetAssetPath(go));
                    Debug.Log(AssetDatabase.GetAssetPath(go));
                }

            }
        }
            else
                Debug.LogError("NO FBX SELECTED");
    }
}