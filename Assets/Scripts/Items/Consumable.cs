using UnityEngine;

public class Consumable : ItemBehavior
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if(other.tag == "Player1" || other.tag == "Player2") Consume();
    }

    public void Consume()
    {
        if (transform.parent != null)
            Destroy(transform.parent.gameObject);
        else
            Destroy(gameObject);
    }
}
