using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public AudioSource footstep;
    public AudioClip footstepClip;
    private Action fixedUpdateAction;

    Movable movable;
    Rigidbody rigid;

    private void Awake()
    {
        movable = GetComponent<Movable>();
        rigid = GetComponent<Rigidbody>();
        footstep = GetComponent<AudioSource>();
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
        if (Input.GetAxisRaw("Horizontal")==0f)
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
            if (h != 0f)
            {
                rigid.constraints &= ~RigidbodyConstraints.FreezePositionX;
                AudioManager.instance.PlayFootstep();
            }
            else
                AudioManager.instance.StopFootstep();
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
