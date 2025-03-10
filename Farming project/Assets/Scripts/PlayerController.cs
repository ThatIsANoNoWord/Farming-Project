using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerControl controlInput;
    public float movementSpeed;
    public float interactCooldown;
    Vector2 moveDirection;
    Rigidbody2D rb;
    HELD playerHolding;
    Interactable closestInteract;
    PlantData heldPlantData;
    float spamPrevention;

    // Start is called before the first frame update
    void Awake()
    {
        controlInput = new PlayerControl();
        controlInput.Player.Enable();
        controlInput.Player.Movement.performed += MoveUpdate;
        controlInput.Player.Movement.canceled += StopMove;
        controlInput.Player.Interact.performed += Interact;
        rb = GetComponent<Rigidbody2D>();
        spamPrevention = 0;
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
        closestInteract.OnInteract(playerHolding, heldPlantData);
    }

    void ChangeHeld(HELD newHeld)
    {
        playerHolding = newHeld;
    }

    public void ChangeClosestInteract(Interactable newInteract)
    {
        if (interactCooldown > 0) return;
        closestInteract = newInteract;
    }
    public void TogglePlayerControl()
    {
        if(controlInput.Player.enabled)
        {
            controlInput.Player.Disable();
        }
        else
        {
            controlInput.Player.Enable();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = moveDirection.normalized * Time.fixedDeltaTime * movementSpeed;
        spamPrevention = Mathf.Clamp(spamPrevention - Time.fixedDeltaTime, 0, float.MaxValue);
    }
}

public enum HELD {
    NOTHING, SEED, COMPOST
}