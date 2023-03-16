//using Com.Core;
//using Com.Geometry.Surface;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;


//namespace ZWS.Equips
//{
//    using static Com.Geometry.ArcUtil;
//    class BuildSing : MonoBehaviour
//    {

//        public Vector3 origin;
//        public Vector3 forward;
//        public Vector3 right;
//        public Vector3 up;
//        public float width = 1;
//        public float Shuntwidth = 1;
//        public float frontLength = 1;
//        public float backLength = 1;
//        public float forkLength = 1;
//        public float forkAngle = 1;
//        public float cornerRadius = 1;

//        public Vector3 ForkDirection { get; private set; }

//        Vector3 forkLeftPoint;
//        public Vector3 ForkLeftPoint => forkLeftPoint;
//        Vector3 forkRightPoint;
//        public Vector3 ForkRightPoint => forkRightPoint;

//        public Vector3 QuadForward { get; private set; }
//        public Vector3 QuadBack { get; private set; }
//        public Vector3 FrontEndPoint { get; private set; }
//        public Vector3 BackEndPoint { get; private set; }
//        public Vector3 ForkEndPoint { get; private set; }
//        public Vector3 LenthDir { get; private set; }
//        public float VirtualForkCenterLength { get; private set; }


//        Vector2[] samplePoints = new Vector2[]
//         {
//            new Vector2(0,0), new Vector2(0.07f,0),new Vector2(0.07f,0.1f),new Vector2(0,0.1f)
//         };

//        public BuildSing()
//        {
//            origin = Vector3.zero;
//            forward = Vector3.forward;
//            right = Vector3.right;
//            up = Vector3.up;
//        }


//        private void Update()
//        {
//            if (Input.GetKeyDown(KeyCode.Space))
//            {
//                Debug.Log("切换Mesh");
//                SetMesh();
//            }

//        }

//        void SetMesh()
//        {
//            Mesh frameMeshFunc(IEnumerable<Vector3> arg) => BuildCorneredPathSurface(arg, samplePoints, cornerRadius, 120);

//            var mesh = frameMeshFunc((Vector3.zero, Vector3.forward * 3, Vector3.forward * 4 + Vector3.right, Vector3.forward * 7 + Vector3.right).Seq());
//            transform.GetComponent<MeshFilter>().mesh = mesh;
//        }

//        public Mesh BuildSingleForkDrumConveyorFrameMesh(IList<Vector2> samplePoints)
//        {
//            ForkDirection = Quaternion.AngleAxis(forkAngle, up) * forward;  //

//            var halfWidth = width * 0.5f;
//            var ShunthalfWidth = Shuntwidth;
//            var rightMulHalfWidth = right * halfWidth;
//            var ShuntWidth = right * ShunthalfWidth;
//            var forkRightDir = Vector3.Cross(up, ForkDirection).normalized;
//            var forkRightDirMulHalfWidth = forkRightDir * halfWidth;
//            var rightPlane = new Plane(right, origin + rightMulHalfWidth);
//            rightPlane.RaycastPoint(new Ray(origin - forkRightDirMulHalfWidth, ForkDirection), out forkLeftPoint);  //
//            rightPlane.RaycastPoint(new Ray(origin + forkRightDirMulHalfWidth, ForkDirection), out forkRightPoint);  //

//            QuadForward = forkLeftPoint - rightMulHalfWidth;  //
//            QuadBack = forkRightPoint - rightMulHalfWidth;  //

//            Mesh frameMeshFunc(IEnumerable<Vector3> arg) => BuildCorneredPathSurface(arg, samplePoints, cornerRadius, 120);
//            var mainForwardCenter = QuadForward + forward * frontLength;

//            FrontEndPoint = mainForwardCenter;
//            var mainBackCenter = QuadBack - forward * backLength;
//            BackEndPoint = mainBackCenter;


