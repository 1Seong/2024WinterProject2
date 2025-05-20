using System;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerJump : MonoBehaviour
{
    public int jumpUnit; // Jump unit - ex) when unit is 'n' -> you can jump over a wall with 'n' height

    private Action updateAction;
    private Action fixedUpdateAction;

    Rigidbody rigid;
    CustomGravity customGravity;

    private bool requestJump = false; // true when user enters jump button - request jump to fixedUpdate
    public bool isJumping = false; // cannot perform another jump when isJumping is true

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        customGravity = GetComponent<CustomGravity>();
    }
    
    void Start()
    {
        updateAction += () =>
        {
            if (Vector3.Dot(rigid.linearVelocity, customGravity.up) <= 0)
                Landing();
        };
        updateAction += RequestJump;
        fixedUpdateAction += () =>
        {
            if (requestJump)
            {
                PerformJump();
                requestJump = false;
            }
        };
    }

    void Update()
    {
        if (!GameManager.instance.isPlaying)
            return;

        // Jump
        updateAction?.Invoke();
    }

    private void RequestJump()
    {
        /*
         * Request jump to fixed update
         */
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            requestJump = true;
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isPlaying)
            return;

        fixedUpdateAction?.Invoke();
    }

    private void Landing()
    {
        /*
         * Check collision with any platform and make isJumping to false
         */
        isJumping = true;

        Vector3 targetVec = customGravity.down;
        Vector3 box = new Vector3(0.47f, 0, 0.5f);

        if (GameManager.instance.isSideView)
            box = new Vector3(0.49f, 0.5f, 0);

        //Debug.DrawRay(rigid.position, targetVec, Color.yellow);

        RaycastHit[] rayHit = Physics.BoxCastAll(rigid.position, box, targetVec, Quaternion.identity, 0.5f, LayerMask.GetMask("Platform"));

        if (rayHit.Length != 0)
            foreach(var i in rayHit)
            {
                if (i.distance < 0.07f && tag != i.collider.tag)
                {
                    isJumping = false;
                    break;
                }
            }
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

        rigid.AddForce(customGravity.up * force, ForceMode.Impulse);
    }
}
