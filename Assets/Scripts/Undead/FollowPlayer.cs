using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private Transform followTarget; // The object THIS pet follows
    [SerializeField] private float followDistance = 1.5f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float stopDistance = 0.5f;
    
    [Header("Smoothing")]
    [SerializeField] private float smoothTime = 0.3f;
    
    private Vector2 velocity = Vector2.zero;
    
    // Public method so other pets can follow this one
    public void SetFollowTarget(Transform target)
    {
        followTarget = target;
    }
    
    void Update()
    {
        if (followTarget == null) return;
        
        float distanceToTarget = Vector2.Distance(transform.position, followTarget.position);
        
        if (distanceToTarget > followDistance)
        {
            Vector2 direction = ((Vector2)followTarget.position - (Vector2)transform.position).normalized;
            Vector2 targetPosition = (Vector2)followTarget.position - direction * (followDistance - stopDistance);
            
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