 
using UnityEngine;
using UnityEditor;
using System.IO;

public class ImageResizer : EditorWindow
{
    [MenuItem("Tools/Image Resizer")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ImageResizer));
    }

    private void OnGUI()
    {
        GUILayout.Label("Select the folder containing the images to resize:");
        if (GUILayout.Button("Select Folder"))
        {
            string path = EditorUtility.OpenFolderPanel("Select Folder", "", "");
            if (!string.IsNullOrEmpty(path))
            {
                ResizeImages(path);
            }
        }
    }

    private void ResizeImages(string folderPath)
    {
        string[] imagePaths = Directory.GetFiles(folderPath, "*.png");
        foreach (string imagePath in imagePaths)
        {
            Texture2D texture = new Texture2D(2, 2);
            byte[] imageData = File.ReadAllBytes(imagePath);
            texture.LoadImage(imageData);

            int newWidth = 512;
            int newHeight = 512;

            TextureScale.Bilinear(texture, newWidth, newHeight);

            byte[] newImageData = texture.EncodeToPNG();
            File.WriteAllBytes(imagePath, newImageData);
        }
    }
}
