using UnityEngine;
using System.Collections.Generic;

public class GrassManager : MonoBehaviour
{
    [Header("Grass Shader Setup")]
    public Material grassMaterial; // Your grass shader material
    public Mesh grassMesh; // Simple quad mesh for grass blade
    
    [Header("Ground Setup")]
    public Transform groundPlane; // Your ground object
    public float groundHeight = 0f; // Height of your ground
    
    [Header("Spawn Settings")]
    public int grassCount = 5000;
    public float spawnAreaSize = 50f;
    public bool spawnOnStart = true;
    
    [Header("Grass Appearance")]
    public float grassScale = 1f;
    public float scaleVariation = 0.3f;
    public float rotationVariation = 360f;
    
    [Header("Performance")]
    public int instancesPerBatch = 1023; // Unity's limit for DrawMeshInstanced
    public bool enableFrustumCulling = true;
    public float maxRenderDistance = 150f;
    
    [Header("Shadow Settings")]
    [Range(0f, 1f)]
    public float shadowIntensity = 0.5f; // Controls how dark shadows are
    public bool useSoftShadows = true; // Toggle between hard/soft shadows
    [Range(0f, 10f)]
    public float shadowSoftness = 2f; // Controls shadow blur/softness
    [Range(0f, 2f)]
    public float shadowBias = 0.05f; // Reduces shadow acne
    [Range(0f, 1f)]
    public float normalBias = 0.4f; // Reduces peter panning
    
    // Rendering data
    private List<Matrix4x4[]> grassBatches = new List<Matrix4x4[]>();
    private List<Vector3> grassPositions = new List<Vector3>();
    private Camera playerCamera;
    
    void Start()
    {
        playerCamera = Camera.main;
        if (playerCamera == null)
            playerCamera = FindObjectOfType<Camera>();
            
        if (grassMaterial == null)
        {
            Debug.LogError("Grass Material is not assigned! Please assign your grass shader material.");
            return;
        }
        
        CreateGrassMesh();
        ApplyShadowSettings();
        
        if (spawnOnStart)
        {
            GenerateGrass();
        }
    }
    
    void CreateGrassMesh()
    {
        if (grassMesh == null)
        {
            grassMesh = new Mesh();
            grassMesh.name = "GrassBlade";
            
            // Create a simple quad for the grass blade
            Vector3[] vertices = new Vector3[]
            {
                new Vector3(-0.5f, 0f, 0f),  // Bottom left
                new Vector3(0.5f, 0f, 0f),   // Bottom right
                new Vector3(-0.3f, 1f, 0f),  // Top left (slightly narrower)
                new Vector3(0.3f, 1f, 0f)    // Top right (slightly narrower)
            };
            
            Vector2[] uvs = new Vector2[]
            {
                new Vector2(0f, 0f), // Bottom left
                new Vector2(1f, 0f), // Bottom right
                new Vector2(0f, 1f), // Top left
                new Vector2(1f, 1f)  // Top right
            };
            
            int[] triangles = new int[]
            {
                0, 2, 1, // First triangle
                2, 3, 1  // Second triangle
            };
            
            Vector3[] normals = new Vector3[]
            {
                Vector3.forward,
                Vector3.forward,
                Vector3.forward,
                Vector3.forward
            };
            
            grassMesh.vertices = vertices;
            grassMesh.uv = uvs;
            grassMesh.triangles = triangles;
            grassMesh.normals = normals;
            grassMesh.bounds = new Bounds(Vector3.zero, Vector3.one * 2f);
        }
    }
    
    [ContextMenu("Generate Grass")]
    public void GenerateGrass()
    {
        grassPositions.Clear();
        grassBatches.Clear();
        
        // Determine spawn center
        Vector3 spawnCenter = transform.position;
        if (groundPlane != null)
        {
            spawnCenter = new Vector3(groundPlane.position.x, groundHeight, groundPlane.position.z);
            // Don't override groundHeight - use the value set in inspector
        }
        
        Debug.Log($"Generating {grassCount} grass instances at height {groundHeight}");
        
        // Generate grass positions
        List<Matrix4x4> allMatrices = new List<Matrix4x4>();
        
        for (int i = 0; i < grassCount; i++)
        {
            // Generate random position within spawn area
            Vector3 randomPos = spawnCenter + new Vector3(
                Random.Range(-spawnAreaSize * 0.5f, spawnAreaSize * 0.5f),
                groundHeight,
                Random.Range(-spawnAreaSize * 0.5f, spawnAreaSize * 0.5f)
            );
            
            grassPositions.Add(randomPos);
            
            // Create transformation matrix
            Matrix4x4 matrix = CreateGrassMatrix(randomPos);
            allMatrices.Add(matrix);
        }
        
        // Split into batches for rendering
        CreateRenderBatches(allMatrices);
        
        Debug.Log($"Created {grassCount} grass instances in {grassBatches.Count} batches");
    }
    
