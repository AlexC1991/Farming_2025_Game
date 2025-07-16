using System.Collections;
using UnityEngine;

namespace farming2025
{
    public class CharacterMovementScript : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float walkSpeed = 3f;
        [SerializeField] private float runSpeedMultiplier = 2f;
        [SerializeField] private float turnSmoothTime = 0.1f;
        [SerializeField] private float speedChangeRate = 10f;
        
        [Header("Gravity Settings")]
        [SerializeField] private float gravityStrength = 20f; // Adjustable gravity force
        [SerializeField] private float maxFallSpeed = 50f; // Terminal velocity
        
        [Header("3rd Person Camera Settings")]
        [SerializeField] private Transform cameraFollowTarget;
        [SerializeField] private float mouseSensitivity = 2f;
        [SerializeField] private float verticalRotationLimit = 80f;
        [SerializeField] private bool invertMouseY = false;
        
        [Header("Camera Positioning")]
        [SerializeField] private float cameraDistance = 8f;
        [SerializeField] private float cameraHeight = 2f;
        [SerializeField] private float cameraSideOffset = 0f;
        [SerializeField] private float cameraFollowSpeed = 10f;
        [SerializeField] private LayerMask cameraCollisionLayers = -1;
        
        [Header("Camera View Modes")]
        [SerializeField] private bool isFirstPerson = false;
        [SerializeField] private KeyCode toggleViewKey = KeyCode.V; // Backup toggle (optional)
        [SerializeField] private float minThirdPersonDistance = 1.5f; // When to switch to first person
        [SerializeField] private float maxThirdPersonDistance = 15f;
        [SerializeField] private float scrollSensitivity = 2f;
        [SerializeField] private bool allowScrollCameraControl = true; // Can be disabled for indoor areas
        
        [Header("First Person Settings")]
        [SerializeField] private float firstPersonHeight = 1.7f;
        [SerializeField] private Vector3 firstPersonOffset = new Vector3(0, 0, 0.1f); // Forward offset to prevent clipping
        [SerializeField] private float firstPersonClipDistance = 0.3f; // Distance from character mesh
        [SerializeField] private bool hideCharacterInFirstPerson = true;
        [SerializeField] private LayerMask characterLayer = 1 << 8; // Layer for character mesh
        
        [Header("Ground Detection")]
        [SerializeField] private bool useBuiltInGroundCheck = true;
        [SerializeField] private float groundStickForce = -9.81f; // Use gravity strength
        [SerializeField] private float skinWidthBuffer = 0.01f; // Buffer for skin width issues
        [Header("Performance Settings")]
        [SerializeField] private float updateRate = 60f; // Updates per second
        [SerializeField] private bool useCoroutineUpdates = true;
        
        [Header("Animation Settings")]
        [SerializeField] private float animationBlendSpeed = 15f;
        [SerializeField] private bool useRootMotion = false;
        [SerializeField] private float runAnimationSpeedMultiplier = 1.2f;
        [SerializeField] private string blendParameterName = "Blend"; // Customizable parameter name
        
        // Components
        private CharacterController _characterController;
        private Animator _animator;
        private Camera _playerCamera;
        private Renderer[] characterRenderers;
        
        // Movement variables
        private Vector3 moveDirection;
        private float currentSpeed;
        private float targetSpeed;
        private float turnSmoothVelocity;
        private float verticalVelocity;
        
        // Camera variables
        private float cameraVerticalRotation;
        private float cameraHorizontalRotation;
        private Vector3 currentCameraPosition;
        private Vector3 targetCameraPosition;
        
        // Input variables
        private Vector2 movementInput;
        private Vector2 mouseInput;
        private bool isRunning;
        
        // Coroutine management
        private Coroutine updateCoroutine;
        private Coroutine cameraCoroutine;
        private bool isApplicationFocused = true;
        private bool forcedFirstPerson = false; // For indoor areas
        private bool lastFirstPersonState = false; // Track visibility changes
        
