using System.Collections;
using UnityEngine;

public class GPause : ItemBehavior
{
    Player player;
    void Start()
    {
        isConsumable = false;
    }

    new void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        Debug.Log("child triggered!");
        player = other.GetComponent<Player>();
        player.CallGPauseAction();
        base.OnTriggerEnter(other);
    }


    /////////////////// NOT USED ////////////////////
    /*
    private void GPauseActivate(GameObject obj)
    {   

        gravity = obj.GetComponent<Player>();
        if (gravity != null)
        {
            Debug.Log("calling CustomGravity");
            gravity.CallGPauseAction();
        }
    }
    */
}
