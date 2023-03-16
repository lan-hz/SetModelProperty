using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts
{
    internal class SetModel3 : Editor
    {
        [MenuItem("Assets/设置ModlesProperty", false, 50)]
        private static void FindReferencesInFbxs()
        {
            FindReferences();
        }

        private static List<string> ModleList;
        private static List<string> guidList;
        private static List<string> pathList;
        private static Dictionary<string, List<string>> referDict = new Dictionary<string, List<string>>();
        private static void FindReferences()
        {
            guidList = new List<string>(Selection.assetGUIDs);
            pathList = new List<string>();
            for (int i = 0; i < guidList.Count; ++i)
            {
                pathList.Add(AssetDatabase.GUIDToAssetPath(guidList[i]));
            }
            GetFilesWithPattern();
            CheckEveryModle();
        }

        private static void GetFilesWithPattern()
        {
            string[] pattern = new string[] //模型类型
            {
                "*.fbx",
                "*.obj",
                "*.3ds",
                "*.gltf",
                "*.dae",
                "*.mx"
            };
            //Application.dataPath  获取路径盘符到Assets
            List<string> prefabArr = GetFiles("Assets\\", pattern);
            Debug.Log(prefabArr);
            ModleList = new List<string>(prefabArr);
        }
        /// <summary>
        /// 获取模型路径
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="pattern"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        private static List<string> GetFiles(string folderPath, string[] pattern, bool recursive = true)
        {
            List<string> pathList = new List<string>();
            List<string> PathList = new List<string>();
            foreach (var item in pattern)
            {
                var DirArr = Directory.GetFiles(folderPath, item, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                DirArr.ToList();
                pathList.AddRange(DirArr);
            }
            for (int i = 0; i < pathList.Count; ++i)
            {
                if (Path.GetExtension(pathList[i]) != ".meta")
                    PathList.Add(pathList[i].Replace('\\', '/'));

                //Debug.Log($"dirArr[i]:{pathList[i]}");
            }
            return PathList;
        }
        /// <summary>
        /// 检查设置模型属性
        /// </summary>
        private static void CheckEveryModle()
        {
            for (int j = 0; j < ModleList.Count; j++)
            {
                EditorUtility.DisplayProgressBar("SetModel", "设置中..." + ModleList[j], (float)j / (float)ModleList.Count);
                //Debug.Log($"ModleList[i]:{ModleList[j]}");
                ModelImporter modelimporter = ModelImporter.GetAtPath(ModleList[j]) as ModelImporter;
                modelimporter.isReadable = true;
                modelimporter.importVisibility = false;
                modelimporter.importCameras = false;
                modelimporter.importLights = false;
                modelimporter.preserveHierarchy = false;
                modelimporter.animationType = ModelImporterAnimationType.None;
                modelimporter.importAnimation = false;
                modelimporter.importConstraints = false;

                modelimporter.SaveAndReimport();
            }

            EditorUtility.ClearProgressBar();
        }

    }
}
