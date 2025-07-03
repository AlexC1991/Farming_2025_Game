using UnityEngine;
using System.Collections.Generic;

namespace farming2025
{
    public class FenceBuilder : MonoBehaviour
    {
        [Header("Build Settings")]
        [SerializeField] private Camera playerCamera;
        [SerializeField] private LayerMask fenceLayerMask = -1;
        [SerializeField] private LayerMask placeholderLayerMask = -1;
        [SerializeField] private Material ghostBuildMaterial;
        [SerializeField] private float buildRange = 10f;
        [SerializeField] private KeyCode buildModeKey = KeyCode.B;
        [SerializeField] private KeyCode cancelBuildKey = KeyCode.Escape;
        [SerializeField] private KeyCode replacePlaceholderKey = KeyCode.R;
        
        [Header("Debug Settings")]
        [SerializeField] private bool enableDebugRays = true;
        [SerializeField] private bool enableVerboseLogging = true;
        
        [Header("Debug Info")]
        [SerializeField] private bool isInBuildMode = false;
        [SerializeField] private GameObject selectedFence;
        [SerializeField] private Vector3 buildDirection;
        [SerializeField] private GameObject ghostPreview;
        [SerializeField] private GameObject placeholderGhostPreview; // NEW: Separate ghost for placeholders
        [SerializeField] private string lastRaycastHit = "None";
        [SerializeField] private float lastRaycastDistance = 0f;
        [SerializeField] private bool raycastHitSomething = false;
        [SerializeField] private GameObject hoveredPlaceholder;
        [SerializeField] private bool canReplacePlaceholder = false;
        
        private ArrayModifier selectedArrayModifier;
        private FenceExtender selectedFenceExtender;
        private Vector3 lastMousePosition;
        private bool isDragging = false;
        
        private void Update()
        {
            HandleBuildModeToggle();
            
            if (isInBuildMode)
            {
                HandleBuildInput();
                UpdateGhostPreview();
                DebugRaycast();
                CheckForPlaceholder();
            }
        }
        
        private void CheckForPlaceholder()
        {
            // Clear previous placeholder ghost
            if (placeholderGhostPreview != null && !canReplacePlaceholder)
            {
                Destroy(placeholderGhostPreview);
                placeholderGhostPreview = null;
                hoveredPlaceholder = null;
            }
            
            if (selectedFence == null) return;
            
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            // Check for placeholder with a more specific raycast
            LayerMask combinedMask = fenceLayerMask | placeholderLayerMask;
            if (Physics.Raycast(ray, out hit, buildRange, combinedMask))
            {
                GameObject hitObject = hit.collider.gameObject;
                
                // Check if this is a placeholder
                if (IsPlaceholderObject(hitObject))
                {
                    FenceChild fenceChild = hitObject.GetComponent<FenceChild>();
                    
                    // Verify it belongs to our selected fence system
                    if (fenceChild != null && fenceChild.IsPlaceholder && 
                        IsChildOfSelectedFence(fenceChild))
                    {
                        hoveredPlaceholder = hitObject;
                        canReplacePlaceholder = true;
                        CreatePlaceholderGhostPreview(hitObject.transform.position);
                        
                        if (enableVerboseLogging)
                        {
                            Debug.Log($"[FenceBuilder] Hovering over replaceable placeholder: {hitObject.name}");
                        }
                        return;
                    }
                }
            }
            
            // Clear placeholder tracking if nothing valid found
            canReplacePlaceholder = false;
        }
        
