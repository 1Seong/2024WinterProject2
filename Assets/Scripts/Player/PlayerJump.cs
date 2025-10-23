using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

public class PlayerJump : MonoBehaviour
{
    public int jumpUnit; // Jump unit - ex) when unit is 'n' -> you can jump over a wall with 'n' height

    Rigidbody rigid;
    CustomGravity customGravity;

    private bool requestJump = false; // true when user enters jump button - request jump to fixedUpdate
    public bool isJumping = false; // cannot perform another jump when isJumping is true

    [SerializeField] private float iceJumpMultiplier = 1.2f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        customGravity = GetComponent<CustomGravity>();
    }

    void Update()
    {
        if (!GameManager.instance.isPlaying)
            return;

        // Jump
        InvokeLanding();
        RequestJump();
    }

    private void InvokeLanding()
    {
        if (Vector3.Dot(rigid.linearVelocity, customGravity.up) < 0 || IsPlayerOnInnerWall())
            Landing();
    }

    private void RequestJump()
    {
        /*
         * Request jump to fixed update
         */
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !isJumping)
        {
            requestJump = true;
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isPlaying)
            return;

        InvokePerformJump();
    }
    
    private void InvokePerformJump()
    {
        if (requestJump)
        {
            PerformJump();
            requestJump = false;
        }
    }

    private void Landing()
    {
        /*
         * Check collision with any platform and make isJumping to false
         */
        //isJumping = true;

        /*
        if (IsPlayerOnInnerWall())
        {
            isJumping = false;
            return;
        }
        */

        Vector3 targetVec = customGravity.down;
        Vector3 box = new Vector3(0.42f, 0, 0.5f);

        if (GameManager.instance.isSideView)
            box = new Vector3(0.42f, 0.5f, 0);

        //Debug.DrawRay(rigid.position, targetVec, Color.blue);

        RaycastHit[] rayHit = Physics.BoxCastAll(rigid.position, box, targetVec, Quaternion.identity, 0.5f, LayerMask.GetMask("Platform"));

        if (rayHit.Length != 0)
            foreach(var i in rayHit)
            {
                if (i.transform.CompareTag("Spring") || i.transform.CompareTag("Player1") || i.transform.CompareTag("Player2"))
                    continue;
                if (i.distance < 0.07f && !i.collider.CompareTag(tag))
                {
                    isJumping = false;
                    
                    var anims = GetComponentsInChildren<Animator>();
                    if (anims[0].GetBool("JumpPad"))
                        foreach (var anim in anims)
                            anim.SetBool("JumpPad", false);

                    return;
                }
            }

        isJumping = true;
    }

    private void PerformJump()
    {
        /*
         * Perform jumping
         */
        isJumping = true;

        float gravity = Physics.gravity.magnitude;

        float initialVelocity = Mathf.Sqrt(2 * gravity * jumpUnit);
        float force = rigid.mass * initialVelocity + 0.5f;

        if (IsOnIce() && !GetComponent<Player>().frog)
        {
            //Debug.Log("Ice Jump Multiplier: " + iceJumpMultiplier);
            force *= iceJumpMultiplier;
        }

        rigid.AddForce(customGravity.up * force, ForceMode.Impulse);
    }

    private bool IsOnIce()
    {
        bool res = GetComponent<Movable>().onIce;
        return res;
    }

    private bool IsPlayerOnInnerWall()
    {
        bool res = GetComponent<Movable>().onInnerWall;
        return res;
    }
}
