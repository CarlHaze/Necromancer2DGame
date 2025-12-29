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
    
    void Update()
    {
        if (player == null) return;
        
        // Calculate distance to player (2D)
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        // Only move if beyond the follow distance
        if (distanceToPlayer > followDistance)
        {
            // Calculate target position
            Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
            Vector2 targetPosition = (Vector2)player.position - direction * (followDistance - stopDistance);
            
            // Smooth movement (2D)
            Vector2 newPosition = Vector2.SmoothDamp(
                transform.position, 
                targetPosition, 
                ref velocity, 
                smoothTime, 
                moveSpeed
            );
            
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        }
    }
}