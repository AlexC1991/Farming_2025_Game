using UnityEngine;

[CreateAssetMenu(fileName = "New3DTexture", menuName = "Custom/3D Noise Texture")]
public class Texture3DGenerator : ScriptableObject
{
    [Header("Texture Settings")]
    public int resolution = 64;
    public float noiseScale = 0.1f;
    public int octaves = 3;
    
    [ContextMenu("Generate 3D Noise Texture")]
    public void Generate3DNoiseTexture()
    {
        // Create the 3D texture
        Texture3D texture3D = new Texture3D(resolution, resolution, resolution, TextureFormat.RGBA32, false);
        
        Color[] colors = new Color[resolution * resolution * resolution];
        
        for (int z = 0; z < resolution; z++)
        {
            for (int y = 0; y < resolution; y++)
            {
                for (int x = 0; x < resolution; x++)
                {
                    float xCoord = (float)x / resolution * noiseScale;
                    float yCoord = (float)y / resolution * noiseScale;
                    float zCoord = (float)z / resolution * noiseScale;
                    
                    float noise = 0f;
                    float amplitude = 1f;
                    float frequency = 1f;
                    
                    // Generate fractal noise
                    for (int i = 0; i < octaves; i++)
                    {
                        noise += Mathf.PerlinNoise(xCoord * frequency, yCoord * frequency) * 
                                Mathf.PerlinNoise(yCoord * frequency, zCoord * frequency) * 
                                Mathf.PerlinNoise(xCoord * frequency, zCoord * frequency) * amplitude;
                        amplitude *= 0.5f;
                        frequency *= 2f;
                    }
                    
                    noise = Mathf.Clamp01(noise);
                    
                    int index = x + y * resolution + z * resolution * resolution;
                    colors[index] = new Color(noise, noise, noise, 1f);
                }
            }
        }
        
        texture3D.SetPixels(colors);
        texture3D.Apply();
        
        // Save as asset
        string path = "Assets/Generated3DNoiseTexture.asset";
        UnityEditor.AssetDatabase.CreateAsset(texture3D, path);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
        
        Debug.Log("3D Noise Texture created at: " + path);
    }
}