//            var leftEdgeMesh = frameMeshFunc(((mainForwardCenter + rightMulHalfWidth) - ShuntWidth, (mainBackCenter + rightMulHalfWidth) - ShuntWidth).Seq());
//            // 分流部分长度解释为两侧取最短的长度
//            var tanTheta = Mathf.Tan(forkAngle * Mathf.Deg2Rad);
//            var forkLenCenter = (tanTheta * forkLength + Mathf.Sign(90 - forkAngle) * halfWidth) / tanTheta;
//            VirtualForkCenterLength = forkLenCenter;
//            var forkDirMulForkLen = ForkDirection * forkLenCenter;
//            var forkStartCenter = Vector3.Lerp(forkRightPoint, forkLeftPoint, 0.5f);
//            ForkEndPoint = forkStartCenter + ForkDirection * forkLenCenter;
//            var forkRightMesh = frameMeshFunc((mainBackCenter + rightMulHalfWidth, forkRightPoint, forkStartCenter + forkRightDirMulHalfWidth + forkDirMulForkLen).Seq());

//            LenthDir = (((mainBackCenter + rightMulHalfWidth) - mainBackCenter + rightMulHalfWidth) - ShuntWidth);

//            var forkLeftMesh = frameMeshFunc((forkStartCenter - forkRightDirMulHalfWidth + forkDirMulForkLen, forkLeftPoint, mainForwardCenter + rightMulHalfWidth).Seq());

//            combine3[0] = new CombineInstance { mesh = leftEdgeMesh };
//            combine3[1] = new CombineInstance { mesh = forkRightMesh };
//            combine3[2] = new CombineInstance { mesh = forkLeftMesh };
//            combine3_1[0] = new CombineInstance { mesh = GetSubFromCombine3(0) };
//            combine3_1[1] = new CombineInstance { mesh = GetSubFromCombine3(1) };
//            combine3_1[2] = new CombineInstance { mesh = GetSubFromCombine3(2) };
//            var mesh = new Mesh { name = "edge" };
//            mesh.CombineMeshes(combine3_1, true, false);
//            return mesh;
//        }

//        static readonly CombineInstance[] combine3 = new CombineInstance[3];
//        static readonly CombineInstance[] combine3_1 = new CombineInstance[3];
//        static Mesh GetSubFromCombine3(int subIndex)
//        {
//            for (int i = 0; i < 3; i++)
//            {
//                ref var c = ref combine3[i];
//                c.subMeshIndex = subIndex;
//            }
//            var sub = new Mesh();
//            sub.CombineMeshes(combine3, true, false);
//            return sub;
//        }

//        public static Mesh BuildCorneredPathSurface(IEnumerable<Vector3> pathPoints, IList<Vector2> samplePoints, float cornerRadius, float smoothAngle)
//        {
//            var rawPath = SharedCollection<Vector3>.ListCell.Create(pathPoints);
//            if (rawPath.Count < 2)
//            {
//                Debug.LogWarning($"points of path is {rawPath.Count} < 3");
//                return null;
//            }
//            if (rawPath.Count == 2)
//            {
//                return PathSurfaceBuilder.BuildPathSurface(samplePoints, rawPath, smoothAngle);
//            }
//            var path = SharedCollection<Vector3>.ListCell.Create();

//            var cornerSegments = SharedCollection<Vector3>.ListCell.Create();
//            {
//                path.Add(rawPath[0]);
//                for (int i = 1; i < rawPath.Count - 1; i++)
//                {
//                    var prev = rawPath[i - 1];
//                    var cur = rawPath[i];
//                    var next = rawPath[i + 1];

//                    if (CalRoundCorner(prev, cur, next, cornerRadius, out var arc))
//                    {
//                        const float MIN_SEGMENT_LEN = 0.02f;
//                        const int SEGMENT_NUMBER = 24;
//                        cornerSegments.Clear();
//                        arc.ArcPointsNonAlloc(arc.SegmentNums(SEGMENT_NUMBER, MIN_SEGMENT_LEN), cornerSegments);
//                        path.Body.AddRange(Vector3.Dot(arc.normal, Vector3.Cross(next - cur, prev - cur)) > 0 ?
//                           cornerSegments :  // 顺向
//                           cornerSegments.Reverse());  // 反向
//                    }
//                    else
//                    {
//                        path.Body.AddRange((prev, cur, next).Seq());
//                    }
//                }
//                path.Add(rawPath.Tail());
//            }

//            return PathSurfaceBuilder.BuildPathSurface(samplePoints, path, smoothAngle);
//        }


//    }
//}
