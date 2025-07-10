using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlyingBird : MonoBehaviour
{
    [Header("Flight Settings")]
    public float flySpeed = 5f;
    public float rotationSpeed = 2f;
    public float landingDistance = 2f;
    public float landingDuration = 8f;
    public float flightHeight = 10f;
    public float circleRadius = 50f;
    public float approachDistance = 25f;
    
    [Header("Landing Positions")]
    public Transform[] landingSpots;
    public bool useRandomLanding = true;
    
    [Header("Animation")]
    public Animator animator;
    public string flyingAnimationName = "Flying";
    public string landingAnimationName = "Landing";
    public string idleAnimationName = "Idle";

    public enum BirdState
    {
        CircularFlying,
        FlyingAway,
        ApproachingLanding,
        Landing,
        Resting,
        TakingOff
    }
    
    private BirdState currentState = BirdState.CircularFlying;
    private Transform currentTarget;
    private Vector3 approachPoint; // The 90-degree approach point
    private Vector3 circleCenter;
    private float circleAngle = 0f;
    private int currentLandingIndex = 0;
    private Coroutine landingRoutine;
    private Vector3 takeoffDirection; // Store the takeoff angle
    private float landingTimer = 0f;
    public float timeBeforeLanding = 15f; // How long to fly in circles before landing
    
    void Start()
    {
        if (landingSpots.Length == 0)
        {
            Debug.LogWarning("No landing spots assigned to " + gameObject.name);
            return;
        }
        
        // Start flying in circles around the center of all landing spots
        CalculateCircleCenter();
        SelectNextLandingSpot();
        SetState(BirdState.CircularFlying);
    }
    
    void Update()
    {
        switch (currentState)
        {
            case BirdState.CircularFlying:
                HandleCircularFlying();
                break;
            case BirdState.FlyingAway:
                HandleFlyingAway();
                break;
            case BirdState.ApproachingLanding:
                HandleApproachingLanding();
                break;
            case BirdState.Landing:
                HandleLanding();
                break;
            case BirdState.TakingOff:
                HandleTakeOff();
                break;
        }
    }
    
    void CalculateCircleCenter()
    {
        if (landingSpots.Length == 0) return;
        
        Vector3 center = Vector3.zero;
        foreach (Transform spot in landingSpots)
        {
            if (spot != null)
                center += spot.position;
        }
        center /= landingSpots.Length;
        circleCenter = center + Vector3.up * flightHeight;
    }
    
    void HandleCircularFlying()
    {
        // Fly in a random, varying circle around the center point
        circleAngle += (flySpeed / circleRadius) * Time.deltaTime;
        
        // Add some randomness to the flight pattern
        float radiusVariation = Mathf.Sin(Time.time * 0.5f) * (circleRadius * 0.3f);
        float currentRadius = circleRadius + radiusVariation;
        
        Vector3 targetPosition = circleCenter + new Vector3(
            Mathf.Cos(circleAngle) * currentRadius,
            Random.Range(-2f, 2f), // Small random height variation
            Mathf.Sin(circleAngle) * currentRadius
        );
        
        // Calculate movement direction before moving
        Vector3 direction = (targetPosition - transform.position).normalized;
        
        // Move towards the circle position
        transform.position += direction * flySpeed * Time.deltaTime;
        
        // Rotate to face movement direction with proper up vector
        if (direction.magnitude > 0.1f)
        {
            // Create rotation that faces the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * 2f * Time.deltaTime);
        }
        
        // Count down to landing
        landingTimer += Time.deltaTime;
        if (landingTimer >= timeBeforeLanding)
        {
            landingTimer = 0f;
            CalculateApproachPoint();
            SetState(BirdState.FlyingAway);
        }
    }
    
    void CalculateApproachPoint()
    {
        if (currentTarget == null) return;
        
        Vector3 landingPos = currentTarget.position;
        
        // Create a random perpendicular direction from the landing spot
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 perpendicular = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle));
        
        // Set approach point AWAY from landing spot at SAME HEIGHT as flight height
        approachPoint = landingPos + (perpendicular * approachDistance);
        approachPoint.y = flightHeight; // Set to flight height, NOT above landing spot
    }
    
    void HandleFlyingAway()
    {
        // Fly to the approach point (which is away from landing spot)
        Vector3 direction = (approachPoint - transform.position).normalized;
        transform.position += direction * flySpeed * Time.deltaTime;
        
        // Face the direction we're flying with proper orientation
        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * 2f * Time.deltaTime);
        }
        
        // Check if reached the approach point
        float distanceToApproach = Vector3.Distance(transform.position, approachPoint);
        if (distanceToApproach <= 3f)
        {
            SetState(BirdState.ApproachingLanding);
        }
    }
    
    void HandleApproachingLanding()
    {
        if (currentTarget == null) return;
        
        // Fly horizontally toward landing spot while descending at the 90-degree angle
        Vector3 landingPos = currentTarget.position;
        Vector3 direction = (landingPos - transform.position).normalized;
        
        transform.position += direction * flySpeed * 0.8f * Time.deltaTime;
        
        // Face the landing direction with proper bank/tilt
        if (direction.magnitude > 0.1f)
        {
            // Calculate banking angle based on descent
            Vector3 bankDirection = direction;
            if (direction.y < 0) // If descending, add slight banking
            {
                bankDirection = Vector3.Slerp(Vector3.up, direction, 0.7f);
            }
            
            Quaternion targetRotation = Quaternion.LookRotation(direction, bankDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * 1.5f * Time.deltaTime);
        }
        
        // When close to landing spot, switch to final landing
        float distanceToLanding = Vector3.Distance(transform.position, landingPos);
        if (distanceToLanding <= 3f)
        {
            SetState(BirdState.Landing);
        }
    }
    
    void HandleLanding()
    {
        if (currentTarget == null) return;
        
        // Final descent - gradual angle down to landing spot
        Vector3 landingPos = currentTarget.position;
        Vector3 direction = (landingPos - transform.position).normalized;
        
        // Maintain some forward momentum while descending
        Vector3 forwardDirection = transform.forward;
        Vector3 landingDirection = Vector3.Slerp(forwardDirection, direction, 0.7f);
        
        float landingSpeed = flySpeed * 0.6f;
        transform.position += landingDirection * landingSpeed * Time.deltaTime;
        
        // Gradually face downward with realistic bird landing posture
        if (landingDirection.magnitude > 0.1f)
        {
            // Birds tilt their body forward when landing
            Vector3 upDirection = Vector3.Slerp(Vector3.up, -direction, 0.3f);
            Quaternion targetRotation = Quaternion.LookRotation(landingDirection, upDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
        // Check if landed
        if (transform.position.y <= landingPos.y + 0.5f)
        {
            transform.position = landingPos;
            // Face forward when landed
            Vector3 forwardDir = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
            if (forwardDir != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(forwardDir, Vector3.up);
            }
            SetState(BirdState.Resting);
        }
    }
    
    void HandleTakeOff()
    {
        // Take off at an angle, not straight up
        transform.position += takeoffDirection * flySpeed * 0.8f * Time.deltaTime;
        
        // Face the takeoff direction
        if (takeoffDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(takeoffDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * 2f * Time.deltaTime);
        }
        
        // When high enough, return to circular flying
        if (transform.position.y >= flightHeight)
        {
            SelectNextLandingSpot();
            SetState(BirdState.CircularFlying);
        }
    }
    
    void SetState(BirdState newState)
    {
        currentState = newState;
        
        // Handle animations
        if (animator != null)
        {
            switch (newState)
            {
                case BirdState.CircularFlying:
                case BirdState.FlyingAway:
                case BirdState.ApproachingLanding:
                case BirdState.Landing:
                    animator.Play(flyingAnimationName);
                    break;
                case BirdState.Resting:
                    animator.Play(landingAnimationName);
                    StartCoroutine(SwitchToIdleAfterLanding());
                    
                    // Start resting timer
                    if (landingRoutine != null) StopCoroutine(landingRoutine);
                    landingRoutine = StartCoroutine(RestingTimer());
                    break;
                case BirdState.TakingOff:
                    // Calculate a NEW random takeoff direction (not the same as landing approach)
                    CalculateTakeoffDirection();
                    animator.Play(flyingAnimationName);
                    break;
            }
        }
    }
    
    IEnumerator SwitchToIdleAfterLanding()
    {
        yield return new WaitForSeconds(0.5f);
        if (currentState == BirdState.Resting && animator != null)
        {
            animator.Play(idleAnimationName);
        }
    }
    
    IEnumerator RestingTimer()
    {
        yield return new WaitForSeconds(landingDuration);
        SetState(BirdState.TakingOff);
    }
    
    void CalculateTakeoffDirection()
    {
        // Generate a NEW random direction for takeoff (different from landing approach)
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 horizontalDirection = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle));
        
        // Takeoff at an upward angle (like real birds)
        takeoffDirection = (horizontalDirection + Vector3.up * 0.7f).normalized;
    }
    
    void SelectNextLandingSpot()
    {
        if (landingSpots.Length == 0) return;
        
        if (useRandomLanding)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, landingSpots.Length);
            } while (randomIndex == currentLandingIndex && landingSpots.Length > 1);
            
            currentLandingIndex = randomIndex;
        }
        else
        {
            currentLandingIndex = (currentLandingIndex + 1) % landingSpots.Length;
        }
        
        currentTarget = landingSpots[currentLandingIndex];
    }
    
    // Public methods for external control
    public void SetLandingSpots(Transform[] spots)
    {
        landingSpots = spots;
        if (spots.Length > 0)
        {
            CalculateCircleCenter();
            SelectNextLandingSpot();
        }
    }
    
    public void ForceLandAt(int spotIndex)
    {
        if (spotIndex >= 0 && spotIndex < landingSpots.Length)
        {
            currentLandingIndex = spotIndex;
            currentTarget = landingSpots[spotIndex];
            
            if (landingRoutine != null) StopCoroutine(landingRoutine);
            CalculateApproachPoint();
            SetState(BirdState.FlyingAway);
        }
    }
    
    public void SetFlightSpeed(float speed)
    {
        flySpeed = speed;
    }
    
    public BirdState GetCurrentState()
    {
        return currentState;
    }
    
    void OnDrawGizmos()
    {
        // Draw circular flight path
        Gizmos.color = Color.white;
        if (Application.isPlaying)
        {
            // Draw circle
            for (int i = 0; i < 32; i++)
            {
                float angle1 = (i / 32f) * 2f * Mathf.PI;
                float angle2 = ((i + 1) / 32f) * 2f * Mathf.PI;
                
                Vector3 point1 = circleCenter + new Vector3(Mathf.Cos(angle1) * circleRadius, 0, Mathf.Sin(angle1) * circleRadius);
                Vector3 point2 = circleCenter + new Vector3(Mathf.Cos(angle2) * circleRadius, 0, Mathf.Sin(angle2) * circleRadius);
                
                Gizmos.DrawLine(point1, point2);
            }
        }
        
        // Draw current target and approach
        if (currentTarget != null)
        {
            // Draw approach point
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(approachPoint, 2f);
            
            // Draw approach path - straight line from approach point to landing spot
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(approachPoint, currentTarget.position);
            
            // Draw final landing approach - NO separate line, it's the same
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(currentTarget.position, 0.5f);
        }
        
        // Draw all landing spots
        if (landingSpots != null)
        {
            for (int i = 0; i < landingSpots.Length; i++)
            {
                if (landingSpots[i] != null)
                {
                    Gizmos.color = (i == currentLandingIndex) ? Color.green : Color.blue;
                    Gizmos.DrawWireSphere(landingSpots[i].position, 0.3f);
                }
            }
        }
    }
}