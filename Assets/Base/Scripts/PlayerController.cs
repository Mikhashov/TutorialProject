using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    private PlayerInputHandler inputHandler;
    private PlayerMovement playerMovement;
    private PlayerCameraController cameraController;


    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        playerMovement = GetComponent<PlayerMovement>();
        cameraController = FindFirstObjectByType<PlayerCameraController>();

    }

    private void Update()
    {
        // Передаем управление движению
        playerMovement.Move(inputHandler.MoveInput, inputHandler.JumpInput);
        cameraController.Look(inputHandler.LookInput);

    }
}