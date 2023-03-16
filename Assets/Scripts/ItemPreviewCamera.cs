//using FairyGUI;
//using UnityEngine;

//namespace ZWS.InternalUI.EquipIndicatorUI
//{
//    /// <summary>
//    /// 相机的运动形态
//    /// </summary>
//    public class ItemPreviewCamera
//    {

//        GameObject Camera;
//        CreatTextureCameraModle CreatTCM;
//        Transform car;

//        public ItemPreviewCamera(GComponent com, CreatTextureCameraModle CreatTCM)
//        {
//            this.CreatTCM = CreatTCM;
//        }
//        public void CameraAction()
//        {
//            Camera = CreatTCM.Camera;
//            car = CreatTCM.rootNode.transform;
//            //car = CreatTCM.Cargo.gameObject.transform;
//            camerarotate();
//             camerazoom();
//        }

//        public void AotuRotate()
//        {
//            Camera = CreatTCM.Camera;
//            car = CreatTCM.Cargo.gameObject.transform;
//            Camera.transform.RotateAround(car.position, Vector3.up, 1 * Time.deltaTime); //摄像机围绕目标旋转
//        }

//        private void camerarotate() //摄像机围绕目标旋转操作
//        {
//            var mouse_x = Input.GetAxis("Mouse X");//获取鼠标X轴移动
//            var mouse_y = -Input.GetAxis("Mouse Y");//获取鼠标Y轴移动

//            ///右键拖动
//              if (Input.GetKey(KeyCode.Mouse1))
//              {
//                  Camera.transform.Translate(Vector3.left * (mouse_x * 15f) * Time.deltaTime);
//                  Camera.transform.Translate(Vector3.up * (mouse_y * 15f) * Time.deltaTime);
//              }
//            ///左键旋转
//            if (Input.GetKey(KeyCode.Mouse0))
//            {
//                Camera.transform.RotateAround(car.position, Vector3.up, mouse_x * 5);
//                Camera.transform.RotateAround(car.position, Camera.transform.right, mouse_y * 5);
//            }
//        }

//        private void camerazoom() //摄像机滚轮缩放
//        {
//            if (Input.GetAxis("Mouse ScrollWheel") > 0)
//                Camera.transform.Translate(Vector3.forward * 0.5f);


//            if (Input.GetAxis("Mouse ScrollWheel") < 0)
//                Camera.transform.Translate(Vector3.forward * -0.5f);
//        }
//    }
//}
