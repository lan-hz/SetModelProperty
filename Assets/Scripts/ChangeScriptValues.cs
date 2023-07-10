using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// 工具类用于替换脚本中的某一个值
/// </summary>

public class ChangeScriptValues : MonoBehaviour
{
    [MenuItem("Tools/Change Script Values")]
    static void ChangeValues()
    {
        string folderPath = EditorUtility.OpenFolderPanel("Select Folder", "", "");
        string[] scriptPaths = Directory.GetFiles(folderPath, "*.cs", SearchOption.AllDirectories); // 获取所有扩展名为 .cs 的文件路径

        foreach (string scriptPath in scriptPaths)
        {

            string scriptContent = File.ReadAllText(scriptPath);

            // 替换 len, wid, hei 的 value 值
            scriptContent = scriptContent.Replace("LENGTH_MIN_DEFAULT = 100", "LENGTH_MIN_DEFAULT = 1");
            // scriptContent = scriptContent.Replace("HEIGHT_MIN_DEFAULT = 100", "HEIGHT_MIN_DEFAULT = 1");
            // scriptContent = scriptContent.Replace("WIDTH_MIN_DEFAULT = 100", "WIDTH_MIN_DEFAULT = 1");

            File.WriteAllText(scriptPath, scriptContent);
        }
    }
}
