using System.Collections;
using UnityEngine;

public class Pause : ItemBehavior
{
    Movable player;
    
    void Start()
    {
        isConsumable = true;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        Debug.Log("child triggered!");
        player = other.GetComponent<Movable>();
        player.Pause(5.0f);
        base.OnTriggerEnter(other);
    }
    

    //////////////////// NOT USED //////////////////////////
   /*
    IEnumerator PauseRoutine(GameObject obj)
    {
        Rigidbody rigid = obj.GetComponent<Rigidbody>();
        //rigid.isKinematic = true;
        Debug.Log("stop");
        rigid.constraints = RigidbodyConstraints.FreezeAll;
        yield return new WaitForSeconds(5f);
        Debug.Log("go");
        rigid.constraints = RigidbodyConstraints.None;
        rigid.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
    }
   */
   

}
