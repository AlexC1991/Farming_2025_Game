using UnityEngine;

public class RockingBoatSimulation : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float sideToSideRotationAmount = 5f;  // Max rotation angle for side to side
    [SerializeField] private float frontToBackRotationAmount = 3f; // Max rotation angle for front to back
    [SerializeField] private float rotationSpeed = 1f;            // Speed of the rocking motion
    
    [Header("Position Settings")]
    [SerializeField] private float verticalMovementAmount = 0.1f;  // How much the boat moves up and down
    [SerializeField] private float verticalSpeed = 1f;            // Speed of vertical movement

    private Vector3 startPosition;
    private Quaternion startRotation;
    private float timeOffset;

    void Start()
    {
        // Store the initial position and rotation
        startPosition = transform.position;
        startRotation = transform.rotation;
        
        // Random offset to make multiple boats look less synchronized
        timeOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    void Update()
    {
        // Calculate the time variable for our sine waves
        float time = (Time.time + timeOffset) * rotationSpeed;
        
        // Calculate rotation angles using sine waves
        float sideToSideRotation = Mathf.Sin(time) * sideToSideRotationAmount;
        float frontToBackRotation = Mathf.Sin(time * 0.5f) * frontToBackRotationAmount;
        
        // Create the rotation offset
        Quaternion rockingRotation = Quaternion.Euler(
            frontToBackRotation,
            0f,
            sideToSideRotation
        );
        
        // Apply the rotation relative to the start rotation
        transform.rotation = startRotation * rockingRotation;

        // Calculate vertical position using a sine wave
        float verticalOffset = Mathf.Sin(Time.time * verticalSpeed) * verticalMovementAmount;
        
        // Apply the position
        transform.position = startPosition + new Vector3(0f, verticalOffset, 0f);
    }
}