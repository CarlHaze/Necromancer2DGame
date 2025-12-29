using UnityEngine;

namespace NecromancersRising.Player
{
    /// <summary>
    /// Debug helper to visualize player movement.
    /// Attach this alongside NecromancerController to see if movement is working.
    /// REMOVE THIS SCRIPT once you confirm movement works!
    /// </summary>
    [RequireComponent(typeof(NecromancerController))]
    public class MovementDebugger : MonoBehaviour
    {
        [Header("Debug Visualization")]
        [SerializeField]
        [Tooltip("Change sprite color when moving?")]
        private bool colorOnMove = true;
        
        [SerializeField]
        [Tooltip("Color when moving")]
        private Color movingColor = Color.green;
        
        [SerializeField]
        [Tooltip("Color when idle")]
        private Color idleColor = Color.white;
        
        [SerializeField]
        [Tooltip("Log movement to console?")]
        private bool logMovement = true;
        
        [SerializeField]
        [Tooltip("Draw movement direction arrow in Scene view?")]
        private bool drawDirectionGizmo = true;
        
        private NecromancerController controller;
        private SpriteRenderer spriteRenderer;
        private Rigidbody2D rb;
        private Vector3 lastPosition;
        private float distanceMoved = 0f;
        
        private void Awake()
        {
            controller = GetComponent<NecromancerController>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            lastPosition = transform.position;
        }
        
        private void Update()
        {
            // Calculate actual movement
            float frameDistance = Vector3.Distance(transform.position, lastPosition);
            distanceMoved += frameDistance;
            lastPosition = transform.position;
            
            // Check if moving
            bool isMoving = controller.IsMoving();
            
            // Change color
            if (colorOnMove && spriteRenderer != null)
            {
                spriteRenderer.color = isMoving ? movingColor : idleColor;
            }
            
            // Log to console
            if (logMovement && isMoving)
            {
                Vector2 facing = controller.GetFacingDirection();
                string direction = GetDirectionName(facing);
                
                Debug.Log($"[Movement] Direction: {direction} | Velocity: {rb.velocity} | Total Distance: {distanceMoved:F2} units");
            }
        }
        
        private void OnGUI()
        {
            // Draw on-screen info
            GUIStyle style = new GUIStyle();
            style.fontSize = 16;
            style.normal.textColor = Color.white;
            style.padding = new RectOffset(10, 10, 10, 10);
            
            string info = "=== MOVEMENT DEBUG ===\n";
            info += $"Position: {transform.position}\n";
            info += $"Velocity: {rb.velocity}\n";
            info += $"Is Moving: {controller.IsMoving()}\n";
            info += $"Facing: {GetDirectionName(controller.GetFacingDirection())}\n";
            info += $"Total Distance: {distanceMoved:F2} units\n";
            info += "\nControls: WASD or Arrow Keys\n";
            info += "(Remove MovementDebugger script when done testing)";
            
            GUI.Label(new Rect(10, 10, 400, 200), info, style);
        }
        
        private void OnDrawGizmos()
        {
            if (!drawDirectionGizmo || controller == null) return;
            
            // Draw direction arrow
            Vector2 facing = controller.GetFacingDirection();
            if (facing != Vector2.zero)
            {
                Gizmos.color = Color.yellow;
                Vector3 start = transform.position;
                Vector3 end = transform.position + (Vector3)facing;
                
                // Draw arrow
                Gizmos.DrawLine(start, end);
                Gizmos.DrawSphere(end, 0.1f);
            }
            
            // Draw velocity vector
            if (rb != null && rb.velocity.magnitude > 0.1f)
            {
                Gizmos.color = Color.cyan;
                Vector3 start = transform.position;
                Vector3 end = transform.position + (Vector3)rb.velocity.normalized;
                Gizmos.DrawLine(start, end);
            }
        }
        
        private string GetDirectionName(Vector2 direction)
        {
            if (direction == Vector2.zero) return "IDLE";
            if (direction == Vector2.up) return "UP";
            if (direction == Vector2.down) return "DOWN";
            if (direction == Vector2.left) return "LEFT";
            if (direction == Vector2.right) return "RIGHT";
            return direction.ToString();
        }
    }
}