        private void CreatePlaceholderGhostPreview(Vector3 position)
        {
            // Don't create duplicate ghost previews
            if (placeholderGhostPreview != null) return;
            
            if (selectedFence != null)
            {
                placeholderGhostPreview = Instantiate(selectedFence);
                placeholderGhostPreview.transform.position = position;
                
                // Remove all scripts from ghost
                Component[] components = placeholderGhostPreview.GetComponents<Component>();
                foreach (Component comp in components)
                {
                    if (!(comp is Transform) && !(comp is MeshRenderer) && !(comp is MeshFilter))
                    {
                        Destroy(comp);
                    }
                }
                
                // Set special ghost material for placeholder replacement
                MeshRenderer renderer = placeholderGhostPreview.GetComponent<MeshRenderer>();
                if (renderer != null && ghostBuildMaterial != null)
                {
                    Material placeholderGhostMaterial = new Material(ghostBuildMaterial);
                    placeholderGhostMaterial.color = Color.green; // Green ghost for placeholder replacement
                    placeholderGhostMaterial.SetFloat("_Metallic", 0.3f);
                    placeholderGhostMaterial.SetFloat("_Glossiness", 0.7f);
                    renderer.material = placeholderGhostMaterial;
                }
                
                placeholderGhostPreview.name = "PlaceholderGhostPreview";
                
                // Add a slight bob animation
                StartCoroutine(AnimatePlaceholderGhost());
                
                Debug.Log($"[FenceBuilder] Created placeholder ghost preview at {position}");
            }
        }
        
        private System.Collections.IEnumerator AnimatePlaceholderGhost()
        {
            Vector3 originalPosition = placeholderGhostPreview.transform.position;
            float time = 0f;
            
            while (placeholderGhostPreview != null)
            {
                time += Time.deltaTime;
                float yOffset = Mathf.Sin(time * 3f) * 0.1f; // Gentle bobbing
                placeholderGhostPreview.transform.position = originalPosition + Vector3.up * yOffset;
                yield return null;
            }
        }
        
        private bool IsPlaceholderObject(GameObject obj)
        {
            // Check multiple ways to identify placeholder
            bool isOnPlaceholderLayer = obj.layer == LayerMask.NameToLayer("PlaceholderFence");
            bool hasFenceChildWithPlaceholderFlag = false;
            
            FenceChild fenceChild = obj.GetComponent<FenceChild>();
            if (fenceChild != null)
            {
                hasFenceChildWithPlaceholderFlag = fenceChild.IsPlaceholder;
            }
            
            bool nameContainsPlaceholder = obj.name.ToLower().Contains("placeholder");
            
            return isOnPlaceholderLayer || hasFenceChildWithPlaceholderFlag || nameContainsPlaceholder;
        }
        
        private bool IsChildOfSelectedFence(FenceChild fenceChild)
        {
            if (selectedArrayModifier != null)
            {
                return fenceChild.ParentFence == selectedFence.transform;
            }
            else if (selectedFenceExtender != null)
            {
                return fenceChild.ParentFence == selectedFence.transform;
            }
            return false;
        }
        
        private void DebugRaycast()
        {
            if (!enableVerboseLogging) return;
            
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            // Combine layer masks for raycast
            LayerMask combinedMask = fenceLayerMask | placeholderLayerMask;
            
            // Cast ray and update debug info
            if (Physics.Raycast(ray, out hit, buildRange, combinedMask))
            {
                raycastHitSomething = true;
                lastRaycastHit = hit.collider.gameObject.name;
                lastRaycastDistance = hit.distance;
                
                // Draw debug ray in scene view
                if (enableDebugRays)
                {
                    Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green, 0.1f);
                    Debug.DrawRay(hit.point, Vector3.up * 0.5f, Color.yellow, 0.1f);
                }
            }
            else
            {
                raycastHitSomething = false;
                lastRaycastHit = "None";
                lastRaycastDistance = 0f;
                
                // Draw debug ray showing full range
                if (enableDebugRays)
                {
                    Debug.DrawRay(ray.origin, ray.direction * buildRange, Color.red, 0.1f);
                }
            }
        }
        
        private void HandleBuildModeToggle()
        {
            if (Input.GetKeyDown(buildModeKey))
            {
                ToggleBuildMode();
            }
            
            if (Input.GetKeyDown(cancelBuildKey) && isInBuildMode)
            {
                ExitBuildMode();
            }
        }
        
