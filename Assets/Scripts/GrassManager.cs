using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrassManager : MonoBehaviour
{
    [Header("Basic Setup")]
    public Camera targetCamera;
    public Material grassMaterial;
    public Transform groundPlane;
    public float groundHeight = 0f;
    
    [Header("Grass Meshes - LOD System")]
    public Mesh grassMeshLOD0; // Highest quality mesh (close distance)
    public Mesh grassMeshLOD1; // Medium quality mesh  
    public Mesh grassMeshLOD2; // Low quality mesh (far distance)
    
    [Header("Simple Spawn Settings")]
    public int grassCount = 500;
    public float spawnAreaSize = 30f;
    
    [Header("Grass Appearance")]
    public float grassScale = 1f;
    public float scaleVariation = 0.3f;
    public float rotationVariation = 360f;
    [Range(0.01f, 0.5f)]
    public float grassWidth = 0.1f;
    [Range(0.1f, 3f)]
    public float grassHeight = 1f;
    
    [Header("LOD System")]
    public bool enableLODSystem = true;
    public float lodDistance0 = 20f;
    public float lodDistance1 = 40f;
    public float lodDistance2 = 80f;
    [Range(0.1f, 1f)]
    public float lodDensity1 = 0.9f;
    [Range(0.1f, 1f)]
    public float lodDensity2 = 0.8f;
    
    [Header("Performance")]
    public bool enableDistanceCulling = true;
    public float maxRenderDistance = 150f;
    public bool enableFrustumCulling = true;
    public int maxInstancesPerBatch = 500;
    [Range(0.05f, 1f)]
    public float lodCheckInterval = 0.2f; // How often to check for LOD changes
    
    [Header("Shadow Settings")]
    [Range(0f, 1f)]
    public float shadowIntensity = 0.9f;
    public bool useSoftShadows = true;
    [Range(0f, 10f)]
    public float shadowSoftness = 5.54f;
    [Range(0f, 2f)]
    public float shadowBias = 0.6f;
    [Range(0f, 1f)]
    public float normalBias = 0.15f;
    
    [Header("APU Optimization")]
    public bool enableAPUMode = false;
    [Range(0.1f, 1f)]
    public float apuDensityMultiplier = 0.6f;
    [Range(50, 300)]
    public int maxInstancesPerFrameAPU = 150;
    
    /*[Header("Debug")]
    public bool showGrassPositions = false;
    public bool showPerformanceInfo = true;
    */
    
    // Core grass data - always maintained
    private List<Matrix4x4> allGrassMatrices = new List<Matrix4x4>();
    private List<Vector3> allGrassPositions = new List<Vector3>();
    
    // Rendering batches - recreated when LOD changes
    private Matrix4x4[][] activeBatches;
    private Mesh activeMesh;
    
    // LOD system tracking
    private bool lastLODState = false;
    private Coroutine lodManagementCoroutine;
    private Coroutine shadowCoroutine;
    
    // Performance tracking
    private int lastRenderedCount = 0;
    private int lastTriangleCount = 0;
    private Plane[] cameraFrustumPlanes = new Plane[6];
    
    void Start()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main ?? FindObjectOfType<Camera>();
        }
        
        if (targetCamera == null)
        {
            /*Debug.LogError("No camera found! Please assign Target Camera.");*/
            return;
        }
        
        if (grassMaterial == null)
        {
            /*Debug.LogError("Grass Material is required!");*/
            return;
        }
        
        // Apply optimizations
        if (enableAPUMode)
        {
            ApplyAPUOptimizations();
        }
        
        // Setup meshes
        SetupLODMeshes();
        
        // Apply shadow settings
        ApplyPreferredShadowSettings();
        
        // Generate grass
        GenerateGrass();
        
        // Start management coroutines
        StartManagementSystem();
        
        /*Debug.Log($"Grass system initialized with {allGrassMatrices.Count} grass instances");*/
    }
    
    void ApplyAPUOptimizations()
    {
        grassCount = Mathf.RoundToInt(grassCount * apuDensityMultiplier);
        maxInstancesPerBatch = Mathf.Min(maxInstancesPerBatch, maxInstancesPerFrameAPU);
        lodDistance0 = Mathf.Min(lodDistance0, 15f);
        lodDistance1 = Mathf.Min(lodDistance1, 30f);
        lodDistance2 = Mathf.Min(lodDistance2, 50f);
        maxRenderDistance = Mathf.Min(maxRenderDistance, 60f);
        
        /*Debug.Log($"APU Mode: Optimized grass count to {grassCount}");*/
    }
    
    void SetupLODMeshes()
    {
        if (grassMeshLOD0 == null) grassMeshLOD0 = CreateGrassMeshWithSize(4, grassWidth, grassHeight);
        if (grassMeshLOD1 == null) grassMeshLOD1 = CreateGrassMeshWithSize(2, grassWidth * 0.8f, grassHeight * 0.9f);
        if (grassMeshLOD2 == null) grassMeshLOD2 = CreateGrassMeshWithSize(2, grassWidth * 0.6f, grassHeight * 0.8f);
    }
    
    Mesh CreateGrassMeshWithSize(int complexity, float width, float height)
    {
        Mesh mesh = new Mesh();
        mesh.name = $"GrassBlade_W{width:F2}_H{height:F2}";
        
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(-width, 0f, 0f),
            new Vector3(width, 0f, 0f),
            new Vector3(-width * 0.6f, height, 0f),
            new Vector3(width * 0.6f, height, 0f)
        };
        
        Vector2[] uvs = new Vector2[]
        {
            new Vector2(0f, 0f), new Vector2(1f, 0f),
            new Vector2(0f, 1f), new Vector2(1f, 1f)
        };
        
        int[] triangles = new int[] { 0, 2, 1, 2, 3, 1 };
        
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        
        return mesh;
    }
    
    [ContextMenu("Generate Grass")]
    public void GenerateGrass()
    {
        allGrassMatrices.Clear();
        allGrassPositions.Clear();
        
        Vector3 spawnCenter = transform.position;
        if (groundPlane != null)
        {
            spawnCenter = new Vector3(groundPlane.position.x, groundHeight, groundPlane.position.z);
        }
        
        /*Debug.Log($"Spawning {grassCount} grass at center: {spawnCenter}");*/
        
        // Generate grass in grid pattern
        int grassPerSide = Mathf.CeilToInt(Mathf.Sqrt(grassCount));
        float spacing = spawnAreaSize / grassPerSide;
        
        for (int x = 0; x < grassPerSide; x++)
        {
            for (int z = 0; z < grassPerSide; z++)
            {
                if (allGrassMatrices.Count >= grassCount) break;
                
                Vector3 basePos = spawnCenter + new Vector3(
                    (x - grassPerSide * 0.5f) * spacing,
                    groundHeight,
                    (z - grassPerSide * 0.5f) * spacing
                );
                
                Vector3 randomOffset = new Vector3(
                    Random.Range(-spacing * 0.4f, spacing * 0.4f),
                    0f,
                    Random.Range(-spacing * 0.4f, spacing * 0.4f)
                );
                
                Vector3 finalPos = basePos + randomOffset;
                
                Matrix4x4 grassMatrix = CreateGrassMatrixWithVariation(finalPos);
                allGrassMatrices.Add(grassMatrix);
                allGrassPositions.Add(finalPos);
            }
        }
        
        // Setup initial rendering system
        SetupRenderingForCurrentLOD();
        
        /*Debug.Log($"Generated {allGrassMatrices.Count} grass instances");*/
    }
    
    Matrix4x4 CreateGrassMatrixWithVariation(Vector3 position)
    {
        float rotationY = Random.Range(0f, rotationVariation);
        Quaternion rotation = Quaternion.Euler(0f, rotationY, 0f);
        
        float scale = grassScale * (1f + Random.Range(-scaleVariation, scaleVariation));
        Vector3 scaleVector = new Vector3(scale, scale, scale);
        
        return Matrix4x4.TRS(position, rotation, scaleVector);
    }
    
    void SetupRenderingForCurrentLOD()
    {
        if (allGrassMatrices.Count == 0)
        {
            /*Debug.LogWarning("No grass matrices to setup rendering for!");*/
            return;
        }
        
        if (enableLODSystem)
        {
            SetupLODRendering();
        }
        else
        {
            SetupSimpleRendering();
        }
        
        lastLODState = enableLODSystem;
    }
    
    void SetupLODRendering()
    {
        // For LOD, we'll use the highest quality mesh and handle distance in rendering
        activeMesh = grassMeshLOD0;
        activeBatches = CreateBatchArrayFromList(allGrassMatrices);
        
        /*Debug.Log($"Setup LOD rendering with {activeBatches.Length} batches using {activeMesh.name}");*/
    }
    
    void SetupSimpleRendering()
    {
        // For simple rendering, use LOD0 mesh and all grass
        activeMesh = grassMeshLOD0;
        activeBatches = CreateBatchArrayFromList(allGrassMatrices);
        
        /*Debug.Log($"Setup simple rendering with {activeBatches.Length} batches using {activeMesh.name}");*/
    }
    
    Matrix4x4[][] CreateBatchArrayFromList(List<Matrix4x4> matrices)
    {
        if (matrices.Count == 0) return new Matrix4x4[0][];
        
        int batchCount = Mathf.CeilToInt((float)matrices.Count / maxInstancesPerBatch);
        Matrix4x4[][] batches = new Matrix4x4[batchCount][];
        
        for (int i = 0; i < batchCount; i++)
        {
            int startIndex = i * maxInstancesPerBatch;
            int batchSize = Mathf.Min(maxInstancesPerBatch, matrices.Count - startIndex);
            
            batches[i] = new Matrix4x4[batchSize];
            
            for (int j = 0; j < batchSize; j++)
            {
                batches[i][j] = matrices[startIndex + j];
            }
        }
        
        return batches;
    }
    
    void StartManagementSystem()
    {
        if (lodManagementCoroutine != null)
        {
            StopCoroutine(lodManagementCoroutine);
        }
        
        if (shadowCoroutine != null)
        {
            StopCoroutine(shadowCoroutine);
        }
        
        lodManagementCoroutine = StartCoroutine(LODManagementLoop());
        shadowCoroutine = StartCoroutine(ShadowUpdateLoop());
        
        /*Debug.Log("Started management system coroutines");*/
    }
    
    void StopManagementSystem()
    {
        if (lodManagementCoroutine != null)
        {
            StopCoroutine(lodManagementCoroutine);
            lodManagementCoroutine = null;
        }
        
        if (shadowCoroutine != null)
        {
            StopCoroutine(shadowCoroutine);
            shadowCoroutine = null;
        }
        
        /*Debug.Log("Stopped management system coroutines");*/
    }
    
    IEnumerator LODManagementLoop()
    {
        while (true)
        {
            // Check for LOD state changes
            if (lastLODState != enableLODSystem)
            {
                /*Debug.Log($"LOD system changed to: {enableLODSystem}. Rebuilding rendering...");*/
                SetupRenderingForCurrentLOD();
            }
            
            yield return new WaitForSeconds(lodCheckInterval);
        }
    }
    
    IEnumerator ShadowUpdateLoop()
    {
        while (true)
        {
            UpdateShadowSettings();
            yield return new WaitForSeconds(1f); // Update shadows less frequently
        }
    }
    
    // Main Update method - renders every frame
    void Update()
    {
        // Render grass every frame (required for Graphics.DrawMeshInstanced)
        if (activeBatches != null && activeMesh != null && targetCamera != null && grassMaterial != null)
        {
            RenderGrass();
        }
    }
    
    void RenderGrass()
    {
        lastRenderedCount = 0;
        lastTriangleCount = 0;
        
        if (enableFrustumCulling)
        {
            GeometryUtility.CalculateFrustumPlanes(targetCamera, cameraFrustumPlanes);
        }
        
        Vector3 cameraPos = targetCamera.transform.position;
        
        if (enableLODSystem)
        {
            RenderWithLOD(cameraPos);
        }
        else
        {
            RenderSimple(cameraPos);
        }
        
        /*// Debug info occasionally
        if (showPerformanceInfo && Time.frameCount % 300 == 0) // Every 5 seconds
        {
            Debug.Log($"Rendered {lastRenderedCount} instances, {lastTriangleCount} triangles");
        }*/
    }
    
    void RenderWithLOD(Vector3 cameraPos)
    {
        foreach (Matrix4x4[] batch in activeBatches)
        {
            if (ShouldRenderBatch(batch, cameraPos))
            {
                // Determine mesh based on distance to first grass in batch
                Mesh meshToUse = GetLODMeshForBatch(batch, cameraPos);
                
                Graphics.DrawMeshInstanced(meshToUse, 0, grassMaterial, batch, batch.Length);
                lastRenderedCount += batch.Length;
                lastTriangleCount += batch.Length * (meshToUse.triangles.Length / 3);
            }
        }
    }
    
    void RenderSimple(Vector3 cameraPos)
    {
        foreach (Matrix4x4[] batch in activeBatches)
        {
            if (ShouldRenderBatch(batch, cameraPos))
            {
                Graphics.DrawMeshInstanced(activeMesh, 0, grassMaterial, batch, batch.Length);
                lastRenderedCount += batch.Length;
                lastTriangleCount += batch.Length * (activeMesh.triangles.Length / 3);
            }
        }
    }
    
    Mesh GetLODMeshForBatch(Matrix4x4[] batch, Vector3 cameraPos)
    {
        if (batch.Length == 0) return grassMeshLOD0;
        
        // Use first grass position to determine LOD
        Vector3 grassPos = new Vector3(batch[0].m03, batch[0].m13, batch[0].m23);
        float distance = Vector3.Distance(cameraPos, grassPos);
        
        if (distance <= lodDistance0)
            return grassMeshLOD0;
        else if (distance <= lodDistance1)
            return grassMeshLOD1 ?? grassMeshLOD0;
        else if (distance <= lodDistance2)
            return grassMeshLOD2 ?? grassMeshLOD1 ?? grassMeshLOD0;
        else
            return grassMeshLOD2 ?? grassMeshLOD1 ?? grassMeshLOD0;
    }
    
    bool ShouldRenderBatch(Matrix4x4[] batch, Vector3 cameraPos)
    {
        foreach (Matrix4x4 matrix in batch)
        {
            Vector3 grassPos = new Vector3(matrix.m03, matrix.m13, matrix.m23);
            
            if (enableDistanceCulling)
            {
                float distance = Vector3.Distance(cameraPos, grassPos);
                if (distance > maxRenderDistance) continue;
            }
            
            if (enableFrustumCulling)
            {
                Bounds grassBounds = new Bounds(grassPos, Vector3.one * grassScale);
                if (!GeometryUtility.TestPlanesAABB(cameraFrustumPlanes, grassBounds))
                    continue;
            }
            
            return true;
        }
        
        return false;
    }
    
    // Shadow system
    void ApplyPreferredShadowSettings()
    {
        shadowIntensity = 0.9f;
        useSoftShadows = true;
        shadowSoftness = 5.54f;
        shadowBias = 0.6f;
        normalBias = 0.15f;
        
        ApplyShadowSettings();
    }
    
    void ApplyShadowSettings()
    {
        Light mainLight = FindMainDirectionalLight();
        
        if (mainLight != null)
        {
            mainLight.shadows = useSoftShadows ? LightShadows.Soft : LightShadows.Hard;
            mainLight.shadowStrength = shadowIntensity;
            mainLight.shadowBias = shadowBias;
            mainLight.shadowNormalBias = normalBias;
            
            if (useSoftShadows)
            {
                mainLight.shadowResolution = shadowSoftness > 5f ? 
                    UnityEngine.Rendering.LightShadowResolution.VeryHigh : 
                    UnityEngine.Rendering.LightShadowResolution.High;
            }
        }
        
        ApplyMaterialShadowSettings();
    }
    
    void UpdateShadowSettings()
    {
        Light mainLight = FindMainDirectionalLight();
        if (mainLight != null)
        {
            bool needsUpdate = false;
            
            if (Mathf.Abs(mainLight.shadowStrength - shadowIntensity) > 0.01f) needsUpdate = true;
            if ((useSoftShadows && mainLight.shadows != LightShadows.Soft) || 
                (!useSoftShadows && mainLight.shadows != LightShadows.Hard)) needsUpdate = true;
                
            if (needsUpdate)
            {
                ApplyShadowSettings();
            }
        }
    }
    
    Light FindMainDirectionalLight()
    {
        Light[] lights = FindObjectsOfType<Light>();
        foreach (Light light in lights)
        {
            if (light.type == LightType.Directional && light.enabled)
                return light;
        }
        return null;
    }
    
    void ApplyMaterialShadowSettings()
    {
        if (grassMaterial != null)
        {
            if (grassMaterial.HasProperty("_ShadowIntensity"))
                grassMaterial.SetFloat("_ShadowIntensity", shadowIntensity);
            if (grassMaterial.HasProperty("_ShadowSoftness"))
                grassMaterial.SetFloat("_ShadowSoftness", shadowSoftness);
                
            // Fix shiny grass issue
            if (grassMaterial.HasProperty("_Metallic"))
                grassMaterial.SetFloat("_Metallic", 0f);
            if (grassMaterial.HasProperty("_Smoothness"))
                grassMaterial.SetFloat("_Smoothness", 0.1f);
            if (grassMaterial.HasProperty("_Glossiness"))
                grassMaterial.SetFloat("_Glossiness", 0.1f);
        }
    }
    
    // Public methods for runtime control
    [ContextMenu("Toggle LOD System")]
    public void ToggleLODSystem()
    {
        enableLODSystem = !enableLODSystem;
        /*Debug.Log($"LOD System: {(enableLODSystem ? "Enabled" : "Disabled")}");*/
        // The LOD management coroutine will handle the switch automatically
    }
    
    [ContextMenu("Restart Management System")]
    public void RestartManagementSystem()
    {
        StopManagementSystem();
        SetupRenderingForCurrentLOD();
        StartManagementSystem();
        /*Debug.Log("Management system restarted");*/
    }
    
    /*[ContextMenu("Debug Rendering State")]
    public void DebugRenderingState()
    {
        Debug.Log($"=== Grass Manager Debug ===");
        Debug.Log($"Enable LOD System: {enableLODSystem}");
        Debug.Log($"Last LOD State: {lastLODState}");
        Debug.Log($"Is Management Active: {(lodManagementCoroutine != null)}");
        Debug.Log($"Total Grass Matrices: {allGrassMatrices.Count}");
        Debug.Log($"Active Batches: {(activeBatches != null ? activeBatches.Length.ToString() : "NULL")}");
        Debug.Log($"Active Mesh: {(activeMesh != null ? activeMesh.name : "NULL")}");
        Debug.Log($"Grass Material: {(grassMaterial != null ? grassMaterial.name : "NULL")}");
        Debug.Log($"Target Camera: {(targetCamera != null ? targetCamera.name : "NULL")}");
        Debug.Log($"Last Rendered Count: {lastRenderedCount}");
        Debug.Log($"LOD Coroutine Active: {(lodManagementCoroutine != null)}");
        Debug.Log($"=========================");
    }*/
    
    [ContextMenu("Clear Grass")]
    public void ClearGrass()
    {
        allGrassMatrices.Clear();
        allGrassPositions.Clear();
        activeBatches = null;
        activeMesh = null;
        /*Debug.Log("Cleared all grass");*/
    }
    
    void OnDrawGizmosSelected()
    {
        Vector3 center = transform.position;
        if (groundPlane != null)
        {
            center = new Vector3(groundPlane.position.x, groundHeight, groundPlane.position.z);
        }
        
        // Draw spawn area only
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, new Vector3(spawnAreaSize, 0.1f, spawnAreaSize));
        
        // Draw grass positions if enabled
        /*if (/*showGrassPositions &&#1# allGrassPositions.Count > 0)
        {
            Gizmos.color = Color.white;
            int maxDraw = Mathf.Min(50, allGrassPositions.Count);
            for (int i = 0; i < maxDraw; i++)
            {
                Gizmos.DrawWireSphere(allGrassPositions[i], 0.1f);
            }
        }*/
    }
    
    // Public properties for debugging
    public int TotalGrassCount => allGrassMatrices.Count;
    public int RenderedGrassCount => lastRenderedCount;
    public int TriangleCount => lastTriangleCount;

}