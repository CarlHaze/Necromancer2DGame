using UnityEngine;

public class PetManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private FollowPlayer[] pets; // Assign all your pets in order
    
    void Start()
    {
        if (pets.Length == 0) return;
        
        // First pet follows player
        pets[0].SetFollowTarget(player);
        
        // Each subsequent pet follows the previous pet
        for (int i = 1; i < pets.length; i++)
        {
            pets[i].SetFollowTarget(pets[i - 1].transform);
        }
    }
}