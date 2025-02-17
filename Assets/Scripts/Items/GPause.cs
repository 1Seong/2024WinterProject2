using System.Collections;
using UnityEngine;

public class GPause : ItemBehavior
{
    CustomGravity gravity;
    void Start()
    {
        isConsumable = true;
    }

    new void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        Debug.Log("child triggered!");
        GPauseActivate(other.gameObject);
        base.OnTriggerEnter(other);
    }

    private void GPauseActivate(GameObject obj)
    {   

        gravity = obj.GetComponent<CustomGravity>();
        if (gravity != null)
        {
            Debug.Log("OK!");
            gravity.CallGPauseAction();
        }
    }
}