    Matrix4x4 CreateGrassMatrix(Vector3 position)
    {
        // Random rotation around Y axis
        float rotationY = Random.Range(0f, rotationVariation);
        Quaternion rotation = Quaternion.Euler(0f, rotationY, 0f);
        
        // Random scale with variation
        float scale = grassScale * (1f + Random.Range(-scaleVariation, scaleVariation));
        Vector3 scaleVector = new Vector3(scale, scale, scale);
        
        return Matrix4x4.TRS(position, rotation, scaleVector);
    }
    
    void CreateRenderBatches(List<Matrix4x4> matrices)
    {
        grassBatches.Clear();
        
        // Split matrices into batches
        for (int i = 0; i < matrices.Count; i += instancesPerBatch)
        {
            int batchSize = Mathf.Min(instancesPerBatch, matrices.Count - i);
            Matrix4x4[] batch = new Matrix4x4[batchSize];
            
            for (int j = 0; j < batchSize; j++)
            {
                batch[j] = matrices[i + j];
            }
            
            grassBatches.Add(batch);
        }
    }
    
    void Update()
    {
        if (grassBatches.Count > 0 && grassMaterial != null && grassMesh != null)
        {
            RenderGrass();
        }
        
        // Update shadow settings if they changed
        if (Application.isPlaying)
        {
            UpdateShadowSettings();
        }
    }
    
    void RenderGrass()
    {
        foreach (Matrix4x4[] batch in grassBatches)
        {
            if (enableFrustumCulling && !ShouldRenderBatch(batch))
                continue;
                
            // Render this batch using the shader
            Graphics.DrawMeshInstanced(
                grassMesh,           // The grass blade mesh
                0,                   // Submesh index
                grassMaterial,       // Your grass shader material
                batch,               // Transformation matrices
                batch.Length         // Number of instances
            );
        }
    }
    
    bool ShouldRenderBatch(Matrix4x4[] batch)
    {
        if (playerCamera == null) return true;
        
        Vector3 cameraPos = playerCamera.transform.position;
        
        // Check if any grass in this batch is within render distance
        foreach (Matrix4x4 matrix in batch)
        {
            Vector3 grassPos = new Vector3(matrix.m03, matrix.m13, matrix.m23);
            float distance = Vector3.Distance(cameraPos, grassPos);
            
            if (distance <= maxRenderDistance)
            {
                return true;
            }
        }
        
        return false;
    }
    
    [ContextMenu("Clear Grass")]
    public void ClearGrass()
    {
        grassBatches.Clear();
        grassPositions.Clear();
        Debug.Log("Cleared all grass");
    }
    
    [ContextMenu("Regenerate Grass")]
    public void RegenerateGrass()
    {
        GenerateGrass();
    }
    
    void OnDrawGizmosSelected()
    {
        Vector3 center = transform.position;
        if (groundPlane != null)
        {
            center = new Vector3(groundPlane.position.x, groundHeight, groundPlane.position.z);
        }
        
        // Draw spawn area
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, new Vector3(spawnAreaSize, 0.1f, spawnAreaSize));
        
        // Draw render distance
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(center, maxRenderDistance);
        
