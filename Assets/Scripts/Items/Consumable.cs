using UnityEngine;

public class Consumable : ItemBehavior
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (playerSelectable is PlayerSelectable && !PerformPlayerCheck(other, (int)color)) return;
        if (other.CompareTag("Player1") || other.CompareTag("Player2")) Consume();
    }

    public void Consume()
    {
        if (transform.parent != null)
            Destroy(transform.parent.gameObject);
        else
            Destroy(gameObject);
    }
}
