using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 3;
    [SerializeField] Camera cam;

    Vector2 mvmnt;
    Vector2 msPos;

    Rigidbody2D rigBod;
    public bool isMoving => mvmnt != Vector2.zero;
  //  public bool isDashing { get; private set; } // Optional: Set this elsewhere


    private void Awake()
    {
      //  plyrInpt = new PlayerInput();
        rigBod = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        InputManager.Inspance.OnMoveInput.Subscribe(OnMovement).AddTo(this);
        InputManager.Inspance.OnMousePosition.Subscribe(OnMousePos).AddTo(this);

    }

    private void OnMovement(Vector2 movement)
    {
        mvmnt = movement;
       // mvmnt = context.ReadValue<Vector2>();
    }

    private void OnMousePos(Vector2 mousePos)
    {
        msPos = cam.ScreenToWorldPoint(mousePos);
      //  msPos = cam.ScreenToWorldPoint(context.ReadValue<Vector2>());
    }

    private void FixedUpdate()
    {
        if (mvmnt != Vector2.zero)
        {
            rigBod.MovePosition(rigBod.position + mvmnt * speed * Time.deltaTime);
        }
        Vector2 facingDirection = msPos - rigBod.position;
        /*float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
        rigBod.MoveRotation(angle);*/
    }
}