        // Animation parameters - will be set dynamically based on parameter name
        private int blendHash;
        private readonly int IsMovingHash = Animator.StringToHash("IsMoving");
        
        private void Start()
        {
            InitializeComponents();
            InitializeCamera();
            InitializeCharacterRenderers();
            
            // Force cursor lock on start
            ForceCursorLock();
            
            if (useCoroutineUpdates)
            {
                StartUpdateCoroutines();
                StartCameraCoroutine();
            }
        }
        
        private void InitializeComponents()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            _playerCamera = Camera.main;
            
            if (_characterController == null)
            {
                _characterController = gameObject.AddComponent<CharacterController>();
                Debug.LogWarning("CharacterController was missing and has been added automatically");
            }
            
            if (_animator != null)
            {
                _animator.applyRootMotion = useRootMotion;
                // Set the blend parameter hash based on the parameter name
                blendHash = Animator.StringToHash(blendParameterName);
            }
            
            // Validate components
            if (_characterController == null)
                Debug.LogError("CharacterController component not found!");
            if (_animator == null)
                Debug.LogError("Animator component not found!");
            if (cameraFollowTarget == null)
                Debug.LogError("Camera follow target not assigned!");
        }
        
        private void InitializeCamera()
        {
            if (cameraFollowTarget != null)
            {
                cameraHorizontalRotation = transform.eulerAngles.y;
                cameraVerticalRotation = 20f;
                
                currentCameraPosition = cameraFollowTarget.position;
                targetCameraPosition = currentCameraPosition;
                
                UpdateCameraPosition();
            }
        }
        
        private void InitializeCharacterRenderers()
        {
            // Get all renderers on character for first person hiding
            characterRenderers = GetComponentsInChildren<Renderer>();
        }
        
        private void ForceCursorLock()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        private void StartUpdateCoroutines()
        {
            if (updateCoroutine != null)
                StopCoroutine(updateCoroutine);
            
            updateCoroutine = StartCoroutine(UpdateLoop());
        }
        
        private void StartCameraCoroutine()
        {
            if (cameraCoroutine != null)
                StopCoroutine(cameraCoroutine);
            
            cameraCoroutine = StartCoroutine(CameraUpdateLoop());
        }
        
        private void StopCameraCoroutine()
        {
            if (cameraCoroutine != null)
            {
                StopCoroutine(cameraCoroutine);
                cameraCoroutine = null;
            }
        }
        
        private IEnumerator UpdateLoop()
        {
            float updateInterval = 1f / updateRate;
            
            while (true)
            {
                if (isApplicationFocused)
                {
                    HandleInput();
                    HandleMovement();
                    HandleCameraRotation();
                    HandleAnimations();
                    HandleCharacterVisibility();
                    // Note: Gravity is handled in Update() for immediate response
                }
                
                yield return new WaitForSeconds(updateInterval);
            }
        }
        
        private IEnumerator CameraUpdateLoop()
        {
            float cameraUpdateRate = 60f; // Higher rate for smooth camera
            float updateInterval = 1f / cameraUpdateRate;
            
            while (true)
            {
                if (isApplicationFocused && !forcedFirstPerson)
                {
                    UpdateCameraPosition();
                }
                else if (forcedFirstPerson)
                {
                    // Only update first person camera when forced
                    UpdateFirstPersonCamera();
                }
                
                yield return new WaitForSeconds(updateInterval);
            }
        }
        
        private void Update()
        {
            // Only handle critical input and cursor management in Update
            HandleCriticalInput();
            
            // If not using coroutines, run all updates here
            if (!useCoroutineUpdates)
            {
                HandleInput();
                HandleMovement();
                HandleCameraRotation();
                HandleAnimations();
                HandleCharacterVisibility();
                
                // Handle camera updates
                if (!forcedFirstPerson)
                {
                    UpdateCameraPosition();
                }
                else
                {
                    UpdateFirstPersonCamera();
                }
            }
            
            // Always handle gravity in Update for immediate response
            HandleGravity();
        }
        
        private void HandleCriticalInput()
        {
            // Always handle cursor lock in Update for immediate response
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleCursorLock();
            }
            
            // Re-lock cursor when clicking back into game
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                if (Cursor.lockState != CursorLockMode.Locked)
                {
                    ForceCursorLock();
                }
            }
            
            // Toggle view mode
            if (Input.GetKeyDown(toggleViewKey))
            {
                ToggleViewMode();
            }
            
            // Handle scroll wheel in Update for immediate response
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0f && allowScrollCameraControl && !forcedFirstPerson)
            {
                HandleScrollCameraControl(scroll);
            }
        }
        
        private void HandleInput()
        {
            if (Cursor.lockState != CursorLockMode.Locked)
                return;
            
            // Movement input
            movementInput.x = Input.GetAxisRaw("Horizontal");
            movementInput.y = Input.GetAxisRaw("Vertical");
            
            // Mouse input
            mouseInput.x = Input.GetAxis("Mouse X");
            mouseInput.y = Input.GetAxis("Mouse Y");
            
            // Run input
            isRunning = Input.GetKey(KeyCode.LeftShift);
        }
        
        private void ToggleCursorLock()
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                ForceCursorLock();
            }
        }
        
        private void HandleScrollCameraControl(float scroll)
        {
            float previousDistance = cameraDistance;
            cameraDistance -= scroll * scrollSensitivity;
            
            // Clamp with safer bounds
            cameraDistance = Mathf.Clamp(cameraDistance, 0.5f, maxThirdPersonDistance);
            
            // Only switch modes if distance actually changed significantly
            if (Mathf.Abs(cameraDistance - previousDistance) > 0.1f)
            {
                // Check if we should switch to first person
                if (cameraDistance <= minThirdPersonDistance && !isFirstPerson)
                {
                    isFirstPerson = true;
                    Debug.Log("Switched to First Person (Scroll)");
                }
                // Check if we should switch back to third person
                else if (cameraDistance > minThirdPersonDistance && isFirstPerson)
                {
                    isFirstPerson = false;
                    Debug.Log("Switched to Third Person (Scroll)");
                }
            }
        }
        private void HandleGravity()
        {
            if (_characterController == null)
                return;
            
            // Check if character is grounded
            if (_characterController.isGrounded)
            {
                // When grounded, apply small downward force to maintain contact
                if (verticalVelocity < 0)
                {
                    verticalVelocity = -groundStickForce;
                }
            }
            else
            {
                // Apply gravity when in air
                verticalVelocity -= gravityStrength * Time.deltaTime;
                
                // Clamp to maximum fall speed
                verticalVelocity = Mathf.Max(verticalVelocity, -maxFallSpeed);
            }
            
            // Apply vertical movement
            Vector3 verticalMovement = Vector3.up * verticalVelocity * Time.deltaTime;
            _characterController.Move(verticalMovement);
        }
        private void HandleMovement()
        {
            if (_characterController == null)
                return;
            
            Vector3 inputDirection = new Vector3(movementInput.x, 0f, movementInput.y).normalized;
            
            if (inputDirection.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cameraHorizontalRotation;
                float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);
                
                moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                targetSpeed = isRunning ? (walkSpeed * runSpeedMultiplier) : walkSpeed;
            }
            else
            {
                moveDirection = Vector3.zero;
                targetSpeed = 0f;
            }
            
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, speedChangeRate * Time.deltaTime);
            
            // Apply only horizontal movement (gravity handled separately)
            Vector3 horizontalMovement = moveDirection * currentSpeed * Time.deltaTime;
            _characterController.Move(horizontalMovement);
        }
        
        private void HandleCameraRotation()
        {
            if (Cursor.lockState != CursorLockMode.Locked)
                return;
            
            cameraHorizontalRotation += mouseInput.x * mouseSensitivity;
            
            float mouseY = invertMouseY ? mouseInput.y : -mouseInput.y;
            cameraVerticalRotation += mouseY * mouseSensitivity;
            cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -verticalRotationLimit, verticalRotationLimit);
            
            // Keep horizontal rotation in 0-360 range
            if (cameraHorizontalRotation > 360f)
                cameraHorizontalRotation -= 360f;
            else if (cameraHorizontalRotation < 0f)
                cameraHorizontalRotation += 360f;
        }
        
        private void UpdateCameraPosition()
        {
            if (cameraFollowTarget == null)
                return;
            
            // Ensure we don't have invalid camera distance values
            if (cameraDistance <= 0 || float.IsNaN(cameraDistance) || float.IsInfinity(cameraDistance))
            {
                cameraDistance = 5f; // Reset to safe default
            }
            
            if (isFirstPerson)
            {
                UpdateFirstPersonCamera();
            }
            else
            {
                UpdateThirdPersonCamera();
            }
        }
        
        private void UpdateFirstPersonCamera()
        {
            // Calculate first person position with customizable offset
            Vector3 characterHeadPosition = transform.position + Vector3.up * firstPersonHeight;
            
            // Apply the customizable offset (forward to prevent clipping)
            Vector3 forwardOffset = transform.TransformDirection(firstPersonOffset);
            targetCameraPosition = characterHeadPosition + forwardOffset;
            
            // Check for clipping and adjust position
            Vector3 adjustedPosition = HandleFirstPersonClipping(characterHeadPosition, targetCameraPosition);
            
            currentCameraPosition = Vector3.Lerp(currentCameraPosition, adjustedPosition, cameraFollowSpeed * Time.deltaTime);
            
            cameraFollowTarget.position = currentCameraPosition;
            cameraFollowTarget.rotation = Quaternion.Euler(cameraVerticalRotation, cameraHorizontalRotation, 0f);
        }
        
        private void UpdateThirdPersonCamera()
        {
            Vector3 characterPosition = transform.position + Vector3.up * cameraHeight;
            
            // Calculate camera position with stable distance
            float safeDistance = Mathf.Max(cameraDistance, 0.5f); // Ensure minimum distance
            Vector3 direction = new Vector3(0, 0, -safeDistance);
            Quaternion rotation = Quaternion.Euler(cameraVerticalRotation, cameraHorizontalRotation, 0);
            
            Vector3 sideOffset = rotation * Vector3.right * cameraSideOffset;
            targetCameraPosition = characterPosition + rotation * direction + sideOffset;
            
            Vector3 finalPosition = HandleCameraCollision(characterPosition, targetCameraPosition);
            
            // Smooth camera movement with consistent speed
            float smoothSpeed = cameraFollowSpeed * Time.deltaTime;
            currentCameraPosition = Vector3.Lerp(currentCameraPosition, finalPosition, smoothSpeed);
            
            // Apply position and rotation
            cameraFollowTarget.position = currentCameraPosition;
            
            // Calculate look direction safely
            Vector3 lookDirection = (characterPosition - currentCameraPosition);
            if (lookDirection.sqrMagnitude > 0.01f) // Avoid near-zero vectors
            {
                cameraFollowTarget.rotation = Quaternion.LookRotation(lookDirection.normalized);
            }
        }
        
        private Vector3 HandleFirstPersonClipping(Vector3 characterPos, Vector3 desiredCameraPos)
        {
            // Cast a ray backward from desired position to check for clipping
            Vector3 direction = (desiredCameraPos - characterPos).normalized;
            float distance = Vector3.Distance(characterPos, desiredCameraPos);
            
            if (Physics.Raycast(characterPos, direction, out RaycastHit hit, distance + firstPersonClipDistance, characterLayer))
            {
                // Move camera forward to prevent clipping
                return hit.point + direction * firstPersonClipDistance;
            }
            
            return desiredCameraPos;
        }
        
        private Vector3 HandleCameraCollision(Vector3 characterPos, Vector3 desiredCameraPos)
        {
            Vector3 direction = (desiredCameraPos - characterPos).normalized;
            float distance = Vector3.Distance(characterPos, desiredCameraPos);
            
            if (Physics.Raycast(characterPos, direction, out RaycastHit hit, distance, cameraCollisionLayers))
            {
                return hit.point - direction * 0.2f;
            }
            
            return desiredCameraPos;
        }
        
        private void HandleCharacterVisibility()
        {
            if (!hideCharacterInFirstPerson || characterRenderers == null)
                return;
            
            // Only change visibility when switching modes to prevent flickering
            if (lastFirstPersonState != isFirstPerson)
            {
                foreach (Renderer renderer in characterRenderers)
                {
                    if (renderer != null)
                    {
                        renderer.enabled = !isFirstPerson;
                    }
                }
                lastFirstPersonState = isFirstPerson;
            }
        }
        
        private void HandleAnimations()
        {
            if (_animator == null)
                return;
            
            // Calculate normalized speed based on actual movement speed
            float normalizedSpeed = 0f;
            float runSpeed = walkSpeed * runSpeedMultiplier;
            
            if (currentSpeed > 0.1f)
            {
                if (isRunning && currentSpeed > walkSpeed * 0.8f)
                {
                    // Map from walk speed to run speed onto 0.5 to 1.0 range
                    float runProgress = Mathf.InverseLerp(walkSpeed, runSpeed, currentSpeed);
                    normalizedSpeed = Mathf.Lerp(0.5f, 1.0f, runProgress);
                }
                else
                {
                    // Map from 0 to walk speed onto 0.0 to 0.5 range
                    float walkProgress = Mathf.InverseLerp(0f, walkSpeed, currentSpeed);
                    normalizedSpeed = Mathf.Lerp(0f, 0.5f, walkProgress);
                }
            }
            
            // Smooth animation transitions
            float currentAnimSpeed = _animator.GetFloat(blendHash);
            float blendSpeed = isRunning ? animationBlendSpeed * 2f : animationBlendSpeed;
            float smoothedAnimSpeed = Mathf.Lerp(currentAnimSpeed, normalizedSpeed, blendSpeed * Time.deltaTime);
            
            // Set animator parameters - only if they exist
            try
            {
                _animator.SetFloat(blendHash, smoothedAnimSpeed);
                _animator.SetBool(IsMovingHash, currentSpeed > 0.1f);
            }
            catch (System.Exception)
            {
                // Parameter doesn't exist - silently continue
            }
            
            // Adjust animation speed for running
            if (isRunning && currentSpeed > walkSpeed * 0.8f)
            {
                _animator.speed = runAnimationSpeedMultiplier;
            }
            else
            {
                _animator.speed = 1f;
            }
        }
        
        private void ToggleViewMode()
        {
            isFirstPerson = !isFirstPerson;
            
            if (isFirstPerson)
            {
                Debug.Log("Switched to First Person View");
            }
            else
            {
                Debug.Log("Switched to Third Person View");
                if (cameraVerticalRotation < -60f || cameraVerticalRotation > 60f)
                {
                    cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -60f, 60f);
                }
            }
        }
        
        private void OnApplicationFocus(bool hasFocus)
        {
            isApplicationFocused = hasFocus;
            if (hasFocus)
            {
                ForceCursorLock();
            }
        }
        
        private void OnApplicationPause(bool pauseStatus)
        {
            isApplicationFocused = !pauseStatus;
        }
        
        // Handle root motion if enabled
        private void OnAnimatorMove()
        {
            if (!useRootMotion || _animator == null)
                return;
            
            Vector3 rootMotionDelta = _animator.deltaPosition;
            if (_characterController != null && rootMotionDelta.magnitude > 0.01f)
            {
                _characterController.Move(rootMotionDelta);
            }
            
            transform.rotation *= _animator.deltaRotation;
        }
        
        // Public methods for adjusting gravity at runtime
        public void SetGravityStrength(float gravity)
        {
            gravityStrength = gravity;
        }
        
        public void SetGroundStickForce(float stickForce)
        {
            groundStickForce = stickForce;
        }
        
        public void SetMaxFallSpeed(float maxSpeed)
        {
            maxFallSpeed = maxSpeed;
        }
        
        public bool IsGrounded()
        {
            return _characterController != null ? _characterController.isGrounded : false;
        }
        public void ForceFirstPerson(bool force)
        {
            forcedFirstPerson = force;
            if (force)
            {
                isFirstPerson = true;
                StopCameraCoroutine(); // Stop third person camera updates
                Debug.Log("Forced First Person Mode (Indoor)");
            }
            else
            {
                if (useCoroutineUpdates)
                {
                    StartCameraCoroutine(); // Resume camera coroutine
                }
                Debug.Log("Released First Person Force (Outdoor)");
            }
        }
        
        public void SetScrollCameraControl(bool enabled)
        {
            allowScrollCameraControl = enabled;
        }
        
        public bool IsInFirstPerson()
        {
            return isFirstPerson || forcedFirstPerson;
        }
        public bool IsMoving => currentSpeed > 0.1f;
        public bool IsRunning => isRunning && IsMoving;
        public float CurrentSpeed => currentSpeed;
        public Vector3 MoveDirection => moveDirection;
        public bool IsFirstPerson => isFirstPerson;
        
        // Method to adjust movement settings at runtime
        public void SetWalkSpeed(float speed)
        {
            walkSpeed = speed;
        }
        
        public void SetRunSpeedMultiplier(float multiplier)
        {
            runSpeedMultiplier = multiplier;
        }
        
        public float GetRunSpeed()
        {
            return walkSpeed * runSpeedMultiplier;
        }
        public void SetCameraDistance(float distance)
        {
            cameraDistance = Mathf.Clamp(distance, 0.1f, maxThirdPersonDistance);
            
            // Check if this should trigger first/third person switch
            if (cameraDistance <= minThirdPersonDistance && !isFirstPerson && !forcedFirstPerson)
            {
                isFirstPerson = true;
            }
            else if (cameraDistance > minThirdPersonDistance && isFirstPerson && !forcedFirstPerson)
            {
                isFirstPerson = false;
            }
        }
        
        public void SetCameraHeight(float height)
        {
            cameraHeight = height;
        }
        
        public void SetCameraSideOffset(float offset)
        {
            cameraSideOffset = offset;
        }
        
        public void SetFirstPerson(bool firstPerson)
        {
            isFirstPerson = firstPerson;
        }
        
        public void SetFirstPersonOffset(Vector3 offset)
        {
            firstPersonOffset = offset;
        }
        
        // Optional: Gizmos for debugging
        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying && cameraFollowTarget != null)
            {
                // Draw movement direction
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(transform.position, moveDirection * 2f);
                
                // Draw current velocity
                Gizmos.color = Color.red;
                if (_characterController != null)
                    Gizmos.DrawRay(transform.position, _characterController.velocity);
                
                // Draw camera target position
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(targetCameraPosition, 0.3f);
                
                // Draw ground detection
                bool isGrounded = _characterController != null ? _characterController.isGrounded : false;
                Gizmos.color = isGrounded ? Color.green : Color.red;
                Gizmos.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down * 0.3f);
                
                // Draw CharacterController bounds
                if (_characterController != null)
                {
                    Gizmos.color = Color.yellow;
                    Vector3 center = transform.position + _characterController.center;
                    Gizmos.DrawWireCube(center, new Vector3(_characterController.radius * 2, _characterController.height, _characterController.radius * 2));
                }
                
                // Draw camera distance thresholds
                if (!isFirstPerson)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawWireSphere(transform.position, minThirdPersonDistance);
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawWireSphere(transform.position, maxThirdPersonDistance);
                }
                
                // Draw first person clipping detection
                if (isFirstPerson)
                {
                    Gizmos.color = Color.cyan;
                    Vector3 headPos = transform.position + Vector3.up * firstPersonHeight;
                    Gizmos.DrawWireSphere(headPos, firstPersonClipDistance);
                }
            }
        }
    }
}