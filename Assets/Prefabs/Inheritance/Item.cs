using System;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    protected event Action ItemActivateEvent;
    protected event Action PlayerCollisonEvent;
    protected event Action PlayerTriggerEvent;
    public bool isConsumable;

    private void Awake()
    {
        PlayerCollisonEvent += Consume;
        PlayerTriggerEvent += Consume;
    }

    protected void OnCollisionEnter(Collision collision)
    {
        Debug.Log("parent collision!");
        if (collision.gameObject.tag == "Player") 
        {
            PlayerCollisonEvent!.Invoke();
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        Debug.Log("parent trigger!");
        if (other.gameObject.tag == "Player")
        { 
            PlayerTriggerEvent!.Invoke();
        }
    }

    public void Consume()
    {
        if(isConsumable) 
        {
            if(transform.parent != null)
                Destroy(transform.parent.gameObject);
            else
                Destroy(gameObject);
        }
    }
}