        private void ToggleBuildMode()
        {
            isInBuildMode = !isInBuildMode;
            
            if (isInBuildMode)
            {
                Debug.Log("[FenceBuilder] Entered build mode. Click a fence to start building.");
                Debug.Log($"[FenceBuilder] LayerMask: {fenceLayerMask.value}, Build Range: {buildRange}");
            }
            else
            {
                ExitBuildMode();
            }
        }
        
        private void ExitBuildMode()
        {
            isInBuildMode = false;
            selectedFence = null;
            selectedArrayModifier = null;
            selectedFenceExtender = null;
            buildDirection = Vector3.zero;
            isDragging = false;
            hoveredPlaceholder = null;
            canReplacePlaceholder = false;
            
            if (ghostPreview != null)
            {
                Destroy(ghostPreview);
            }
            
            if (placeholderGhostPreview != null)
            {
                Destroy(placeholderGhostPreview);
            }
            
            Debug.Log("[FenceBuilder] Exited build mode.");
        }
        
        private void HandleBuildInput()
        {
            // Left click to select fence, place fence, or replace placeholder
            if (Input.GetMouseButtonDown(0))
            {
                if (selectedFence == null)
                {
                    TrySelectFence();
                }
                else if (canReplacePlaceholder && hoveredPlaceholder != null)
                {
                    ReplacePlaceholderWithFence();
                }
                else
                {
                    TryPlaceOrRemoveFence();
                }
            }
            
            // R key to replace placeholder (alternative method)
            if (Input.GetKeyDown(replacePlaceholderKey) && canReplacePlaceholder && hoveredPlaceholder != null)
            {
                ReplacePlaceholderWithFence();
            }
            
            // Right click to remove fence
            if (Input.GetMouseButtonDown(1) && selectedFence != null)
            {
                TryRemoveFence();
            }
            
            // Track mouse movement for direction
            Vector3 currentMousePosition = Input.mousePosition;
            if (lastMousePosition != currentMousePosition)
            {
                UpdateBuildDirection();
                lastMousePosition = currentMousePosition;
            }
        }
        
        private void ReplacePlaceholderWithFence()
        {
            if (hoveredPlaceholder == null) return;
            
            Vector3 placeholderPosition = hoveredPlaceholder.transform.position;
            FenceChild fenceChild = hoveredPlaceholder.GetComponent<FenceChild>();
            
            if (fenceChild != null)
            {
                Debug.Log($"[FenceBuilder] Replacing placeholder with real fence at {placeholderPosition}");
                
                // Get the ArrayModifier that owns this placeholder
                ArrayModifier parentArrayMod = fenceChild.ParentFence.GetComponent<ArrayModifier>();
                
                if (parentArrayMod != null)
                {
                    // FIRST: Create new fence using the parent ArrayModifier
                    GameObject newFence = parentArrayMod.CreateFence(
                        placeholderPosition, 
                        $"Fence_{fenceChild.Direction}_{fenceChild.IndexInDirection}"
                    );
                    
                    if (newFence != null)
                    {
                        // Add FenceChild component to new fence
                        FenceChild newChild = newFence.AddComponent<FenceChild>();
                        newChild.Initialize(
                            fenceChild.ParentFence, 
                            fenceChild.Direction, 
                            fenceChild.IndexInDirection, 
                            false // Not a placeholder
                        );
                        
                        // THEN: Replace the placeholder reference in the fence list
                        parentArrayMod.ReplacePlaceholderWithFence(
                            fenceChild.Direction, 
                            fenceChild.IndexInDirection, 
                            newFence
                        );
                        
                        // FINALLY: Clean up the placeholder
                        hoveredPlaceholder = null;
                        canReplacePlaceholder = false;
                        
                        if (placeholderGhostPreview != null)
                        {
                            Destroy(placeholderGhostPreview);
                            placeholderGhostPreview = null;
                        }
                        
                        // Destroy the placeholder GameObject
                        Destroy(fenceChild.gameObject);
                        
                        Debug.Log($"[FenceBuilder] Successfully replaced placeholder with fence");
                    }
                }
                else
                {
                    Debug.LogError("[FenceBuilder] Could not find parent ArrayModifier for placeholder replacement");
                }
            }
        }
        
