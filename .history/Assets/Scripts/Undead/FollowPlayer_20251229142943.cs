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
    
    private Vector3 velocity = Vector3.zero;
    
    void Update()
    {
        if (player == null) return;
        
        // Calculate distance to player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        // Only move if beyond the follow distance
        if (distanceToPlayer > followDistance)
        {
            // Calculate direction to player
            Vector3 direction = (player.position - transform.position).normalized;
            
            // Calculate target position (follow distance behind player)
            Vector3 targetPosition = player.position - direction * (followDistance - stopDistance);
            
            // Smooth movement
            transform.position = Vector3.SmoothDamp(
                transform.position, 
                targetPosition, 
                ref velocity, 
                smoothTime, 
                moveSpeed
            );
            
            // Optional: Face the direction of movement
            if (velocity.magnitude > 0.1f)
            {
                transform.rotation = Quaternion.LookRotation(velocity.normalized);
            }
        }
    }
}