 
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ScrollListWindow : EditorWindow
{
    private Vector2 scrollPosition = Vector2.zero;
    private List<string> items = new List<string>();

    [MenuItem("Tools/Scroll List Window")]
    public static void ShowWindow()
    {
        GetWindow<ScrollListWindow>("Scroll List Window");
    }

    private void OnGUI()
    {
        GUILayout.Label("Scroll List Window", EditorStyles.boldLabel);

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        foreach (string item in items)
        {
            GUILayout.Label(item);
        }

        GUILayout.EndScrollView();

        if (GUILayout.Button("Add Item"))
        {
            items.Add("New Item");
        }
    }
}

