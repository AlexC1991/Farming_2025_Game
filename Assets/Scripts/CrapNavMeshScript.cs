using UnityEngine;
using UnityEngine.AI;

public class CrapNavMeshScript : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    
    public float wanderRadius = 10f;
    public float minWanderWaitTime = 3f;
    public float maxWanderWaitTime = 10f;
    private float waitTimer;

    // Animation parameter names - match these with your Animator Controller
    private readonly string isWalkingParam = "IsWalking";

    void Start()
    {
        // Get the NavMeshAgent component
        agent = GetComponent<NavMeshAgent>();
        // Get the Animator component
        animator = GetComponent<Animator>();
        
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component missing from the crab!");
            return;
        }

        if (animator == null)
        {
            Debug.LogError("Animator component missing from the crab!");
            return;
        }

        // Start the wandering behavior
        SetNewRandomDestination();
    }

    void Update()
    {
        // Update animation based on whether the crab is moving
        bool isMoving = agent.velocity.magnitude > 0.1f; // Small threshold to determine if moving
        animator.SetBool(isWalkingParam, isMoving);

        // Check if we've reached the destination or are not moving
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            // Wait for some time before setting a new destination
            waitTimer -= Time.deltaTime;
            
            if (waitTimer <= 0)
            {
                SetNewRandomDestination();
            }
        }
    }

    void SetNewRandomDestination()
    {
        // Get a random position within the wander radius
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        // Find the nearest point on the NavMesh to the random position
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
        {
            // Set the destination
            agent.SetDestination(hit.position);
            
            // Set a random wait time for the next destination
            waitTimer = Random.Range(minWanderWaitTime, maxWanderWaitTime);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a sphere around the wander radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}