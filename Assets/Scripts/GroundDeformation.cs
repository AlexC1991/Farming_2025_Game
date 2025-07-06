using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class GroundDeformation : MonoBehaviour
{
    [Header("Mesh Generation")]
    public int meshResolution = 32;
    public float meshSize = 5f;
    
    [Header("Deformation Settings")]
    public float deformationStrength = 0.5f;
    public float deformationRadius = 1.0f;
    public AnimationCurve deformationFalloff = AnimationCurve.Linear(0, 1, 1, 0);
    
    [Header("Visual Settings")]
    public Material mudMaterial;
    public Color baseColor = new Color(0.6f, 0.4f, 0.2f, 1f);
    public Color deformedColor = new Color(0.3f, 0.2f, 0.1f, 1f);
    
    [Header("Mouse Controls")]
    public bool enableMouseDeformation = true;
    public KeyCode deformationKey = KeyCode.Mouse0;
    
    [Header("Debug")]
    [Space]
    [Button("Regenerate Mesh")]
    public bool regenerateMesh;
    
    private Mesh proceduralMesh;
    private Vector3[] originalVertices;
    private Vector3[] currentVertices;
    private Color[] vertexColors;
    private int[] triangles;
    private Vector2[] uvs;
    
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    
    void Start()
    {
        InitializeComponents();
        GenerateGroundMesh();
    }
    
    void OnValidate()
    {
        if (regenerateMesh)
        {
            regenerateMesh = false;
            if (Application.isPlaying)
            {
                GenerateGroundMesh();
            }
        }
    }
    
    void InitializeComponents()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        
        if (mudMaterial != null)
        {
            meshRenderer.material = mudMaterial;
        }
    }
    
    void GenerateGroundMesh()
    {
        proceduralMesh = new Mesh();
        proceduralMesh.name = "DeformableGround";
        proceduralMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        
        // Calculate vertex count for single surface
        int vertexCount = (meshResolution + 1) * (meshResolution + 1);
        
        // Initialize arrays
        originalVertices = new Vector3[vertexCount];
        currentVertices = new Vector3[vertexCount];
        vertexColors = new Color[vertexCount];
        uvs = new Vector2[vertexCount];
        
        // Generate vertices for single horizontal surface
        for (int z = 0; z <= meshResolution; z++)
        {
            for (int x = 0; x <= meshResolution; x++)
            {
                int index = z * (meshResolution + 1) + x;
                
                // Generate vertices in LOCAL space from -meshSize/2 to +meshSize/2
                float xPos = ((float)x / meshResolution - 0.5f) * meshSize;
                float zPos = ((float)z / meshResolution - 0.5f) * meshSize;
                
                // Single horizontal surface at Y = 0
                originalVertices[index] = new Vector3(xPos, 0f, zPos);
                currentVertices[index] = originalVertices[index];
                vertexColors[index] = baseColor;
                uvs[index] = new Vector2((float)x / meshResolution, (float)z / meshResolution);
            }
        }
        
        GenerateTriangles();
        
        // Apply to mesh
        proceduralMesh.vertices = currentVertices;
        proceduralMesh.triangles = triangles;
        proceduralMesh.uv = uvs;
        proceduralMesh.colors = vertexColors;
        proceduralMesh.RecalculateNormals();
        proceduralMesh.RecalculateBounds();
        
        // Assign to components
        meshFilter.mesh = proceduralMesh;
        meshCollider.sharedMesh = proceduralMesh;
        
        Debug.Log($"Generated ground mesh with {vertexCount} vertices and {triangles.Length/3} triangles");
        Debug.Log($"Mesh bounds: {proceduralMesh.bounds}");
    }
    
    void GenerateTriangles()
    {
        int triangleCount = meshResolution * meshResolution * 6; // 6 indices per quad
        triangles = new int[triangleCount];
        int triangleIndex = 0;
        
        // Generate triangles for single surface
        for (int y = 0; y < meshResolution; y++)
        {
            for (int x = 0; x < meshResolution; x++)
            {
                int bottomLeft = y * (meshResolution + 1) + x;
                int bottomRight = bottomLeft + 1;
                int topLeft = bottomLeft + (meshResolution + 1);
                int topRight = topLeft + 1;
                
                // First triangle (counter-clockwise for upward facing)
                triangles[triangleIndex] = bottomLeft;
                triangles[triangleIndex + 1] = topLeft;
                triangles[triangleIndex + 2] = topRight;
                
                // Second triangle
                triangles[triangleIndex + 3] = bottomLeft;
                triangles[triangleIndex + 4] = topRight;
                triangles[triangleIndex + 5] = bottomRight;
                
                triangleIndex += 6;
            }
        }
    }
    
    void UpdateMesh()
    {
        if (proceduralMesh == null)
        {
            Debug.LogError("Procedural mesh is null! Regenerating...");
            GenerateGroundMesh();
            return;
        }
        
        proceduralMesh.vertices = currentVertices;
        proceduralMesh.colors = vertexColors;
        proceduralMesh.RecalculateNormals();
        proceduralMesh.RecalculateBounds();
        
        meshFilter.mesh = proceduralMesh;
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = proceduralMesh;
    }
    
    void Update()
    {
        if (enableMouseDeformation && Input.GetKey(deformationKey))
        {
            HandleMouseDeformation();
        }
    }
    
    void HandleMouseDeformation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                Vector3 localHitPoint = transform.InverseTransformPoint(hit.point);
                DeformAtPosition(localHitPoint);
            }
        }
    }
    
    public void DeformAtPosition(Vector3 localPosition)
    {
        bool meshChanged = false;
        
        for (int i = 0; i < currentVertices.Length; i++)
        {
            // Calculate distance in XZ plane only
            float distance = Vector3.Distance(
                new Vector3(currentVertices[i].x, 0, currentVertices[i].z), 
                new Vector3(localPosition.x, 0, localPosition.z)
            );
            
            if (distance <= deformationRadius)
            {
                float normalizedDistance = distance / deformationRadius;
                float deformationAmount = deformationFalloff.Evaluate(normalizedDistance) * deformationStrength;
                
                // Apply deformation downward
                currentVertices[i].y = originalVertices[i].y - deformationAmount;
                
                // Update vertex color
                float colorBlend = deformationAmount / deformationStrength;
                vertexColors[i] = Color.Lerp(baseColor, deformedColor, colorBlend);
                
                meshChanged = true;
            }
        }
        
        if (meshChanged)
        {
            UpdateMesh();
        }
    }
    
    public void OnToolImpact(Vector3 worldPosition, float toolStrength = 1.0f, float toolRadius = 1.0f)
    {
        Vector3 localPos = transform.InverseTransformPoint(worldPosition);
        
        float originalStrength = deformationStrength;
        float originalRadius = deformationRadius;
        
        deformationStrength = toolStrength;
        deformationRadius = toolRadius;
        
        DeformAtPosition(localPos);
        
        deformationStrength = originalStrength;
        deformationRadius = originalRadius;
    }
    
    [ContextMenu("Reset Deformation")]
    public void ResetDeformation()
    {
        if (originalVertices != null && currentVertices != null)
        {
            System.Array.Copy(originalVertices, currentVertices, originalVertices.Length);
            
            for (int i = 0; i < vertexColors.Length; i++)
            {
                vertexColors[i] = baseColor;
            }
            
            UpdateMesh();
            Debug.Log("Deformation reset!");
        }
    }
    
    [ContextMenu("Regenerate Mesh")]
    public void RegenerateMesh()
    {
        GenerateGroundMesh();
        Debug.Log("Mesh regenerated!");
    }
    
    void OnDrawGizmosSelected()
    {
        // Draw the mesh bounds
        Gizmos.color = Color.yellow;
        Vector3 center = transform.position;
        Vector3 size = new Vector3(meshSize, 0.1f, meshSize);
        Gizmos.DrawWireCube(center, size);
        
        if (enableMouseDeformation)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(center, deformationRadius);
        }
        
        // Debug: Show vertex positions (limit to avoid performance issues)
        if (currentVertices != null && currentVertices.Length > 0)
        {
            Gizmos.color = Color.green;
            int step = Mathf.Max(1, currentVertices.Length / 100);
            for (int i = 0; i < currentVertices.Length; i += step)
            {
                Vector3 worldPos = transform.TransformPoint(currentVertices[i]);
                Gizmos.DrawSphere(worldPos, 0.05f);
            }
        }
    }
}

// Custom attribute for buttons in inspector
public class ButtonAttribute : PropertyAttribute
{
    public string MethodName { get; }
    
    public ButtonAttribute(string methodName)
    {
        MethodName = methodName;
    }
}