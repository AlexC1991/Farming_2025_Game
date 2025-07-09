using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class FlyingBird : MonoBehaviour
{
    [Header("Flying Settings")]
    public float flySpeed = 5f;
    public float rotationSpeed = 2f;
    public float wanderRadius = 20f;
    public float wanderTimer = 5f;
    
    [Header("Link Traversal Settings")]
    public float linkTraversalSpeed = 4f;
    public AnimationCurve takeoffCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public AnimationCurve landingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    [Header("Flight Behavior")]
    public bool preferAirTravel = true;
    public float groundTime = 2f; // Time to spend on ground before taking off again
    
    private NavMeshAgent agent;
    private float timer;
    private Vector3 targetPosition;
    private bool isTraversingLink = false;
    private bool isOnGround = true;
    private float groundTimer = 0f;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        // Configure agent for flying
        agent.speed = flySpeed;
        agent.angularSpeed = rotationSpeed * 57.2958f;
        agent.acceleration = 8f;
        agent.stoppingDistance = 1f;
        
        // Important settings for off-mesh links
        agent.autoTraverseOffMeshLink = false; // We'll handle this manually
        agent.areaMask = NavMesh.AllAreas; // Can use all areas including links
        
        timer = wanderTimer;
        
        // Check if starting on ground or in air
        CheckCurrentSurface();
        
        SetNewDestination();
    }
    
    void Update()
    {
        // Handle off-mesh link traversal
        if (agent.isOnOffMeshLink && !isTraversingLink)
        {
            StartCoroutine(TraverseOffMeshLink());
        }
        
        // Don't update destination while traversing links
        if (isTraversingLink)
            return;
            
        timer += Time.deltaTime;
        
        // Ground behavior - wait before taking off
        if (isOnGround)
        {
            groundTimer += Time.deltaTime;
            if (groundTimer >= groundTime || timer >= wanderTimer)
            {
                if (preferAirTravel)
                {
                    SetAirDestination();
                }
                else
                {
                    SetNewDestination();
                }
                groundTimer = 0f;
                timer = 0f;
            }
        }
        else
        {
            // Air behavior - normal wandering
            if (timer >= wanderTimer || agent.remainingDistance < 0.5f)
            {
                SetNewDestination();
                timer = 0f;
            }
        }
        
        // Smoothly rotate toward movement direction
        if (agent.velocity.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(agent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    void CheckCurrentSurface()
    {
        // Check if we're on ground level or in air
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 1f, NavMesh.AllAreas))
        {
            // Simple height check - adjust threshold based on your scene
            isOnGround = hit.position.y < 2f;
        }
    }
    
    void SetNewDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        
        // Adjust height based on current surface
        if (isOnGround)
        {
            randomDirection.y = transform.position.y; // Stay on ground level
        }
        else
        {
            randomDirection.y = Mathf.Clamp(randomDirection.y, 3f, 15f); // Stay in air
        }
        
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
        {
            targetPosition = hit.position;
            agent.SetDestination(targetPosition);
        }
    }
    
    void SetAirDestination()
    {
        // Specifically look for air destinations
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        randomDirection.y = Random.Range(5f, 15f); // Force air height
        
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
        {
            targetPosition = hit.position;
            agent.SetDestination(targetPosition);
        }
    }
    
    IEnumerator TraverseOffMeshLink()
    {
        isTraversingLink = true;
        
        OffMeshLinkData linkData = agent.currentOffMeshLinkData;
        Vector3 startPos = linkData.startPos;
        Vector3 endPos = linkData.endPos;
        
        // Determine if this is takeoff or landing
        bool isTakeoff = endPos.y > startPos.y;
        AnimationCurve currentCurve = isTakeoff ? takeoffCurve : landingCurve;
        
        float distance = Vector3.Distance(startPos, endPos);
        float duration = distance / linkTraversalSpeed;
        
        float elapsedTime = 0f;
        
        // Disable agent movement during traversal
        agent.updatePosition = false;
        agent.updateRotation = false;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;
            
            // Use curve for more natural movement
            float curveProgress = currentCurve.Evaluate(progress);
            
            // Interpolate position
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, progress);
            
            // Add curve-based height adjustment for more natural flight arc
            if (isTakeoff)
            {
                currentPos.y = Mathf.Lerp(startPos.y, endPos.y, curveProgress);
            }
            else
            {
                currentPos.y = Mathf.Lerp(startPos.y, endPos.y, curveProgress);
            }
            
            transform.position = currentPos;
            
            // Rotate towards movement direction
            Vector3 direction = (endPos - startPos).normalized;
            if (direction.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            
            yield return null;
        }
        
        // Ensure we end up exactly at the end position
        transform.position = endPos;
        
        // Re-enable agent movement
        agent.updatePosition = true;
        agent.updateRotation = true;
        
        // Complete the link traversal
        agent.CompleteOffMeshLink();
        
        // Update surface state
        isOnGround = endPos.y < 2f; // Adjust threshold as needed
        
        isTraversingLink = false;
        
        // Reset timers
        timer = 0f;
        groundTimer = 0f;
    }
    
    // Optional: Visual debugging
    void OnDrawGizmosSelected()
    {
        // Draw wander radius
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
        
        if (agent != null)
        {
            // Draw destination
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(targetPosition, 0.5f);
            
            // Draw path
            if (agent.hasPath)
            {
                Gizmos.color = Color.green;
                Vector3[] corners = agent.path.corners;
                for (int i = 0; i < corners.Length - 1; i++)
                {
                    Gizmos.DrawLine(corners[i], corners[i + 1]);
                }
            }
        }
    }
}