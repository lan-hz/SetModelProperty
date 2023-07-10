using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

namespace CZGame
{

    namespace feicun.Assets.UnityLib.Editor
    {
        public class ShowAllNotUseResourceDeal
        {

            [MenuItem("Tools/资源检查")]
            public static void ShowAllNotUseResource()
            {
                EditorWindow.GetWindow(typeof(ShowAllNotUseResource), false, "资源检查"); //>(false,"",true);
            }

        }
    }
    public class ShowAllNotUseResource : EditorWindow
    {


        private static List<string> checkType = new List<string>();
        private static List<string> allResource = new List<string>();  //所有使用资源
        private static List<string> notUseReource = new List<string>();  //没有使用过的资源

        static Dictionary<string, List<string>> mapReferenceOtherModulePref = new Dictionary<string, List<string>>();
        Vector2 scrollPos = Vector2.zero;
        private static void StartCheck()
        {
            List<string> withoutExtensions = new List<string>() { ".prefab", ".unity" };
            string[] files = Directory.GetFiles(Application.dataPath + "", "*.*", SearchOption.AllDirectories)
                .Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray();

            List<string> withoutExtensions2 = new List<string>() { ".prefab" };
            //所有project 里面的资源
            string[] UIReource = Directory.GetFiles(Application.dataPath + "", "*.*", SearchOption.AllDirectories)
                 .Where(s => withoutExtensions2.Contains(Path.GetExtension(s).ToLower())).ToArray();

            for (int i = 0; i < UIReource.Length; i++)
            {
                UIReource[i] = UIReource[i].Replace("\\", "/");
                int index = UIReource[i].IndexOf("Assets");
                UIReource[i] = UIReource[i].Substring(index);
            }

            mapReferenceOtherModulePref.Clear();
            float count = 0;
            foreach (string file in files)
            {
                // Debug.Log("files :" + file);
                count++;
                EditorUtility.DisplayProgressBar("Processing...", "检索中....", count / files.Length);

                string strFile = file.Substring(file.IndexOf("Asset")).Replace('\\', '/');
                string[] dependenciesFiles = AssetDatabase.GetDependencies(strFile);

                foreach (string depFile in dependenciesFiles)
                {
                    bool isNeedShow = false;
                    foreach (string type in checkType)
                    {
                        //存在设置类型  需要显示
                        if (depFile.IndexOf(type) > -1)
                        {
                            isNeedShow = true;
                            break;
                        }
                    }
                    if (isNeedShow == false)
                    {
                        continue;
                    }
                    allResource.Add(depFile);

                }
            }

            EditorUtility.ClearProgressBar();


            for (int i = 0; i < UIReource.Length; i++)
            {
                bool isUse = false;
                foreach (string usePath in allResource)
                {
                    if (UIReource[i] == usePath)
                    {
                        isUse = true;
                        break;
                    }
                }

                if (isUse == false)
                {
                    notUseReource.Add(UIReource[i]);
                }

            }
        }

        void OnGUI()
        {

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("列出所有未使用过的资源", GUILayout.Width(200)))
            {
                allResource = new List<string>();
                notUseReource = new List<string>();
                checkType = new List<string>() { ".prefab" };
                // checkType.Add(".jpg");
                // checkType.Add(".png");
                StartCheck();
            }

            if (GUILayout.Button("删除所有 !", GUILayout.Width(200)))
            {

                float count = 0;
                foreach (var path in notUseReource)
                {
                    count++;
                    EditorUtility.DisplayProgressBar("Processing...", "删除中... (" + count + "/ " + notUseReource.Count + ")", count / notUseReource.Count);
                    File.Delete(path);
                    AssetDatabase.Refresh();
                }

                EditorUtility.ClearProgressBar();
            }
            EditorGUILayout.EndHorizontal();

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            foreach (var path in notUseReource)
            {

                if (path.IndexOf(".prefab") > -1)
                {
                    Debug.Log("Unuse prefab : " + path);
                    EditorGUILayout.BeginHorizontal();
                    Material t = (Material)AssetDatabase.LoadAssetAtPath(path, typeof(Material));
                    EditorGUILayout.ObjectField(t, typeof(Material), false);

                    if (GUILayout.Button("删除", GUILayout.Width(200)))
                    {
                        File.Delete(path);
                        AssetDatabase.Refresh();
                    }

                    EditorGUILayout.Space();

                    EditorGUILayout.EndHorizontal();
                }

                if (path.IndexOf(".mat") > -1)
                {
                    EditorGUILayout.BeginHorizontal();
                    Material t = (Material)AssetDatabase.LoadAssetAtPath(path, typeof(Material));
                    EditorGUILayout.ObjectField(t, typeof(Material), false);

                    if (GUILayout.Button("删除", GUILayout.Width(200)))
                    {
                        File.Delete(path);
                        AssetDatabase.Refresh();
                    }

                    EditorGUILayout.Space();

                    EditorGUILayout.EndHorizontal();
                }

                if (path.IndexOf(".png") > -1 || path.IndexOf(".jpg") > -1 || path.IndexOf(".tga") > -1)
                {
                    EditorGUILayout.BeginHorizontal();
                    Texture2D t = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));
                    EditorGUILayout.ObjectField(t, typeof(Texture2D), false);

                    if (GUILayout.Button("删除", GUILayout.Width(200)))
                    {
                        File.Delete(path);
                        AssetDatabase.Refresh();
                    }

                    EditorGUILayout.Space();

                    EditorGUILayout.EndHorizontal();
                }



            }
            EditorGUILayout.EndScrollView();



        }
    }
}