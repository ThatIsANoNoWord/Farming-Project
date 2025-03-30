using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerControl controlInput;
    public float movementSpeed;
    public float interactCooldown;
    public GameObject cropPickupPrefab;
    public GameObject compostPickupPrefab;
    Vector2 moveDirection;
    Rigidbody2D rb;
    HELD playerHolding;
    HeldUI heldUI;
    Interactable closestInteract;
    PlantData heldPlantData;
    Animator animator;
    float spamPrevention;
    int holdingCount;

    // Start is called before the first frame update
    void Awake()
    {
        controlInput = new PlayerControl();
        controlInput.Player.Enable();
        controlInput.Player.Movement.performed += MoveUpdate;
        controlInput.Player.Movement.canceled += StopMove;
        controlInput.Player.Interact.performed += Interact;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        heldUI = FindObjectOfType<HeldUI>();
        spamPrevention = 0;
        holdingCount = 0;
    }

    void MoveUpdate(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
        animator.SetFloat("xMov", moveDirection.x);
        animator.SetFloat("yMov", moveDirection.y);
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

    public HELD GetHeld()
    {
        return playerHolding;
    }

    public int GetHeldQuant()
    {
        return holdingCount;
    }

    public PlantData GetHeldPlantData()
    {
        return heldPlantData;
    }

    public void ChangeHeld(HELD newHeld, PlantData seedHeld, Sprite spriteHeld, int countHeld)
    {
        // What to do if player picking up something they already have
        if (playerHolding == newHeld)
        {
            // If taking same seed of same type, hold nothing
            // Otherwise switch seed
            switch (playerHolding)
            {
                case HELD.SEED:
                    heldPlantData = seedHeld;
                    if(countHeld > 0) holdingCount = countHeld;
                    heldUI.ChangeHeld(spriteHeld, countHeld);
                    return;
                case HELD.CROP:
                    if (seedHeld == heldPlantData)
                    {
                        holdingCount += countHeld;
                        heldUI.ChangeHeld(spriteHeld, holdingCount);
                        return;
                    }
                    GameObject droppedCrop = Instantiate(cropPickupPrefab, transform.position, Quaternion.identity);
                    droppedCrop.GetComponent<CropPickupable>().SetPlantType(heldPlantData, holdingCount);
                    heldPlantData = seedHeld;
                    holdingCount = countHeld;
                    heldUI.ChangeHeld(spriteHeld, countHeld);
                    return;
                case HELD.COMPOST:
                    holdingCount += countHeld;
                    heldUI.ChangeHeld(spriteHeld, holdingCount);
                    return;
                default:
                    // Holding nothing
                    holdingCount = 0;
                    heldUI.ChangeHeld(null, 0);
                    return;
            }
        }

        // Drop your crop
        if (playerHolding == HELD.CROP)
        {
            GameObject droppedCrop = Instantiate(cropPickupPrefab, transform.position, Quaternion.identity);
            droppedCrop.GetComponent<CropPickupable>().SetPlantType(heldPlantData, holdingCount);
            heldPlantData = seedHeld;
            holdingCount = countHeld;
            heldUI.ChangeHeld(spriteHeld, countHeld);
            playerHolding = newHeld;
            return;
        }

        // Drop the compost
        if (playerHolding == HELD.COMPOST)
        {
            GameObject droppedCrop = Instantiate(compostPickupPrefab, transform.position, Quaternion.identity);
            droppedCrop.GetComponent<CompostPickupable>().InitialSpecial(holdingCount);
            heldPlantData = seedHeld;
            holdingCount = countHeld;
            heldUI.ChangeHeld(spriteHeld, countHeld);
            playerHolding = newHeld;
            return;
        }

        playerHolding = newHeld;
        heldPlantData = seedHeld;
        holdingCount = countHeld;
        heldUI.ChangeHeld(spriteHeld, countHeld);
    }
    public void HoldNothing()
    {
        ChangeHeld(HELD.NOTHING, null, null, 0);
    }
    public void DecrementHeld()
    {
        holdingCount--;
        if (holdingCount <= 0)
        {
            HoldNothing();
            return;
        }
        ChangeHeld(playerHolding, heldPlantData, heldUI.CurrentSprite(), 0);
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
    NOTHING, SEED, COMPOST, CROP
}