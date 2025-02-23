using System;
using UnityEngine;

public abstract class ItemBehavior : MonoBehaviour
{
    protected event Action<Collision> PlayerCollisonEvent;
    protected event Action<Collider> PlayerTriggerEvent;

    protected void OnCollisionEnter(Collision collision)
    {
        Debug.Log("parent collision!");
        if (collision.gameObject.tag == "Player1" || collision.gameObject.tag == "Player2") 
        {
            PlayerCollisonEvent?.Invoke(collision);
        }
    }

    virtual protected void OnTriggerEnter(Collider other)
    {
        Debug.Log("parent trigger!");
        if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2")
        { 
            PlayerTriggerEvent?.Invoke(other);
        }
    }
}
