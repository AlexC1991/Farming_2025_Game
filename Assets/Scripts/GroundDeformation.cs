using UnityEngine;

public class GroundDeformation : MonoBehaviour
{
    [Header("Deformation Settings")]
    public float impactForce = 800f;
    public float impactRadius = 0.5f;
    public LayerMask toolLayerMask = -1; // What layers count as "tools"
    
    [Header("Mouse Controls")]
    public bool enableMouseDeformation = true;
    public KeyCode deformationKey = KeyCode.Mouse0;
    
    [Header("Visual Painting")]
    public bool enablePainting = true;
    public Color paintColor = Color.red;
    public float paintRadius = 1.0f;
    
    private RMD_Deformation rmdComponent;
    private Mesh originalMesh;
    private Color[] vertexColors;
    
    void Start()
    {
        // Get the RMD component
        rmdComponent = GetComponent<RMD_Deformation>();
        if (rmdComponent == null)
        {
            Debug.LogError("RMD_Deformation component not found! Please add it to this GameObject.");
        }
        
        // Initialize vertex painting system
        if (enablePainting)
        {
            InitializeVertexPainting();
        }
    }
    
    void InitializeVertexPainting()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null && meshFilter.mesh != null)
        {
            originalMesh = meshFilter.mesh;
            
            // Initialize vertex colors array
            vertexColors = new Color[originalMesh.vertexCount];
            
            // Set all vertices to white initially (or whatever base color you want)
            for (int i = 0; i < vertexColors.Length; i++)
            {
                vertexColors[i] = Color.white;
            }
            
            // Apply initial colors to mesh
            originalMesh.colors = vertexColors;
            
            Debug.Log($"Initialized vertex painting with {vertexColors.Length} vertices");
        }
        else
        {
            Debug.LogError("No MeshFilter or Mesh found for vertex painting!");
        }
    }
    
    void Update()
    {
        // Handle mouse-based deformation for testing
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
                // Create impact at mouse position
                CreateImpactAtPosition(hit.point, hit.normal, impactForce);
            }
        }
    }
    
    public void CreateImpactAtPosition(Vector3 worldPosition, Vector3 normal, float force)
    {
        // Paint on the mesh where impact occurs
        if (enablePainting)
        {
            PaintAtPosition(worldPosition);
        }
        
        // Create a temporary impact object that will trigger RMD deformation
        GameObject impactObj = new GameObject("GroundImpact");
        impactObj.transform.position = worldPosition + normal * 2f; // Higher starting position
        
        // Add sphere collider for impact area - make it bigger like your test sphere
        SphereCollider sphereCol = impactObj.AddComponent<SphereCollider>();
        sphereCol.radius = 0.5f; // Same size as your test sphere
        sphereCol.isTrigger = false;
        
        // Add rigidbody with similar settings to your successful sphere
        Rigidbody rb = impactObj.AddComponent<Rigidbody>();
        rb.mass = 1f; // Standard mass like your test sphere
        rb.linearDamping = 0f; // No drag for maximum impact
        rb.angularDamping = 0f; // No angular drag
        
        // Apply much stronger impact force - simulate a heavy falling object
        rb.AddForce(-normal * 1000f, ForceMode.Impulse); // Much stronger force
        
        // Destroy the impact object after collision
        Destroy(impactObj, 3.0f); // Give it more time to settle
    }
    
    void PaintAtPosition(Vector3 worldPosition)
    {
        if (originalMesh == null || vertexColors == null) return;
        
        Vector3 localPosition = transform.InverseTransformPoint(worldPosition);
        Vector3[] vertices = originalMesh.vertices;
        
        bool anyVertexPainted = false;
        
        // Check each vertex and paint if within radius
        for (int i = 0; i < vertices.Length; i++)
        {
            float distance = Vector3.Distance(vertices[i], localPosition);
            
            if (distance <= paintRadius)
            {
                // Calculate paint intensity based on distance (closer = more intense)
                float intensity = 1f - (distance / paintRadius);
                
                // Blend current color with paint color based on intensity
                vertexColors[i] = Color.Lerp(vertexColors[i], paintColor, intensity * 0.8f);
                anyVertexPainted = true;
            }
        }
        
        // Update the mesh with new colors
        if (anyVertexPainted)
        {
            originalMesh.colors = vertexColors;
        }
    }
    
    // Call this method from tool scripts when they hit the ground
    public void OnToolImpact(Vector3 impactPoint, Vector3 impactNormal, float toolForce)
    {
        CreateImpactAtPosition(impactPoint, impactNormal, toolForce);
    }
    
    // For use with farming tools like tractors, plows, etc.
    void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is a farming tool
        if (IsValidTool(other))
        {
            Vector3 impactPoint = other.ClosestPoint(transform.position);
            Vector3 impactNormal = Vector3.up; // Assume ground normal is up
            
            // Get tool force (you can add this as a component to tools)
            ToolImpact toolScript = other.GetComponent<ToolImpact>();
            float toolForce = toolScript != null ? toolScript.impactForce : impactForce;
            
            CreateImpactAtPosition(impactPoint, impactNormal, toolForce);
        }
    }
    
    bool IsValidTool(Collider other)
    {
        // Check if object is on the tool layer mask
        return (toolLayerMask.value & (1 << other.gameObject.layer)) > 0;
    }
    
    // Public method to reset the paint
    public void ClearPaint()
    {
        if (vertexColors != null)
        {
            for (int i = 0; i < vertexColors.Length; i++)
            {
                vertexColors[i] = Color.white;
            }
            originalMesh.colors = vertexColors;
        }
    }
    
    // Public method to change paint color during runtime
    public void SetPaintColor(Color newColor)
    {
        paintColor = newColor;
    }
}

// Simple component to add to farming tools - must be separate from GroundDeformation class
public class ToolImpact : MonoBehaviour
{
    public float impactForce = 3.0f;
    public ToolType toolType = ToolType.Spade;
}

public enum ToolType
{
    Spade,
    Plow,
    Tractor,
    Hoe,
    Rake
}