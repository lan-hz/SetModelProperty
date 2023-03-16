//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using System.IO;
//using System.Text;

//public class ReferenceEditor : Editor
//{
//    [MenuItem("Assets/Find References In Prefabs", false, 50)]
//    private static void FindReferencesInPrefabs()
//    {
//        FindReferences("*.prefab");
//    }

//    [MenuItem("Assets/Find References In Fbx", false, 50)]
//    private static void FindReferencesInFbxs()
//    {
//        FindReferences("*.fbx");
//    }

//    [MenuItem("Assets/Find References In Prefabs", true)]
//    private static bool ValidateFindReferencesInPrefabs()
//    {
//        return Selection.activeObject != null;
//    }

//    [MenuItem("Assets/Find References In Materials", false, 50)]
//    private static void FindReferencesInMaterials()
//    {
//        FindReferences("*.mat");
//    }

//    [MenuItem("Assets/Find References In Materials", true)]
//    private static bool ValidateFindReferencesInMaterials()
//    {
//        return Selection.activeObject != null;
//    }

//    [MenuItem("Assets/Find References In Configs", false, 50)]
//    private static void FindReferencesInConfigs()
//    {
//        FindReferences("*.asset");
//    }

//    [MenuItem("Assets/Find References In Configs", true)]
//    private static bool ValidateFindReferencesInConfigs()
//    {
//        return Selection.activeObject != null;
//    }

//    [MenuItem("Assets/Find References In SpriteAtlas", false, 50)]
//    private static void FindReferencesInSpriteAtlas()
//    {
//        FindReferences("*.spriteatlas");
//    }

//    [MenuItem("Assets/Find References In SpriteAtlas", true)]
//    private static bool ValidateFindReferencesInSpriteAtlas()
//    {
//        return Selection.activeObject != null;
//    }

//    private static List<string> prefabList;
//    private static List<string> guidList;
//    private static List<string> pathList;
//    private static Dictionary<string, List<string>> referDict = new Dictionary<string, List<string>>();

//    private static void FindReferences(string pattern)
//    {
//        guidList = new List<string>(Selection.assetGUIDs);
//        pathList = new List<string>();
//        for (int i = 0; i < guidList.Count; ++i)
//        {
//            pathList.Add(AssetDatabase.GUIDToAssetPath(guidList[i]));
           
//        }

//        GetFilesWithPattern(pattern);

//        CheckEveryPrefab();
//    }

//    private static void GetFilesWithPattern(string pattern)
//    {
//        string[] prefabArr = GetFiles(Application.dataPath, pattern);
//        prefabList = new List<string>(prefabArr);
//    }

//    private static string[] GetFiles(string folderPath, string pattern, bool recursive = true)
//    {
//        List<string> pathList = new List<string>();
//        string[] dirArr = Directory.GetFiles(folderPath, pattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
//        for (int i = 0; i < dirArr.Length; ++i)
//        {
//            if (Path.GetExtension(dirArr[i]) != ".meta")
//                pathList.Add(dirArr[i].Replace('\\', '/'));
//        }
//        return pathList.ToArray();
//    }

//    private static void CheckEveryPrefab()
//    {
//        if (guidList == null || guidList.Count == 0)
//            return;

//        if (prefabList == null || prefabList.Count == 0)
//            return;

//        referDict.Clear();
//        for (int i = 0; i < guidList.Count; ++i)
//        {
//            for (int j = 0; j < prefabList.Count; ++j)
//            {
//                EditorUtility.DisplayProgressBar("搜索预设引用关系", "搜索中..." + pathList[i], j / prefabList.Count);
//                FileStream fs = new FileStream(prefabList[j], FileMode.Open, FileAccess.Read);
//                byte[] buff = new byte[fs.Length];
//                fs.Read(buff, 0, (int)fs.Length);
//                string strText = Encoding.Default.GetString(buff);
//                int index = strText.IndexOf(guidList[i]);
//                if (index != -1)
//                {
//                    Debug.Log(pathList[i] + " : " + prefabList[j].Replace(Application.dataPath, ""));
//                }
//                Debug.Log(prefabList[j]);
//            }
//        }
//        EditorUtility.ClearProgressBar();
//    }
//}