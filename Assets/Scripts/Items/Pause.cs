using System.Collections;
using UnityEngine;
using static PlayerSelectableInterface;

public class Pause : Consumable
{
    [SerializeField] private float pauseTime = 5.0f;
    Movable player;

    [SerializeField] private Animator switchAnim;

    protected override void OnTriggerEnter(Collider other)
    {
        switchAnim.SetTrigger("Activate");

        var anims = other.GetComponentsInChildren<Animator>();
        foreach(var anim in anims )
        {
            anim.SetBool("Pause", true);
        }

        player = other.GetComponent<Movable>();
        PlayerTriggerEvent += _ => player.Pause(pauseTime);
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
