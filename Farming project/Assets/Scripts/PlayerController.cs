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
    HeldUI heldUI;
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
        heldUI = FindObjectOfType<HeldUI>();
        spamPrevention = 0;
    }

    void MoveUpdate(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }
    void StopMove(InputAction.CallbackContext context)
    {
        moveDirection = Vector2.zero;
    }

    void Interact(InputAction.CallbackContext context)
    {
        if (spamPrevention > 0) return;
        if (closestInteract == null) return;
        closestInteract.OnInteract(playerHolding, heldPlantData);
        spamPrevention = interactCooldown;
    }

    public void ChangeHeld(HELD newHeld, PlantData seedHeld, Sprite spriteHeld, int countHeld)
    {
        playerHolding = newHeld;
        if (playerHolding == HELD.SEED)
        {
            heldPlantData = seedHeld;
            heldUI.ChangeHeld(spriteHeld, countHeld);
        } else
        {
            heldPlantData = null;
            heldUI.ChangeHeld(spriteHeld, countHeld);
        }
    }
    public void ChangeHeld(HELD newHeld, PlantData seedHeld, int countHeld)
    {
        ChangeHeld(newHeld, seedHeld, seedHeld.seedSprite, countHeld);
    }

    public void ChangeClosestInteract(Interactable newInteract)
    {
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
    public void SetPlayerControl(bool set)
    {
        if (controlInput.Player.enabled != set)
        {
            TogglePlayerControl();
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