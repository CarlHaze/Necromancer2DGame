using UnityEngine;

namespace NecromancersRising.Player
{
    /// <summary>
    /// Basic 2D character controller for the Necromancer player character.
    /// Handles 4-directional movement (up, down, left, right) with no diagonal movement.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class NecromancerController : MonoBehaviour
    {
        #region Serialized Fields
        
        [Header("Movement Settings")]
        [SerializeField]
        [Tooltip("Movement speed in units per second")]
        private float moveSpeed = 5f;
        
        [Header("Animation Settings (Optional)")]
        [SerializeField]
        [Tooltip("Reference to the Animator component if using animations")]
        private Animator animator;
        
        #endregion
        
        #region Private Fields
        
        private Rigidbody2D rb;
        private Vector2 movement;
        private Vector2 lastMovement; // Track last movement for facing direction
        
        // Animation parameter names (if using animations later)
        private const string ANIM_HORIZONTAL = "Horizontal";
        private const string ANIM_VERTICAL = "Vertical";
        private const string ANIM_SPEED = "Speed";
        
        #endregion
        
        #region Unity Lifecycle
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            
            // Configure Rigidbody2D for top-down movement
            rb.gravityScale = 0f; // No gravity in top-down view
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Prevent rotation
            
            // Get animator if it exists
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
        }
        
        private void Update()
        {
            // Get input from keyboard
            GetInput();
        }
        
        private void FixedUpdate()
        {
            // Apply movement in FixedUpdate for physics consistency
            Move();
        }
        
        #endregion
        
        #region Movement Methods
        
        /// <summary>
        /// Gets player input and determines movement direction.
        /// Prevents diagonal movement by prioritizing vertical over horizontal.
        /// </summary>
        private void GetInput()
        {
            float horizontal = Input.GetAxisRaw("Horizontal"); // -1, 0, or 1
            float vertical = Input.GetAxisRaw("Vertical");     // -1, 0, or 1
            
            // Prevent diagonal movement: prioritize vertical input over horizontal
            if (vertical != 0)
            {
                movement = new Vector2(0, vertical);
            }
            else if (horizontal != 0)
            {
                movement = new Vector2(horizontal, 0);
            }
            else
            {
                movement = Vector2.zero;
            }
            
            // Track last non-zero movement for facing direction
            if (movement != Vector2.zero)
            {
                lastMovement = movement;
            }
            
            // Update animations if animator exists
            UpdateAnimations();
        }
        
        /// <summary>
        /// Applies movement to the Rigidbody2D.
        /// </summary>
        private void Move()
        {
            // Calculate velocity
            Vector2 velocity = movement.normalized * moveSpeed;
            
            // Apply velocity to rigidbody
            rb.linearVelocity = velocity;
        }
        
        #endregion
        
        #region Animation Methods
        
        /// <summary>
        /// Updates animator parameters based on movement.
        /// Only runs if an Animator component is attached.
        /// </summary>
        private void UpdateAnimations()
        {
            if (animator == null) return;
            
            // Set movement direction parameters
            animator.SetFloat(ANIM_HORIZONTAL, lastMovement.x);
            animator.SetFloat(ANIM_VERTICAL, lastMovement.y);
            
            // Set speed parameter (0 = idle, 1 = moving)
            animator.SetFloat(ANIM_SPEED, movement.magnitude);
        }
        
        #endregion
        
        #region Public Methods
        
        /// <summary>
        /// Gets the current facing direction of the character.
        /// Useful for spawning projectiles, determining interaction direction, etc.
        /// </summary>
        public Vector2 GetFacingDirection()
        {
            return lastMovement;
        }
        
        /// <summary>
        /// Gets whether the character is currently moving.
        /// </summary>
        public bool IsMoving()
        {
            return movement.magnitude > 0;
        }
        
        /// <summary>
        /// Enables or disables character movement.
        /// Useful for cutscenes, dialogues, or combat states.
        /// </summary>
        public void SetMovementEnabled(bool enabled)
        {
            if (!enabled)
            {
                movement = Vector2.zero;
                rb.velocity = Vector2.zero;
            }
            this.enabled = enabled;
        }
        
        #endregion
        
        #region Editor Utilities
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            // Ensure move speed is never negative
            if (moveSpeed < 0)
            {
                moveSpeed = 0;
            }
        }
        #endif
        
        #endregion
    }
}