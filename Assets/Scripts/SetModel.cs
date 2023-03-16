using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;
using static UnityEditor.AssetImporter;

class SetModel : MonoBehaviour
{

    private void Start()
    {

    }
   // public static void ModifyMeshReadable()
    //{
    //    FileInfo[] files = new DirectoryInfo(modelPath).GetFiles("*.FBX", SearchOption.AllDirectories);

    //    for (int i = 0; i < files.Length; i++)
    //    {
    //        FileInfo file = files[i];
    //        // 路径要为Assets/xxx/ 
    //        string absPath = "Assets/" + file.FullName.Replace("\\", "/").Replace(Application.dataPath + "/", "");
    //        Debug.Log(absPath);
    //        ModelImporter modelimporter = ModelImporter.GetAtPath(absPath) as ModelImporter;
    //        modelimporter.isReadable = true;
    //        modelimporter.SaveAndReimport();
    //    }
    //    AssetDatabase.Refresh();
   // }
    void SeeAbout()
    {

        print("按名称：  " + GameObject.Find("name--01").name);


        //查找到结果为单个物体
        print("按标签（单个）：  " + GameObject.FindGameObjectWithTag("tag").name);


        //查找到结果为所有符合的物体返回数组
        GameObject[] Objs;
        Objs = GameObject.FindGameObjectsWithTag("tag");
        for (int i = 0; i < Objs.Length; i++)
        {
            print("按标签（多个）：  " + Objs[i].name);
        }


        //查找到结果为所有符合的物体返回单个数据    
        ModelImporter findObj = (ModelImporter)GameObject.FindObjectOfType(typeof(ModelImporter));
        print("按类型查找（单个）：   " + findObj.name);


        //查找到结果为所有符合的物体返回数组
        ModelImporter[] findObjs = (ModelImporter[])GameObject.FindObjectsOfType(typeof(ModelImporter));
        for (int i = 0; i < findObjs.Length; i++)
        {
            print("按类型查找（多个）：   " + findObjs[i].name);
        }


    }
}


