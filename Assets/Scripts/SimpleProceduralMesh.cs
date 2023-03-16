using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// 程序网格
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SimpleProceduralMesh : MonoBehaviour
{
    void OnEnable()
    {
        var mesh = new Mesh
        {
            name = "Procedural"
        };

        mesh.vertices = new Vector3[] //顶点
        {
            Vector3.zero,Vector3.right,Vector3.up
        };
        mesh.triangles = new int[] //索引
        {
            0, 2, 1,
        };
        mesh.normals = new Vector3[] //法线
        {
            Vector3.back,Vector3.back,Vector3.back
        };

        mesh.tangents = new Vector4[]
        {
            new Vector4(1f, 0f, 0f, -1f),
            new Vector4(1f, 0f, 0f, -1f),
            new Vector4(1f, 0f, 0f, -1f),
            new Vector4(1f, 0f, 0f, -1f)
        };
        mesh.uv = new Vector2[] //uv
        {
            Vector2.zero,Vector2.right,Vector2.up
        };

        GetComponent<MeshFilter>().mesh = mesh;
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
