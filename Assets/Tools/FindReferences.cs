
// Unity 查找未使用资源
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class UnusedAssetsFinder : EditorWindow
{
    private Vector2 scrollPosition = Vector2.zero;
    private static List<string> unusedAssets = new List<string>();
    private static List<string> usedAssets = new List<string>();

    [MenuItem("Tools/Find Unused Assets")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(UnusedAssetsFinder));
    }

    void OnGUI()
    {
        if (GUILayout.Button("Find Unused Assets"))
        {
            FindUnusedAssets();
        }

        if (unusedAssets.Count > 0)
        {
            GUILayout.Label("Unused Assets:");
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            foreach (string asset in unusedAssets)
            {
                GUILayout.Label(asset);
            }
        }
        else
        {
            GUILayout.Label("No unused assets found.");
        }
    }

    private static void FindUnusedAssets()
    {
        unusedAssets.Clear();
        usedAssets.Clear();

        string[] allAssets = AssetDatabase.GetAllAssetPaths();

        foreach (string asset in allAssets)
        {
            if (IsAssetUsed(asset))
            {
                usedAssets.Add(asset);
            }
            else
            {
                unusedAssets.Add(asset);
            }
        }
    }

    private static bool IsAssetUsed(string asset)
    {
        string[] dependencies = AssetDatabase.GetDependencies(asset);

        foreach (string dependency in dependencies)
        {
            if (dependency != asset && !usedAssets.Contains(dependency))
            {
                return false;
            }
        }

        return true;
    }
}
