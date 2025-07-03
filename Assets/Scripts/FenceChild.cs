using UnityEngine;

namespace farming2025
{
    public class FenceChild : MonoBehaviour
    {
        [Header("Child Info")]
        [SerializeField] private Transform parentFence;
        [SerializeField] private string direction;
        [SerializeField] private int indexInDirection;
        [SerializeField] private bool isPlaceholder;
        
        public Transform ParentFence => parentFence;
        public string Direction => direction;
        public int IndexInDirection => indexInDirection;
        public bool IsPlaceholder => isPlaceholder;
        
        public void Initialize(Transform parent, string dir, int index, bool placeholder = false)
        {
            parentFence = parent;
            direction = dir;
            indexInDirection = index;
            isPlaceholder = placeholder;
            
            // Name the fence for easier debugging
            string prefix = placeholder ? "Placeholder" : parent.name;
            gameObject.name = $"{prefix}_{dir}_{index}";
        }
        
        public void CleanupAsChild()
        {
            // This is called when the parent is being destroyed
            // Remove any components that might cause issues
            FenceExtender extender = GetComponent<FenceExtender>();
            if (extender != null)
            {
                StartCoroutine(extender.DeleteAllMyFences());
            }
            
            // Destroy this child after a short delay
            Destroy(gameObject, 0.1f);
        }
        
        private void OnDestroy()
        {
            // If this child is destroyed and it's not a placeholder,
            // notify the parent that manual deletion occurred
            if (!isPlaceholder && parentFence != null)
            {
                // Parent scripts will handle this in their next update cycle
                Debug.Log($"[FenceChild] Child {gameObject.name} was manually deleted");
            }
        }
        
        // Draw a line to parent in scene view for debugging
        private void OnDrawGizmosSelected()
        {
            if (parentFence != null)
            {
                Gizmos.color = isPlaceholder ? Color.red : Color.yellow;
                Gizmos.DrawLine(transform.position, parentFence.position);
                
                if (isPlaceholder)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(transform.position, 0.2f);
                }
            }
        }
    }
}