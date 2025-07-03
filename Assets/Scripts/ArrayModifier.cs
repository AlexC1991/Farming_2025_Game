using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace farming2025
{
    public class ArrayModifier : MonoBehaviour
    {
        [Header("Directional Counts")]
        [SerializeField] private int countPositiveX = 1;
        [SerializeField] private int countNegativeX = 1;
        [SerializeField] private int countPositiveZ = 1;
        [SerializeField] private int countNegativeZ = 1;
        
        [Header("Settings")]
        [SerializeField] private float spacing = 0.5f;
        [SerializeField] private Material ghostMaterial;
        [SerializeField] private Material badMaterial;
        [SerializeField] private GameObject placeholderPrefab;
        
        [Header("Settings Tracker")]
        [SerializeField] private FenceSettingsTracker fenceSettingsTracker;
        
        private GameObject _arrayItem;
        private Material _itemDefaultMaterial;
        private const int MAX_FENCE_POSTS = 20;
        
        // Separate lists for each direction
        private List<GameObject> posXFences = new List<GameObject>();
        private List<GameObject> negXFences = new List<GameObject>();
        private List<GameObject> posZFences = new List<GameObject>();
        private List<GameObject> negZFences = new List<GameObject>();
        
        // Track end fences that have FenceExtender
        private List<GameObject> endFencesWithExtenders = new List<GameObject>();

        private void Awake()
        {
            _arrayItem = this.gameObject;
            _itemDefaultMaterial = this.gameObject.GetComponent<MeshRenderer>().material;
            
            // Create settings tracker if none assigned
            if (fenceSettingsTracker == null)
            {
                fenceSettingsTracker = ScriptableObject.CreateInstance<FenceSettingsTracker>();
            }
            
            // Create placeholder prefab if none assigned
            if (placeholderPrefab == null)
            {
                placeholderPrefab = CreatePlaceholderPrefab();
            }
            
            // Load saved settings if available
            LoadSavedSettings();
        }

        private GameObject CreatePlaceholderPrefab()
        {
            GameObject placeholder = new GameObject("PlaceholderFence");
            placeholder.transform.localScale = Vector3.one;
            
            // Set to PlaceholderFence layer
            placeholder.layer = LayerMask.NameToLayer("PlaceholderFence");
            
            // Add collider for detection - this is all we need, no visual mesh
            BoxCollider collider = placeholder.AddComponent<BoxCollider>();
            collider.size = Vector3.one;
            collider.isTrigger = false; // Keep solid for raycast detection
            
            // Optional: Add a very small invisible mesh for editor visualization
            if (Application.isEditor)
            {
                MeshRenderer renderer = placeholder.AddComponent<MeshRenderer>();
                MeshFilter filter = placeholder.AddComponent<MeshFilter>();
                
                // Create a tiny cube that's barely visible
                filter.mesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
                
                // Make it completely transparent
                Material invisibleMaterial = new Material(Shader.Find("Standard"));
                invisibleMaterial.color = new Color(1, 0, 1, 0.1f); // Very transparent magenta for debugging
                invisibleMaterial.SetFloat("_Mode", 3); // Transparent mode
                invisibleMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                invisibleMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                invisibleMaterial.SetInt("_ZWrite", 0);
                invisibleMaterial.DisableKeyword("_ALPHATEST_ON");
                invisibleMaterial.EnableKeyword("_ALPHABLEND_ON");
                invisibleMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                invisibleMaterial.renderQueue = 3000;
                renderer.material = invisibleMaterial;
                
                // Make it very small
                placeholder.transform.localScale = Vector3.one * 0.1f;
            }
            
            return placeholder;
        }

        private void LoadSavedSettings()
        {
            if (fenceSettingsTracker != null)
            {
                var savedSettings = fenceSettingsTracker.GetSettings(transform.position);
                if (savedSettings != null)
                {
                    countPositiveX = savedSettings.countPositiveX;
                    countNegativeX = savedSettings.countNegativeX;
                    countPositiveZ = savedSettings.countPositiveZ;
                    countNegativeZ = savedSettings.countNegativeZ;
                    Debug.Log($"[ArrayModifier] Loaded saved settings: {savedSettings}");
                }
            }
        }

        private void Start()
        {
            // Add this fence as the first fence in each direction
            posXFences.Add(this.gameObject);
            negXFences.Add(this.gameObject);
            posZFences.Add(this.gameObject);
            negZFences.Add(this.gameObject);
            
            StartCoroutine(ManageAllDirections());
        }

        private IEnumerator ManageAllDirections()
        {
            while (true)
            {
                // Save current settings
                SaveCurrentSettings();
                
                // Handle manual deletions first
                HandleManualDeletions();
                
                // Clean up null references
                CleanupNullReferences();
                
                yield return StartCoroutine(UpdateDirection(posXFences, countPositiveX, Vector3.right, "PosX"));
                yield return StartCoroutine(UpdateDirection(negXFences, countNegativeX, Vector3.left, "NegX"));
                yield return StartCoroutine(UpdateDirection(posZFences, countPositiveZ, Vector3.forward, "PosZ"));
                yield return StartCoroutine(UpdateDirection(negZFences, countNegativeZ, Vector3.back, "NegZ"));
                
                // Update FenceExtender on end fences
                UpdateEndFenceExtenders();
                
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void HandleManualDeletions()
        {
            HandleManualDeletionsInList(posXFences, Vector3.right, "PosX");
            HandleManualDeletionsInList(negXFences, Vector3.left, "NegX");
            HandleManualDeletionsInList(posZFences, Vector3.forward, "PosZ");
            HandleManualDeletionsInList(negZFences, Vector3.back, "NegZ");
        }

        private void HandleManualDeletionsInList(List<GameObject> fenceList, Vector3 direction, string dirName)
        {
            for (int i = 1; i < fenceList.Count; i++) // Skip index 0 (main fence)
            {
                if (fenceList[i] == null)
                {
                    // Replace with placeholder to maintain chain
                    Vector3 placeholderPos = transform.position + direction * spacing * i;
                    GameObject placeholder = Instantiate(placeholderPrefab, placeholderPos, Quaternion.identity);
                    placeholder.name = $"Placeholder_{dirName}_{i}";
                    
                    // Set to PlaceholderFence layer
                    placeholder.layer = LayerMask.NameToLayer("PlaceholderFence");
                    
                    // Add FenceChild to maintain parent relationship
                    FenceChild childComponent = placeholder.AddComponent<FenceChild>();
                    childComponent.Initialize(this.transform, dirName, i, true); // true = isPlaceholder
                    
                    fenceList[i] = placeholder;
                    Debug.Log($"[ArrayModifier] Replaced manually deleted fence with placeholder at {placeholderPos}");
                }
                // NEW: Clean up destroyed placeholder references
                else if (fenceList[i] != null && fenceList[i].GetComponent<FenceChild>() != null && 
                         fenceList[i].GetComponent<FenceChild>().IsPlaceholder && !fenceList[i].activeInHierarchy)
                {
                    // Placeholder was destroyed but reference still exists
                    fenceList[i] = null;
                    Debug.Log($"[ArrayModifier] Cleaned up destroyed placeholder reference in {dirName} at index {i}");
                }
            }
        }

        private void UpdateEndFenceExtenders()
        {
            // Clean up old extenders
            CleanupOldExtenders();
            
            // Add extenders to end fences only
            AddExtenderToEndFence(posXFences, countPositiveX);
            AddExtenderToEndFence(negXFences, countNegativeX);
            AddExtenderToEndFence(posZFences, countPositiveZ);
            AddExtenderToEndFence(negZFences, countNegativeZ);
        }

        private void CleanupOldExtenders()
        {
            for (int i = endFencesWithExtenders.Count - 1; i >= 0; i--)
            {
                if (endFencesWithExtenders[i] == null)
                {
                    endFencesWithExtenders.RemoveAt(i);
                }
            }
        }

        private void AddExtenderToEndFence(List<GameObject> fenceList, int targetCount)
        {
            if (fenceList.Count > 1 && targetCount > 1)
            {
                GameObject endFence = fenceList[fenceList.Count - 1];
                
                // Only add extender if it's not a placeholder and doesn't already have one
                if (endFence != null && endFence != this.gameObject && 
                    !IsPlaceholder(endFence) && 
                    endFence.GetComponent<FenceExtender>() == null)
                {
                    FenceExtender extender = endFence.AddComponent<FenceExtender>();
                    extender.Initialize(this, spacing, ghostMaterial, badMaterial, fenceSettingsTracker);
                    endFencesWithExtenders.Add(endFence);
                    Debug.Log($"[ArrayModifier] Added FenceExtender to end fence at {endFence.transform.position}");
                }
            }
        }

        private bool IsPlaceholder(GameObject fence)
        {
            FenceChild child = fence.GetComponent<FenceChild>();
            return child != null && child.IsPlaceholder;
        }

        private void SaveCurrentSettings()
        {
            if (fenceSettingsTracker != null)
            {
                fenceSettingsTracker.RecordSettings(
                    transform.position, 
                    countPositiveX, 
                    countNegativeX, 
                    countPositiveZ, 
                    countNegativeZ
                );
            }
        }

        private void CleanupNullReferences()
        {
            CleanupFenceList(posXFences);
            CleanupFenceList(negXFences);
            CleanupFenceList(posZFences);
            CleanupFenceList(negZFences);
        }

        private void CleanupFenceList(List<GameObject> fenceList)
        {
            // Don't remove nulls here - HandleManualDeletions will replace them with placeholders
            // Just ensure I'm always in the list at index 0
            if (fenceList.Count == 0 || fenceList[0] != this.gameObject)
            {
                if (this.gameObject != null)
                {
                    fenceList.Insert(0, this.gameObject);
                }
            }
        }

        private IEnumerator UpdateDirection(List<GameObject> fenceList, int targetCount, Vector3 direction, string dirName)
        {
            int clampedCount = Mathf.Min(targetCount, MAX_FENCE_POSTS);
            
            if (clampedCount > fenceList.Count)
            {
                // Add fences
                for (int i = fenceList.Count; i < clampedCount; i++)
                {
                    Vector3 newPosition = transform.position + direction * spacing * i;
                    GameObject newFence = CreateChildFence(newPosition, dirName, i);
                    
                    if (newFence != null)
                    {
                        fenceList.Add(newFence);
                    }
                    
                    yield return new WaitForSeconds(0.01f);
                }
            }
            else if (clampedCount < fenceList.Count)
            {
                // Remove excess fences (the "rope burning" effect)
                while (fenceList.Count > clampedCount && fenceList.Count > 1)
                {
                    GameObject fenceToRemove = fenceList[fenceList.Count - 1];
                    fenceList.RemoveAt(fenceList.Count - 1);
                    
                    if (fenceToRemove != null && fenceToRemove != this.gameObject)
                    {
                        // Remove from extender tracking
                        endFencesWithExtenders.Remove(fenceToRemove);
                        
                        // If it has extender, clean it up
                        FenceExtender extender = fenceToRemove.GetComponent<FenceExtender>();
                        if (extender != null)
                        {
                            yield return StartCoroutine(extender.DeleteAllMyFences());
                        }
                        
                        StartCoroutine(RemoveWithRedWarning(fenceToRemove));
                    }
                    
                    yield return new WaitForSeconds(0.05f);
                }
            }
            
            yield return null;
        }

        private GameObject CreateChildFence(Vector3 position, string direction, int index)
        {
            GameObject newFence = Instantiate(_arrayItem, position, Quaternion.identity);
            
            // Start with ghost material
            MeshRenderer renderer = newFence.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.material = ghostMaterial;
            }

            // Remove any existing ArrayModifier or FenceExtender scripts
            ArrayModifier duplicateModifier = newFence.GetComponent<ArrayModifier>();
            if (duplicateModifier != null)
                Destroy(duplicateModifier);
            
            FenceExtender duplicateExtender = newFence.GetComponent<FenceExtender>();
            if (duplicateExtender != null)
                Destroy(duplicateExtender);

            // Add FenceChild component to track parent relationship
            FenceChild childComponent = newFence.AddComponent<FenceChild>();
            childComponent.Initialize(this.transform, direction, index, false); // false = not placeholder

            StartCoroutine(HandleMaterialTransition(newFence));
            return newFence;
        }

        private IEnumerator HandleMaterialTransition(GameObject fence)
        {
            if (fence == null) yield break;
            
            yield return new WaitForSeconds(1f);
            
            if (fence != null && fence.activeInHierarchy)
            {
                MeshRenderer renderer = fence.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    renderer.material = _itemDefaultMaterial;
                }
            }
        }

        private IEnumerator RemoveWithRedWarning(GameObject fence)
        {
            if (fence != null)
            {
                MeshRenderer renderer = fence.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    renderer.material = badMaterial;
                }
            }
            
            yield return new WaitForSeconds(1f);
            
            if (fence != null)
            {
                Destroy(fence);
            }
        }

        // Public methods for FenceExtender
        public void RequestFenceDeletion(GameObject fence)
        {
            if (fence != null)
            {
                endFencesWithExtenders.Remove(fence);
                StartCoroutine(RemoveWithRedWarning(fence));
            }
        }

        public GameObject CreateFence(Vector3 position, string name)
        {
            GameObject newFence = Instantiate(_arrayItem, position, Quaternion.identity);
            newFence.name = name;
            
            MeshRenderer renderer = newFence.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.material = ghostMaterial;
            }

            // Remove duplicate scripts
            ArrayModifier duplicateModifier = newFence.GetComponent<ArrayModifier>();
            if (duplicateModifier != null)
                Destroy(duplicateModifier);
            
            FenceExtender duplicateExtender = newFence.GetComponent<FenceExtender>();
            if (duplicateExtender != null)
                Destroy(duplicateExtender);

            StartCoroutine(HandleMaterialTransition(newFence));
            return newFence;
        }

        public List<GameObject> GetAllChildFences()
        {
            List<GameObject> allChildren = new List<GameObject>();
            
            AddChildrenFromList(posXFences, allChildren);
            AddChildrenFromList(negXFences, allChildren);
            AddChildrenFromList(posZFences, allChildren);
            AddChildrenFromList(negZFences, allChildren);
            
            return allChildren;
        }

        private void AddChildrenFromList(List<GameObject> fenceList, List<GameObject> targetList)
        {
            for (int i = 1; i < fenceList.Count; i++) // Start from 1 to skip the parent
            {
                if (fenceList[i] != null && !targetList.Contains(fenceList[i]))
                {
                    targetList.Add(fenceList[i]);
                }
            }
        }

        // NEW PLAYER BUILD METHODS
        public float GetSpacing()
        {
            return spacing;
        }

        public void PlaceFenceAtPosition(Vector3 position, Vector3 direction)
        {
            // Determine which direction list to use
            List<GameObject> targetList = null;
            string dirName = "";
            
            if (direction == Vector3.right)
            {
                targetList = posXFences;
                dirName = "PosX";
                countPositiveX++;
            }
            else if (direction == Vector3.left)
            {
                targetList = negXFences;
                dirName = "NegX";
                countNegativeX++;
            }
            else if (direction == Vector3.forward)
            {
                targetList = posZFences;
                dirName = "PosZ";
                countPositiveZ++;
            }
            else if (direction == Vector3.back)
            {
                targetList = negZFences;
                dirName = "NegZ";
                countNegativeZ++;
            }
            
            if (targetList != null)
            {
                // Calculate index based on position
                int index = Mathf.RoundToInt(Vector3.Distance(transform.position, position) / spacing);
                
                // Ensure list is large enough
                while (targetList.Count <= index)
                {
                    targetList.Add(null);
                }
                
                // Only place if position is empty
                if (index < targetList.Count && targetList[index] == null)
                {
                    GameObject newFence = CreateChildFence(position, dirName, index);
                    if (newFence != null)
                    {
                        targetList[index] = newFence;
                        Debug.Log($"[ArrayModifier] Manually placed fence at {position}");
                    }
                }
            }
        }

        // NEW METHOD: Direct replacement of placeholder in fence lists
        public void ReplacePlaceholderWithFence(string direction, int index, GameObject newFence)
        {
            List<GameObject> targetList = null;
            
            switch (direction)
            {
                case "PosX":
                    targetList = posXFences;
                    break;
                case "NegX":
                    targetList = negXFences;
                    break;
                case "PosZ":
                    targetList = posZFences;
                    break;
                case "NegZ":
                    targetList = negZFences;
                    break;
            }
            
            if (targetList != null && index < targetList.Count)
            {
                // Replace the placeholder reference with the new fence
                targetList[index] = newFence;
                Debug.Log($"[ArrayModifier] Replaced placeholder in {direction} list at index {index}");
            }
        }

        // NEW METHOD: Help clean up placeholder references
        public void CleanupPlaceholderReference(string direction, int index)
        {
            List<GameObject> targetList = null;
            
            switch (direction)
            {
                case "PosX":
                    targetList = posXFences;
                    break;
                case "NegX":
                    targetList = negXFences;
                    break;
                case "PosZ":
                    targetList = posZFences;
                    break;
                case "NegZ":
                    targetList = negZFences;
                    break;
            }
            
            if (targetList != null && index < targetList.Count)
            {
                if (targetList[index] != null && IsPlaceholder(targetList[index]))
                {
                    targetList[index] = null; // Clear the placeholder reference
                    Debug.Log($"[ArrayModifier] Cleaned up placeholder reference in {direction} list at index {index}");
                }
            }
        }

        private void OnDestroy()
        {
            // Clean up all child fences when main fence is destroyed
            List<GameObject> allChildren = GetAllChildFences();
            foreach (GameObject child in allChildren)
            {
                if (child != null)
                {
                    FenceChild fenceChild = child.GetComponent<FenceChild>();
                    if (fenceChild != null)
                    {
                        fenceChild.CleanupAsChild();
                    }
                }
            }
            
            // Clean up settings
            if (fenceSettingsTracker != null)
            {
                fenceSettingsTracker.RemoveSettings(transform.position);
            }
        }

        [Header("Debug Info")]
        [SerializeField] private int totalChildCount;
        
        private void Update()
        {
            if (Application.isEditor)
            {
                totalChildCount = GetAllChildFences().Count;
            }
        }
    }
}