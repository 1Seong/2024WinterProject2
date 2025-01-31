using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Movable : MonoBehaviour
{
    private const float ICE_ACCELATION = 2f;

    private Action updateAction;

    public event Action invertEvent;

    Rigidbody rigid;
    CustomGravity customGravity;

    private bool isPaused = false;
    private float pauseTimer = 0f;
    private const float PAUSE_DURATION = 5f;
    private Vector3 savedVelocity; //used for Pause

    public bool hitInnerWall = false; // boolean for check horizontal collision with inner walls
    public bool onInnerWall = false; // boolean for check vertical collision with inner walls

    private bool onIce = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        customGravity = GetComponent<CustomGravity>();
    }

    private void Start()
    {
        updateAction += CheckInvert;
        updateAction += CheckInnerWallVert;
        updateAction += CheckInnerWallHoriz;
        updateAction += IceAction;
    }

    private void Update()
    {
        if (!GameManager.instance.isPlaying)
            return;

        /*
        //Handle pause effect
        if (isPaused)
        {
            HandlePauseEffect();
            return;  // Skip other updates while paused
        }
        */
        updateAction?.Invoke();
    }

    private void CheckInvert()
    {
        /*
         * Check inversion condition
         */
        if (GameManager.instance.isSideView)
            return;
        
        float targetZ = 4f;

        if(customGravity.gravityState == GravityState.defaultG && rigid.position.z > targetZ || customGravity.gravityState == GravityState.invertG && rigid.position.z < targetZ)
        {
            invertEvent?.Invoke();
        }
        
    }

    private void CheckInnerWallVert()
    {
        /*
         * Check collision with any platform and make isJumping to false
         */
        if (Vector3.Dot(rigid.linearVelocity, customGravity.up) > 0)
        {
            onInnerWall = false;
            return;
        }

        Vector3 targetVec = customGravity.down;
        Vector3 box = new Vector3(0.49f, 0, 0.5f);

        if (GameManager.instance.isSideView)
            box = new Vector3(0.49f, 0.5f, 0);

        //Debug.DrawRay(rigid.position, targetVec, Color.yellow);

        RaycastHit[] rayHit = Physics.BoxCastAll(rigid.position, box, targetVec, Quaternion.identity, 0.5f, LayerMask.GetMask("Platform"));

        if (rayHit.Length != 0 && rayHit[0].distance < 0.07f && rayHit[0].transform.tag == "Inner")
            onInnerWall = true;
        else
            onInnerWall = false;
        
    }

    private void CheckInnerWallHoriz()
    {
        /*
         * Custom horizontal collision with inner walls
         */
        Vector3 box = !GameManager.instance.isSideView ? new Vector3(0.5f, 0.1f, 0.49f) : new Vector3(0.5f, 0.49f, 0.1f);
        Vector3 targetVec = Input.GetAxisRaw("Horizontal") > 0 ? Vector3.right : Vector3.left;

        // Use box cast to check inner walls
        RaycastHit[] rayHit = Physics.BoxCastAll(rigid.position, box, targetVec, Quaternion.identity, 0.5f, LayerMask.GetMask("Platform"));
   
        if (rayHit.Length != 0 && rayHit[0].transform.tag == "Inner" && rayHit[0].distance < 0.06f)
            hitInnerWall = true;
        else
            hitInnerWall = false;
    }

    private void IceAction()
    {
        Vector3 targetVec = customGravity.down;
        Vector3 box = new Vector3(0.49f, 0, 0.5f);
        bool iceExist;

        Func<RaycastHit[], bool> IceExist = (RaycastHit[] rayHit) =>
        {
            if (rayHit.Length == 0) return false;

            foreach (var i in rayHit)
                if (i.distance < 0.1f && i.transform.tag == "Ice")
                    return true;

            return false;
        };

        Func<RaycastHit[], bool> PlatformExist = (RaycastHit[] rayHit) =>
        {
            if (rayHit.Length == 0) return false;

            foreach (var i in rayHit)
                if (i.distance < 0.1f)
                    return true;

            return false;
        };

        if (GameManager.instance.isSideView)
            box = new Vector3(0.49f, 0.5f, 0);

        //Debug.DrawRay(rigid.position, targetVec, Color.yellow);

        RaycastHit[] rayHit = Physics.BoxCastAll(rigid.position, box, targetVec, Quaternion.identity, 0.5f, LayerMask.GetMask("Platform"));

        iceExist = IceExist(rayHit);

        if(!onIce && iceExist && rigid.linearVelocity.x != 0) // onIce : false -> true
        {
            onIce = true;
            if (tag == "Player")
                GetComponent<PlayerMove>().enabled = false;
            else
                GetComponent<BoxCollider>().material.dynamicFriction = 0;
        }
        else if(onIce && PlatformExist(rayHit) && !iceExist) // onIce : true -> false
        {
            onIce = false;
            rigid.linearVelocity = Vector3.zero;
            if (tag == "Player")
            {
                GetComponent<PlayerMove>().enabled = true;
            }
            else
                GetComponent<BoxCollider>().material.dynamicFriction = 2;
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isPlaying)
            return;

        //check on inner wall
        CheckOnInnerWall();

        if (onIce)
        {
            int dir = rigid.linearVelocity.x > 0 ? 1 : -1;
            rigid.AddForce(new Vector3(ICE_ACCELATION * dir, 0, 0), ForceMode.Acceleration);
        }
    }

    private void CheckOnInnerWall()
    {
        /*
         * Check if player is on inner walls
         * Restrict vertical velocity
         */
        if (onInnerWall)
        {
            if (!GameManager.instance.isSideView)
                rigid.linearVelocity = new Vector3(rigid.linearVelocity.x, rigid.linearVelocity.y, 0);
            else
                rigid.linearVelocity = new Vector3(rigid.linearVelocity.x, 0, rigid.linearVelocity.z);
        }
    }

    private void HandlePauseEffect()
    {
        if (isPaused)
        {
            // Keep the player stopped
            rigid.linearVelocity = Vector3.zero;

            // Update pause timer
            pauseTimer -= Time.deltaTime;

            // Check for space key or timer completion
            if (Input.GetKeyDown(KeyCode.Space) || pauseTimer <= 0)
            {
                isPaused = false;
                rigid.linearVelocity = savedVelocity;
                pauseTimer = 0f;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
       
    }

    private void OnCollisionExit(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<NonConsum>()?.type == NonConsum.Type.Pause && !isPaused)
        {
            isPaused = true;
            pauseTimer = PAUSE_DURATION;
            savedVelocity = rigid.linearVelocity;
            rigid.linearVelocity = Vector3.zero;
        }
    }

}
