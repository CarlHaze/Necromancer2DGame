using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Vector2 Movement;
    private PlayerInput playerInput;
    private InputAction moveAction;

    public void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
    }

    public void Update()
    {
        Movement = moveAction.ReadValue<Vector2>();
    }

}
