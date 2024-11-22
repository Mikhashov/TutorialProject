using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public bool JumpInput { get; private set; }
    public Vector2 LookInput { get; private set; }


    private void OnEnable()
    {
        var playerInput = new InputSystem_Actions(); // Автогенерируемый класс на основе Input Actions
        playerInput.Player.Enable();

        playerInput.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        playerInput.Player.Move.canceled += ctx => MoveInput = Vector2.zero;

        playerInput.Player.Jump.performed += ctx => JumpInput = true;
        playerInput.Player.Jump.canceled += ctx => JumpInput = false;
        
        // Камера
        playerInput.Player.Look.performed += ctx => LookInput = ctx.ReadValue<Vector2>();
        playerInput.Player.Look.canceled += ctx => LookInput = Vector2.zero;
    }

    private void OnDisable()
    {
        // Отключаем события, чтобы избежать утечек
        var playerInput = new InputSystem_Actions();
        playerInput.Player.Disable();
    }
}
