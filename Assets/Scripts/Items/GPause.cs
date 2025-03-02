using System.Collections;
using UnityEngine;
using static PlayerSelectableInterface;

public class GPause : ItemBehavior
{
    Movable player;

    protected override void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.gpauseActive) return;
        
        player = other.GetComponent<Movable>();
        PlayerTriggerEvent += _ => player.CallGPauseAction();
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
