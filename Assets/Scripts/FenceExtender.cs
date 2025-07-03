using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace farming2025
{
    public class FenceExtender : MonoBehaviour
    {
        [Header("Directional Counts")]
        [SerializeField] private int countPositiveX = 1;
        [SerializeField] private int countNegativeX = 1;
        [SerializeField] private int countPositiveZ = 1;
        [SerializeField] private int countNegativeZ = 1;
        
        [Header("Settings")]
        [SerializeField] private GameObject placeholderPrefab; // For maintaining chain when manually deleted
        
        [Header("Settings Tracker")]
        [SerializeField] private FenceSettingsTracker settingsTracker;
        
        private ArrayModifier mainController;
        private float spacing;
        private Material ghostMaterial;
        private Material badMaterial;
        
        // My fence lists
        private List<GameObject> myPosXFences = new List<GameObject>();
        private List<GameObject> myNegXFences = new List<GameObject>();
        private List<GameObject> myPosZFences = new List<GameObject>();
        private List<GameObject> myNegZFences = new List<GameObject>();
        
        private bool isDeleting = false;
        private Coroutine managementCoroutine;

        public void Initialize(ArrayModifier controller, float fenceSpacing, Material ghost, Material bad, FenceSettingsTracker tracker)
        {
            mainController = controller;
            spacing = fenceSpacing;
            ghostMaterial = ghost;
            badMaterial = bad;
            settingsTracker = tracker;
            
            // Create placeholder prefab if none assigned
            /*if (placeholderPrefab == null)
            {
                placeholderPrefab = CreatePlaceholderPrefab();
            }*/
            
            // Load saved settings
            if (settingsTracker != null)
            {
                var savedSettings = settingsTracker.GetSettings(transform.position);
                if (savedSettings != null)
                {
                    countPositiveX = savedSettings.countPositiveX;
                    countNegativeX = savedSettings.countNegativeX;
                    countPositiveZ = savedSettings.countPositiveZ;
                    countNegativeZ = savedSettings.countNegativeZ;
                }
            }
            
            // Add myself as the center point
            myPosXFences.Add(gameObject);
            myNegXFences.Add(gameObject);
            myPosZFences.Add(gameObject);
            myNegZFences.Add(gameObject);
            
            managementCoroutine = StartCoroutine(ManageMyFences());
        }

        private IEnumerator ManageMyFences()
        {
            while (this != null && !isDeleting)
            {
                // Save settings
                if (settingsTracker != null)
                {
                    settingsTracker.RecordSettings(transform.position, countPositiveX, countNegativeX, countPositiveZ, countNegativeZ);
                }
                
                // Handle manual deletions first - replace with placeholders
                HandleManualDeletions();
                
                // Update directions
                yield return StartCoroutine(UpdateMyDirection(myPosXFences, countPositiveX, Vector3.right, "PosX"));
                if (isDeleting) break;
                
                yield return StartCoroutine(UpdateMyDirection(myNegXFences, countNegativeX, Vector3.left, "NegX"));
                if (isDeleting) break;
                
                yield return StartCoroutine(UpdateMyDirection(myPosZFences, countPositiveZ, Vector3.forward, "PosZ"));
                if (isDeleting) break;
                
                yield return StartCoroutine(UpdateMyDirection(myNegZFences, countNegativeZ, Vector3.back, "NegZ"));
                if (isDeleting) break;
                
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void HandleManualDeletions()
        {
            HandleManualDeletionsInList(myPosXFences, Vector3.right, "PosX");
            HandleManualDeletionsInList(myNegXFences, Vector3.left, "NegX");
            HandleManualDeletionsInList(myPosZFences, Vector3.forward, "PosZ");
            HandleManualDeletionsInList(myNegZFences, Vector3.back, "NegZ");
        }

        private void HandleManualDeletionsInList(List<GameObject> fenceList, Vector3 direction, string dirName)
        {
            for (int i = 1; i < fenceList.Count; i++) // Skip index 0 (myself)
            {
                if (fenceList[i] == null)
                {
                    // Replace with placeholder to maintain chain
                    Vector3 placeholderPos = transform.position + direction * spacing * i;
                    GameObject placeholder = Instantiate(placeholderPrefab, placeholderPos, Quaternion.identity);
                    placeholder.name = $"Placeholder_{gameObject.name}_{dirName}_{i}";
                    
                    // Set to PlaceholderFence layer
                    placeholder.layer = LayerMask.NameToLayer("PlaceholderFence");
                    
                    // Add FenceChild to maintain parent relationship
                    FenceChild childComponent = placeholder.AddComponent<FenceChild>();
                    childComponent.Initialize(this.transform, dirName, i, true); // true = isPlaceholder
                    
                    fenceList[i] = placeholder;
                    Debug.Log($"[FenceExtender] Replaced manually deleted fence with placeholder at {placeholderPos}");
                }
                // Clean up destroyed placeholder references
                else if (fenceList[i] != null && fenceList[i].GetComponent<FenceChild>() != null && 
                         fenceList[i].GetComponent<FenceChild>().IsPlaceholder && !fenceList[i].activeInHierarchy)
                {
                    // Placeholder was destroyed but reference still exists
                    fenceList[i] = null;
                    Debug.Log($"[FenceExtender] Cleaned up destroyed placeholder reference in {dirName} at index {i}");
                }
            }
        }

        private bool IsPlaceholder(GameObject fence)
        {
            FenceChild child = fence.GetComponent<FenceChild>();
            return child != null && child.IsPlaceholder;
        }

        private IEnumerator UpdateMyDirection(List<GameObject> fenceList, int targetCount, Vector3 direction, string dirName)
        {
            if (isDeleting) yield break;
            
            const int MAX_FENCE_POSTS = 20;
            int clampedCount = Mathf.Min(targetCount, MAX_FENCE_POSTS);
            
            if (clampedCount > fenceList.Count)
            {
                // Add fences
                for (int i = fenceList.Count; i < clampedCount; i++)
                {
                    Vector3 newPosition = transform.position + direction * spacing * i;
                    GameObject newFence = mainController.CreateFence(newPosition, $"{gameObject.name}_{dirName}_{Random.Range(0, 500)}");
                    if (!isDeleting && newFence != null)
                    {
                        fenceList.Add(newFence);
                    }
                    yield return new WaitForSeconds(0.01f);
                    if (isDeleting) break;
                }
            }
            else if (clampedCount < fenceList.Count)
            {
                // Remove excess fences (the "rope burning" effect) - ONLY when count is reduced
                while (fenceList.Count > clampedCount && fenceList.Count > 1 && !isDeleting)
                {
                    GameObject fenceToRemove = fenceList[fenceList.Count - 1];
                    fenceList.RemoveAt(fenceList.Count - 1);
                    
                    if (fenceToRemove != gameObject && fenceToRemove != null && mainController != null)
                    {
                        mainController.RequestFenceDeletion(fenceToRemove);
                    }
                    
                    yield return new WaitForSeconds(0.01f);
                }
            }
            
            yield return null;
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
                targetList = myPosXFences;
                dirName = "PosX";
                countPositiveX++;
            }
            else if (direction == Vector3.left)
            {
                targetList = myNegXFences;
                dirName = "NegX";
                countNegativeX++;
            }
            else if (direction == Vector3.forward)
            {
                targetList = myPosZFences;
                dirName = "PosZ";
                countPositiveZ++;
            }
            else if (direction == Vector3.back)
            {
                targetList = myNegZFences;
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
                    GameObject newFence = mainController.CreateFence(position, $"{gameObject.name}_{dirName}_{Random.Range(0, 500)}");
                    if (newFence != null)
                    {
                        targetList[index] = newFence;
                        Debug.Log($"[FenceExtender] Manually placed fence at {position}");
                    }
                }
            }
        }

        public GameObject CreateFence(Vector3 position, string name)
        {
            if (mainController != null)
            {
                return mainController.CreateFence(position, name);
            }
            return null;
        }

        public IEnumerator DeleteAllMyFences()
        {
            isDeleting = true;
            if (managementCoroutine != null)
            {
                StopCoroutine(managementCoroutine);
                managementCoroutine = null;
            }
            
            // Delete all my fences except myself
            List<GameObject>[] allMyLists = { myPosXFences, myNegXFences, myPosZFences, myNegZFences };
            
            foreach (var fenceList in allMyLists)
            {
                List<GameObject> fencesToDelete = new List<GameObject>();
                for (int i = 1; i < fenceList.Count; i++)
                {
                    if (fenceList[i] != null && fenceList[i] != gameObject)
                    {
                        fencesToDelete.Add(fenceList[i]);
                    }
                }
                
                fenceList.Clear();
                if (gameObject != null)
                {
                    fenceList.Add(gameObject);
                }
                
                foreach (GameObject fence in fencesToDelete)
                {
                    if (fence != null && fence != gameObject && mainController != null)
                    {
                        mainController.RequestFenceDeletion(fence);
                    }
                    yield return new WaitForSeconds(0.005f);
                }
            }
            
            yield return new WaitForSeconds(0.02f);
            isDeleting = false;
        }

        void OnDestroy()
        {
            isDeleting = true;
            if (managementCoroutine != null)
            {
                StopCoroutine(managementCoroutine);
            }
            
            if (settingsTracker != null)
            {
                settingsTracker.RemoveSettings(transform.position);
            }
        }
    }
}