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
                var linkedPos = linkedPortal.position;

                // Teleport the player to the linked portal's position
                // Teleport when sideview
                if(GameManager.instance.isSideView)
                {
                    // default
                    if (playerRb.position.z == 0.5f)
                    {
                        player.transform.position = new Vector3(linkedPos.x, linkedPos.y, 0.5f);
                    }
                    // inverted
                    else
                    {
                        player.transform.position = new Vector3(linkedPos.x, linkedPos.y, 7.5f);
                    }

                }
                else // topview
                {
                    player.transform.position = new Vector3(linkedPos.x, 0f, linkedPos.z);
                }

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