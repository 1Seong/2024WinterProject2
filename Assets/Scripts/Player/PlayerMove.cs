using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Action fixedUpdateAction;

    Movable movable;
    Rigidbody rigid;

    private void Awake()
    {
        movable = GetComponent<Movable>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        fixedUpdateAction += PlayerMoving;
        fixedUpdateAction += RestrictSpeed;
    }

    void Update()
    {
        if (!GameManager.instance.isPlaying)
            return;

        //Stop speed when button is up
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.constraints |= RigidbodyConstraints.FreezePositionX;
            rigid.linearVelocity = new Vector3(0f, rigid.linearVelocity.y, rigid.linearVelocity.z);
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isPlaying)
            return;

        fixedUpdateAction?.Invoke();
    }

    private void PlayerMoving()
    {
        /*
         * Move player horizontally
         */
        float h = Input.GetAxisRaw("Horizontal");
        
        Vector3 dirVec = Vector3.right * GameManager.instance.speed * h;

        if (!movable.hitInnerWall)
        {
            if(h != 0f)
                rigid.constraints &= ~RigidbodyConstraints.FreezePositionX;
            rigid.AddForce(dirVec, ForceMode.Impulse);
        }
        else
            rigid.linearVelocity = new Vector3(0, rigid.linearVelocity.y, rigid.linearVelocity.z);
    }

    private void RestrictSpeed()
    {
        /*
         * Restrict min max speed
         */
        float max = GameManager.instance.maxSpeed;

        if (rigid.linearVelocity.x > max)
            rigid.linearVelocity = new Vector3(max, rigid.linearVelocity.y, rigid.linearVelocity.z);
        else if (rigid.linearVelocity.x < max * (-1))
            rigid.linearVelocity = new Vector3(max * (-1), rigid.linearVelocity.y, rigid.linearVelocity.z);
    }
}