        private void TrySelectFence()
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            Debug.Log($"[FenceBuilder] Attempting to select fence...");
            
            if (Physics.Raycast(ray, out hit, buildRange, fenceLayerMask))
            {
                GameObject hitObject = hit.collider.gameObject;
                Debug.Log($"[FenceBuilder] Raycast HIT: {hitObject.name} at distance {hit.distance}");
                
                // Check if it's a fence with ArrayModifier or FenceExtender
                ArrayModifier arrayMod = hitObject.GetComponent<ArrayModifier>();
                FenceExtender fenceExt = hitObject.GetComponent<FenceExtender>();
                
                if (arrayMod != null || fenceExt != null)
                {
                    selectedFence = hitObject;
                    selectedArrayModifier = arrayMod;
                    selectedFenceExtender = fenceExt;
                    
                    Debug.Log($"[FenceBuilder] ✓ Selected fence: {selectedFence.name}");
                    Debug.Log("[FenceBuilder] Move mouse to choose direction, then click to place fence or hover over empty spaces to replace with fence.");
                }
            }
        }
        
        private void UpdateBuildDirection()
        {
            if (selectedFence == null) return;
            
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, selectedFence.transform.position);
            
            if (groundPlane.Raycast(ray, out float distance))
            {
                Vector3 worldPoint = ray.GetPoint(distance);
                Vector3 directionVector = (worldPoint - selectedFence.transform.position).normalized;
                
                // Snap to cardinal directions
                Vector3 newDirection = GetCardinalDirection(directionVector);
                
                if (newDirection != buildDirection)
                {
                    buildDirection = newDirection;
                    if (enableVerboseLogging)
                    {
                        Debug.Log($"[FenceBuilder] Build direction changed to: {buildDirection}");
                    }
                }
            }
        }
        
        private Vector3 GetCardinalDirection(Vector3 direction)
        {
            float x = Mathf.Abs(direction.x);
            float z = Mathf.Abs(direction.z);
            
            if (x > z)
            {
                return direction.x > 0 ? Vector3.right : Vector3.left;
            }
            else
            {
                return direction.z > 0 ? Vector3.forward : Vector3.back;
            }
        }
        
        private void UpdateGhostPreview()
        {
            // Don't show regular ghost preview if we're hovering over a placeholder
            if (canReplacePlaceholder)
            {
                if (ghostPreview != null)
                {
                    ghostPreview.SetActive(false);
                }
                return;
            }
            
            if (selectedFence == null || buildDirection == Vector3.zero) 
            {
                if (ghostPreview != null)
                {
                    ghostPreview.SetActive(false);
                }
                return;
            }
            
            Vector3 nextPosition = GetNextFencePosition();
            
            if (nextPosition != Vector3.zero)
            {
                if (ghostPreview == null)
                {
                    CreateGhostPreview();
                }
                
                ghostPreview.transform.position = nextPosition;
                ghostPreview.SetActive(true);
            }
            else
            {
                if (ghostPreview != null)
                {
                    ghostPreview.SetActive(false);
                }
            }
        }
        
        private void CreateGhostPreview()
        {
            if (selectedFence != null)
            {
                ghostPreview = Instantiate(selectedFence);
                
                // Remove all scripts from ghost
                Component[] components = ghostPreview.GetComponents<Component>();
                foreach (Component comp in components)
                {
                    if (!(comp is Transform) && !(comp is MeshRenderer) && !(comp is MeshFilter) && !(comp is Collider))
                    {
                        Destroy(comp);
                    }
                }
                
                // Set ghost material
                MeshRenderer renderer = ghostPreview.GetComponent<MeshRenderer>();
                if (renderer != null && ghostBuildMaterial != null)
                {
                    renderer.material = ghostBuildMaterial;
                }
                
                ghostPreview.name = "GhostPreview";
                Debug.Log($"[FenceBuilder] Created ghost preview at {ghostPreview.transform.position}");
            }
        }
        
        private Vector3 GetNextFencePosition()
        {
            if (selectedFence == null) return Vector3.zero;
            
            float spacing = GetSpacing();
            Vector3 basePosition = selectedFence.transform.position;
            
            // Find the furthest existing fence in the build direction
            Vector3 furthestPosition = FindFurthestFenceInDirection(basePosition, buildDirection, spacing);
            
            // Calculate next position from the furthest fence
            Vector3 nextPosition = furthestPosition + buildDirection * spacing;
            
            // Check if that position is occupied (but allow placeholders to be replaced)
            if (!IsPositionOccupied(nextPosition, spacing * 0.5f))
            {
                return nextPosition;
            }
            
            return Vector3.zero;
        }

        private Vector3 FindFurthestFenceInDirection(Vector3 startPosition, Vector3 direction, float spacing)
        {
            Vector3 furthestPosition = startPosition;
            float furthestDistance = 0f;
            
            // Get all child fences from the selected fence system
            List<GameObject> allFences = GetAllFencesFromSelected();
            
            foreach (GameObject fence in allFences)
            {
                if (fence == null) continue;
                
                Vector3 fencePos = fence.transform.position;
                Vector3 directionToFence = (fencePos - startPosition).normalized;
                
                // Check if this fence is in the same direction we're building
                float dot = Vector3.Dot(directionToFence, direction);
                if (dot > 0.8f) // Same direction (allowing some tolerance)
                {
                    float distance = Vector3.Distance(startPosition, fencePos);
                    if (distance > furthestDistance)
                    {
                        furthestDistance = distance;
                        furthestPosition = fencePos;
                    }
                }
            }
            
            return furthestPosition;
        }

        private List<GameObject> GetAllFencesFromSelected()
        {
            List<GameObject> fences = new List<GameObject>();
            
            if (selectedArrayModifier != null)
            {
                fences.Add(selectedFence);
                List<GameObject> childFences = selectedArrayModifier.GetAllChildFences();
                fences.AddRange(childFences);
            }
            else if (selectedFenceExtender != null)
            {
                Collider[] nearbyColliders = Physics.OverlapSphere(
                    selectedFence.transform.position, 
                    GetSpacing() * 20f,
                    fenceLayerMask
                );
                
                foreach (Collider col in nearbyColliders)
                {
                    GameObject fence = col.gameObject;
                    if (fence != null && !fences.Contains(fence))
                    {
                        FenceChild fenceChild = fence.GetComponent<FenceChild>();
                        if (fenceChild != null && fenceChild.ParentFence == selectedFence.transform)
                        {
                            fences.Add(fence);
                        }
                        else if (fence == selectedFence)
                        {
                            fences.Add(fence);
                        }
                    }
                }
            }
            
            return fences;
        }
        
        private bool IsPositionOccupied(Vector3 position, float tolerance)
        {
            Collider[] colliders = Physics.OverlapSphere(position, tolerance, fenceLayerMask);
            foreach (Collider col in colliders)
            {
                if (col.gameObject != ghostPreview && col.gameObject != placeholderGhostPreview)
                {
                    return true;
                }
            }
            return false;
        }
        
        private float GetSpacing()
        {
            if (selectedArrayModifier != null)
            {
                return selectedArrayModifier.GetSpacing();
            }
            else if (selectedFenceExtender != null)
            {
                return selectedFenceExtender.GetSpacing();
            }
            return 0.5f; // Default
        }
        
        private void TryPlaceOrRemoveFence()
        {
            Vector3 targetPosition = GetNextFencePosition();
            
            if (targetPosition != Vector3.zero)
            {
                PlaceFence(targetPosition);
            }
            else
            {
                Debug.Log("[FenceBuilder] No valid position found for fence placement");
            }
        }
        
        private void PlaceFence(Vector3 position)
        {
            if (selectedArrayModifier != null)
            {
                selectedArrayModifier.PlaceFenceAtPosition(position, buildDirection);
                Debug.Log($"[FenceBuilder] Placed fence via ArrayModifier at {position}");
            }
            else if (selectedFenceExtender != null)
            {
                selectedFenceExtender.PlaceFenceAtPosition(position, buildDirection);
                Debug.Log($"[FenceBuilder] Placed fence via FenceExtender at {position}");
            }
        }
        
        private void TryRemoveFence()
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            LayerMask combinedMask = fenceLayerMask | placeholderLayerMask;
            
            if (Physics.Raycast(ray, out hit, buildRange, combinedMask))
            {
                GameObject hitObject = hit.collider.gameObject;
                Debug.Log($"[FenceBuilder] Right-clicked on: {hitObject.name}");
                
                if (hitObject != selectedFence)
                {
                    RemoveFence(hitObject);
                }
            }
        }
        
        private void RemoveFence(GameObject fence)
        {
            FenceChild fenceChild = fence.GetComponent<FenceChild>();
            if (fenceChild != null)
            {
                Debug.Log($"[FenceBuilder] Manually removing fence: {fence.name}");
                Destroy(fence);
            }
            else
            {
                Debug.Log($"[FenceBuilder] Cannot remove fence '{fence.name}' - it's not a child fence.");
            }
        }
        
        private void OnGUI()
        {
            if (isInBuildMode)
            {
                GUILayout.BeginArea(new Rect(10, 10, 450, 350));
                GUILayout.Box("FENCE BUILD MODE - DEBUG");
                
                if (selectedFence == null)
                {
                    GUILayout.Label("Click a fence to start building");
                    GUILayout.Label($"Raycast Hit: {(raycastHitSomething ? "YES" : "NO")}");
                    if (raycastHitSomething)
                    {
                        GUILayout.Label($"Hit Object: {lastRaycastHit}");
                        GUILayout.Label($"Distance: {lastRaycastDistance:F2}");
                    }
                }
                else
                {
                    GUILayout.Label($"Selected: {selectedFence.name}");
                    GUILayout.Label($"Direction: {buildDirection}");
                    GUILayout.Label("Left Click: Place fence");
                    GUILayout.Label("Right Click: Remove fence");
                    
                    if (canReplacePlaceholder && hoveredPlaceholder != null)
                    {
                        GUIStyle highlightStyle = new GUIStyle(GUI.skin.label);
                        highlightStyle.normal.textColor = Color.green;
                        highlightStyle.fontStyle = FontStyle.Bold;
                        
                        GUILayout.Label($"PLACEHOLDER DETECTED!", highlightStyle);
                        GUILayout.Label($"Green ghost shows replacement fence");
                        GUILayout.Label($"Press {replacePlaceholderKey} or Left Click to replace!");
                    }
                }
                
                GUILayout.Label($"Fence LayerMask: {fenceLayerMask.value}");
                GUILayout.Label($"Placeholder LayerMask: {placeholderLayerMask.value}");
                GUILayout.Label($"Build Range: {buildRange}");
                GUILayout.Label($"Press {cancelBuildKey} to exit");
                GUILayout.EndArea();
            }
            else
            {
                GUILayout.BeginArea(new Rect(10, 10, 200, 50));
                GUILayout.Label($"Press {buildModeKey} for Build Mode");
                GUILayout.EndArea();
            }
        }
    }
}