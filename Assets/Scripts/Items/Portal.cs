using UnityEngine;
using static PlayerSelectableInterface;

public class Portal : ItemBehavior
{
    public Transform linkedPortal; // Reference to the linked portal
    private bool canTeleport = true;
    private float cooldownTime = 0.5f; // Cooldown to prevent repeated teleportation

    protected override void Awake()
    {
        base.Awake();
        PlayerTriggerEvent += PortalActivate;
    }

    private void PortalActivate(Collider other)
    {
        if (!canTeleport) return;

        Movable player = other.GetComponent<Movable>();
        if (player != null && linkedPortal != null)
        {
            // Get the player's rigidbody
            Rigidbody playerRb = player.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // Teleport the player to the linked portal's position
                player.transform.position = linkedPortal.position;

                // Start cooldown
                canTeleport = false;
                Invoke("ResetCooldown", cooldownTime);
            }
        }
    }

    private void ResetCooldown()
    {
        canTeleport = true;
    }

}