using UnityEngine;
using DG.Tweening;
using static PlayerSelectableInterface;
using System.Collections;
using NUnit.Framework.Constraints;

public class Portal : ItemBehavior
{
    public Transform linkedPortal; // Reference to the linked portal
    private bool canTeleport = true;
    private float cooldownTime = 1f; // Cooldown to prevent repeated teleportation

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
                StartCoroutine(portalActivateCoroutine(other));
               
            }
        }
    }

    private IEnumerator portalActivateCoroutine(Collider other)
    {
        canTeleport = false;
        other.GetComponent<PlayerMove>().enabled = false;

        Movable player = other.GetComponent<Movable>();
        Rigidbody playerRb = player.GetComponent<Rigidbody>();

        var targetRotVec = GameManager.instance.isSideView ? new Vector3(0, 0f, 180f) : new Vector3(0, 180f, 0f);

        var inSeq = DOTween.Sequence();
        inSeq.Join(playerRb.transform.DOLocalMove(transform.position, 1.0f));
        inSeq.Join(playerRb.transform.DORotate(other.transform.rotation.eulerAngles + targetRotVec, 0.25f).SetLoops(4));
        inSeq.Join(playerRb.transform.DOScale(Vector3.zero, 1f));
        yield return inSeq.WaitForCompletion();
        playerRb.DOKill();
        yield return new WaitForFixedUpdate();

        var linkedPos = linkedPortal.position;

        // Teleport the player to the linked portal's position
        // Teleport when sideview
        if (GameManager.instance.isSideView)
        {
            // default
            if (playerRb.position.z == 0.5f)
            {
                playerRb.transform.position = new Vector3(linkedPos.x, linkedPos.y, 0.5f);
            }
            // inverted
            else
            {
                playerRb.transform.position = new Vector3(linkedPos.x, linkedPos.y, 7.5f);
            }

        }
        else // topview
        {
            playerRb.transform.position = new Vector3(linkedPos.x, 0f, linkedPos.z);
        }
        playerRb.transform.rotation = Quaternion.identity;
        playerRb.linearVelocity = Vector3.zero;
        
        var outSeq = DOTween.Sequence();
        outSeq.Join(playerRb.transform.DOScale(Vector3.one, 0.5f));
        yield return outSeq.WaitForCompletion();

        other.GetComponent<PlayerMove>().enabled = true;

        Invoke("ResetCooldown", cooldownTime);
    }

    private IEnumerator portalInTween(Collider other)
    {
        var seq = DOTween.Sequence();
        seq.Join(other.transform.DORotate(other.transform.rotation.eulerAngles + new Vector3(0, 180f, 0f), 0.25f).SetLoops(4));
        seq.Join(other.transform.DOScale(Vector3.zero, 1f));
        yield return seq.WaitForCompletion();
    }

    private IEnumerator portalOutTween(Collider other)
    {
        var seq = DOTween.Sequence();
        seq.Join(other.transform.DORotate(other.transform.rotation.eulerAngles + new Vector3(0, 180f, 0f), 0.25f).SetLoops(4));
        seq.Join(other.transform.DOScale(Vector3.one, 1f));
        yield return seq.WaitForCompletion();
    }

    private void ResetCooldown()
    {
        canTeleport = true;
    }

}