        // Draw grass positions (only first 100 for performance)
        if (grassPositions.Count > 0)
        {
            Gizmos.color = Color.red;
            int maxDraw = Mathf.Min(100, grassPositions.Count);
            for (int i = 0; i < maxDraw; i++)
            {
                Gizmos.DrawWireSphere(grassPositions[i], 0.1f);
            }
        }
    }
    
    void OnValidate()
    {
        // Clamp values to reasonable ranges
        grassCount = Mathf.Max(1, grassCount);
        spawnAreaSize = Mathf.Max(1f, spawnAreaSize);
        instancesPerBatch = Mathf.Clamp(instancesPerBatch, 1, 1023);
        maxRenderDistance = Mathf.Max(1f, maxRenderDistance);
        
        // Apply shadow settings when values change in editor
        if (Application.isPlaying)
        {
            ApplyShadowSettings();
        }
    }
    
    void ApplyShadowSettings()
    {
        // Find the main directional light (sun)
        Light mainLight = FindMainDirectionalLight();
        
        if (mainLight != null)
        {
            // Set shadow type based on soft shadow toggle
            mainLight.shadows = useSoftShadows ? LightShadows.Soft : LightShadows.Hard;
            
            // Apply shadow intensity (0 = no shadows, 1 = full shadows)
            mainLight.shadowStrength = shadowIntensity;
            
            // Apply shadow bias settings
            mainLight.shadowBias = shadowBias;
            mainLight.shadowNormalBias = normalBias;
            
            // Set shadow resolution based on softness setting
            if (useSoftShadows)
            {
                // Higher resolution for softer shadows
                mainLight.shadowResolution = shadowSoftness > 5f ? 
                    UnityEngine.Rendering.LightShadowResolution.VeryHigh : 
                    UnityEngine.Rendering.LightShadowResolution.High;
            }
            else
            {
                // Medium resolution for hard shadows
                mainLight.shadowResolution = UnityEngine.Rendering.LightShadowResolution.Medium;
            }
            
            Debug.Log($"Applied shadow settings: Intensity={shadowIntensity}, Soft={useSoftShadows}, Softness={shadowSoftness}");
        }
        else
        {
            Debug.LogWarning("No main directional light found. Shadow settings cannot be applied.");
        }
        
        // Apply material-specific shadow settings
        ApplyMaterialShadowSettings();
    }
    
    void UpdateShadowSettings()
    {
        // Only update if we're in play mode and settings might have changed
        Light mainLight = FindMainDirectionalLight();
        if (mainLight != null)
        {
            // Check if shadow settings need updating
            bool needsUpdate = false;
            
            if (Mathf.Abs(mainLight.shadowStrength - shadowIntensity) > 0.01f)
                needsUpdate = true;
            if ((useSoftShadows && mainLight.shadows != LightShadows.Soft) || 
                (!useSoftShadows && mainLight.shadows != LightShadows.Hard))
                needsUpdate = true;
            if (Mathf.Abs(mainLight.shadowBias - shadowBias) > 0.001f)
                needsUpdate = true;
            if (Mathf.Abs(mainLight.shadowNormalBias - normalBias) > 0.01f)
                needsUpdate = true;
                
            if (needsUpdate)
            {
                ApplyShadowSettings();
            }
        }
    }
    
    Light FindMainDirectionalLight()
    {
        // Find the main directional light in the scene
        Light[] lights = FindObjectsOfType<Light>();
        
        foreach (Light light in lights)
        {
            if (light.type == LightType.Directional && light.enabled)
            {
                return light;
            }
        }
        
        return null;
    }
    
    void ApplyMaterialShadowSettings()
    {
        if (grassMaterial != null)
        {
            // Set material properties for shadow receiving
            if (grassMaterial.HasProperty("_ShadowIntensity"))
            {
                grassMaterial.SetFloat("_ShadowIntensity", shadowIntensity);
            }
            
            if (grassMaterial.HasProperty("_ShadowSoftness"))
            {
                grassMaterial.SetFloat("_ShadowSoftness", shadowSoftness);
            }
            
            // Enable shadow receiving
            grassMaterial.EnableKeyword("_RECEIVE_SHADOWS_OFF");
        }
    }
    
    [ContextMenu("Apply Shadow Settings")]
    public void ApplyShadowSettingsManually()
    {
        ApplyShadowSettings();
    }
    
    [ContextMenu("Reset Shadow Settings")]
    public void ResetShadowSettings()
    {
        shadowIntensity = 0.5f;
        useSoftShadows = true;
        shadowSoftness = 2f;
        shadowBias = 0.05f;
        normalBias = 0.4f;
        ApplyShadowSettings();
    }
    
    // Public properties for runtime access
    public int TotalGrassCount => grassPositions.Count;
    public int BatchCount => grassBatches.Count;
    public bool IsGrassGenerated => grassBatches.Count > 0;
}