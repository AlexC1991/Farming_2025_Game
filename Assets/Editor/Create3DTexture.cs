using UnityEngine;
using UnityEditor;

public class Create3DTexture : EditorWindow
{
    [MenuItem("Tools/Create 3D Noise Texture")]
    public static void CreateTexture3D()
    {
        int size = 64;
        Texture3D texture = new Texture3D(size, size, size, TextureFormat.RGBA32, false);
        
        Color[] colors = new Color[size * size * size];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = new Color(Random.value, Random.value, Random.value, 1f);
        }
        
        texture.SetPixels(colors);
        texture.Apply();
        
        AssetDatabase.CreateAsset(texture, "Assets/Noise3D.asset");
        AssetDatabase.SaveAssets();
    }
}