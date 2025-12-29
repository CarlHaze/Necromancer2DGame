using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private float followDistance = 2f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float stopDistance = 0.5f;
    
    [Header("Smoothing")]
    [SerializeField] private float smoothTime = 0.3f;
    
    private Vector2 velocity = Vector2.zero;
    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0; // Disable gravity for top-down games
        }
    }
    
    void FixedUpdate() // Use FixedUpdate with Rigidbody2D
    {
        if (player == null) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (distanceToPlayer > followDistance)
        {
            Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
            Vector2 targetPosition = (Vector2)player.position - direction * (followDistance - stopDistance);
            
            Vector2 newPosition = Vector2.SmoothDamp(
                rb.position, 
                targetPosition, 
                ref velocity, 
                smoothTime, 
                moveSpeed
            );
            
            rb.MovePosition(newPosition); // Use MovePosition instead of direct transform
        }
    }
}