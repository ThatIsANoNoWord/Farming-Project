using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerControl controlInput;
    public float movementSpeed;
    Vector2 moveDirection;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake()
    {
        controlInput = new PlayerControl();
        controlInput.Player.Enable();
        controlInput.Player.Movement.performed += MoveUpdate;
        controlInput.Player.Movement.canceled += StopMove;
        controlInput.Player.Interact.performed += Interact;
        rb = GetComponent<Rigidbody2D>();
    }

    void MoveUpdate(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }
    void StopMove(InputAction.CallbackContext context)
    {
        moveDirection = Vector2.zero;
        Debug.Log("E");
    }

    void Interact(InputAction.CallbackContext context)
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = moveDirection.normalized * Time.fixedDeltaTime * movementSpeed;
    }
}
