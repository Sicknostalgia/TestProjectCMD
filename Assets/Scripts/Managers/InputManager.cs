using UnityEngine;
using UniRx;
using System;
public class InputManager : MonoBehaviour
{
    public static InputManager Inspance { get; private set; }
    [field: SerializeField] private PlayerInput playerInput;
    private Subject<Vector2> moveSubject = new Subject<Vector2>();
    private Subject<Vector2> mousePosSubject = new Subject<Vector2>(); // To track mouse position
    private Subject<Vector2> attackSubject = new Subject<Vector2>();

    public IObservable<Vector2> OnMoveInput => moveSubject;
    public IObservable<Vector2> OnMousePosition => mousePosSubject;
    public IObservable<Vector2> OnAttackPressed => attackSubject;

    private void Awake()
    {
        Inspance = this;
        playerInput = new PlayerInput(); // Assuming you're using Unity's new Input System
        SetupInputActions();
    }
    public void SetupInputActions()
    {
        if (playerInput != null)
        {
            playerInput.Enable();
        }
        playerInput.Gameplay.Movement.performed += ctx =>
            moveSubject.OnNext(ctx.ReadValue<Vector2>());

        playerInput.Gameplay.Movement.canceled += ctx =>
     moveSubject.OnNext(Vector2.zero); // Reset movement on key release   IF NOT THIS CAUSE CONSTANT MOVEMENT  applicable to held action


        playerInput.Gameplay.MousePos.performed += ctx =>
        {
            Vector2 mousePosition = ctx.ReadValue<Vector2>();  //getmouse position in screenspace
            mousePosSubject.OnNext(mousePosition);  //send mouse pos to subscriber
        };

        playerInput.Gameplay.Combat.performed += ctx =>
        {
            // Get mouse position on attack press
            Vector2 screenPos = playerInput.Gameplay.MousePos.ReadValue<Vector2>();
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos); // Convert to world position
            Debug.Log("Mouse Position on Attack: " + worldPos); // Debug log
            attackSubject.OnNext(worldPos);  // Pass mouse world position on attack
        };
        GameManager.Instance.OnPlayerDied.Subscribe(_ => ReactOnPlayerDied()).AddTo(this);
    }

    void ReactOnPlayerDied()
    {
        EnableUIInput();
    }
    public void EnableGameplayInput()
    {
        playerInput.OffGameplay.Disable();
        playerInput.Gameplay.Enable();
    }

    public void EnableUIInput()
    {
        playerInput.Gameplay.Disable();
        playerInput.OffGameplay.Enable();
    }

    private void OnApplicationQuit()
    {
        playerInput.Disable();
    }
}

