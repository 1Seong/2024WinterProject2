using UnityEngine;

public class PortalTest : ItemBehavior
{
    public Transform linkedPortal; // Reference to the linked portal (PortalOut for PortalIn, and vice versa)
    private NonConsum nonConsum;
    private bool canTeleport = true;
    private float cooldownTime = 0.5f; // Cooldown to prevent repeated teleportation

    private void Start()
    {
        nonConsum = GetComponent<NonConsum>();
        isConsumable = false;
    }

    new void OnTriggerEnter(Collider other)
    {
        Debug.Log("child triggered!");
        PortalActivate(other);
        base.OnTriggerEnter(other);
    }

    private void PortalActivate(Collider other)
    {
        if (!canTeleport) return;

        Player player = other.GetComponent<Player>();
        if (player != null && nonConsum.type == NonConsum.Type.PortalIn && linkedPortal != null)
        {
            // Get the player's rigidbody
            Rigidbody playerRb = player.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // Store current velocity
                Vector3 currentVelocity = playerRb.linearVelocity;

                // Teleport the player to the linked portal's position
                player.transform.position = linkedPortal.position;

                // Maintain the player's momentum
                playerRb.linearVelocity = currentVelocity;

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