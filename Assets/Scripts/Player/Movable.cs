using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Movable : MonoBehaviour
{
    [SerializeField] private float ICE_ACCELATION = 2.0f;

    protected Action updateAction;

    public event Action invertEvent;

    protected Rigidbody rigid;
    protected CustomGravity customGravity;
    protected const float TARGET_Z = 4.0f;

    private bool isPaused = false;
    private float pauseTimer = 0f;
    private Vector3 savedVelocity; //used for Pause

    public bool hitInnerWall = false; // boolean for check horizontal collision with inner walls
    public bool onInnerWall = false; // boolean for check vertical collision with inner walls

    public bool onIce = false;

    [SerializeField] private float _horizCollDis = 0.063f;
    [SerializeField] private float _vertCollDis = 0.005f;

    private List<Movable> movables;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        customGravity = GetComponent<CustomGravity>();
    }

    virtual protected void Start()
    {
        updateAction += CheckInvert;
        updateAction += IceAction;
        Stage.convertEventLast += CheckConvertCollision;

        movables = GetComponent<CustomGravity>().gravityState == GravityState.defaultG ? StageManager.instance.stage.defaultMovables : StageManager.instance.stage.invertMovables;

        movables.Add(this);

        if(GameManager.instance.dynamicInnerWallInstantiation)
        {
            updateAction += CheckInnerWallVert;
            updateAction += CheckInnerWallHoriz;
        }
    }

    private void OnDestroy()
    {
        Stage.convertEventLast -= CheckConvertCollision;
        movables.Remove(this);
        StopCoroutine(GPauseAction());
    }

    private void Update()
    {
        if (!GameManager.instance.isPlaying)
            return;

        
        //Handle pause effect
        if (isPaused)
        {
            HandlePauseEffect();
            return;  // Skip other updates while paused
        }
        
        updateAction?.Invoke();
    }

    private void CheckInvert()
    {
        /*
         * Check inversion condition
         */
        if (GameManager.instance.isSideView)
            return;
        
        if(customGravity.gravityState == GravityState.defaultG && rigid.position.z > TARGET_Z || customGravity.gravityState == GravityState.invertG && rigid.position.z < TARGET_Z)
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
        Vector3 box = new Vector3(0.44f, 0, 0.44f);

        if (GameManager.instance.isSideView)
            box = new Vector3(0.44f, 0.44f, 0);

        //Debug.DrawRay(rigid.position, targetVec, Color.yellow);

        RaycastHit[] rayHit = Physics.BoxCastAll(rigid.position + 0.1f * targetVec, box, targetVec, Quaternion.identity, 0.5f, LayerMask.GetMask("Platform"));

        if (rayHit.Length != 0)
        {
            foreach(var hit in rayHit)
            {
                
                if(hit.collider.CompareTag("Inner") && hit.distance < _vertCollDis)
                {
                    onInnerWall = true;
                    return;
                }
            }
        }
        onInnerWall = false;
    }

    // TODO: refactor wall detection mech -> use observer pattern and notify to PlayerJump and CustomGravity
    private void CheckInnerWallHoriz()
    {
        /*
         * Custom horizontal collision with inner walls
         */
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            hitInnerWall = false;
            return;
        }

        Vector3 box = !GameManager.instance.isSideView ? new Vector3(0.44f, 0.1f, 0.44f) : new Vector3(0.44f, 0.44f, 0.1f);
        Vector3 targetVec = Input.GetAxisRaw("Horizontal") > 0 ? Vector3.right : Vector3.left;

        Debug.DrawRay(rigid.position, targetVec, Color.yellow);

        // Use box cast to check inner walls
        RaycastHit[] rayHit = Physics.BoxCastAll(rigid.position, box, targetVec, Quaternion.identity, 0.5f, LayerMask.GetMask("Platform"));

        if (rayHit.Length != 0)
        {
            foreach (var hit in rayHit)
            {

                if (hit.collider.CompareTag("Inner") && hit.distance < _horizCollDis)
                {
                    hitInnerWall = true;
                    return;
                }
            }
        }
        hitInnerWall = false;
    }
    
    // TODO : refactor - Ice
    protected virtual void IceAction()
    {
        Vector3 targetVec = customGravity.down;
        Vector3 box = new Vector3(0.49f, 0, 0.5f);
        bool iceExist;

        if (GameManager.instance.isSideView)
            box = new Vector3(0.49f, 0.5f, 0);

        //Debug.DrawRay(rigid.position, targetVec, Color.yellow);

        RaycastHit[] rayHit = Physics.BoxCastAll(rigid.position, box, targetVec, Quaternion.identity, 0.5f, LayerMask.GetMask("Platform"));

        iceExist = ObjectExistInRaycast(rayHit, "Ice");

        if(!onIce && iceExist && rigid.linearVelocity.x != 0) // onIce : false -> true
        {
            onIce = true;
            
            GetComponent<BoxCollider>().material.dynamicFriction = 0;
        }
        else if(onIce && ObjectExistInRaycast(rayHit) && !iceExist) // onIce : true -> false
        {
            onIce = false;
            rigid.linearVelocity = Vector3.zero;
            
            GetComponent<BoxCollider>().material.dynamicFriction = 2;
        }
    }

    // TODO : refactor - make a raycast utility class to capsulate raycast behaviors
    protected bool ObjectExistInRaycast(RaycastHit[] rayHit, string tag)
    {
        if (rayHit.Length == 0) return false;

        foreach (var i in rayHit)
            if (i.distance < 0.1f && i.collider.tag == tag)
                return true;

        return false;
    }

    protected bool ObjectExistInRaycast(RaycastHit[] rayHit)
    {
        if (rayHit.Length == 0) return false;

        foreach (var i in rayHit)
            if (i.distance < 0.1f && tag != i.collider.tag)
                return true;

        return false;
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isPlaying)
            return;

        //check on inner wall
        if(GameManager.instance.dynamicInnerWallInstantiation)
            CheckOnInnerWall();

        
        if (onIce)
        {
            MoveOnIce();
        }
        
    }

    private void MoveOnIce()
    {
        int dir = rigid.linearVelocity.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector3(ICE_ACCELATION * dir, 0, 0), ForceMode.Acceleration);
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
            rigid.constraints = RigidbodyConstraints.FreezeAll;
            // Update pause timer
            pauseTimer -= Time.deltaTime;

            // Check for space key or timer completion
            if (Input.GetKeyDown(KeyCode.Q) || pauseTimer <= 0)
            {
                isPaused = false;
                rigid.constraints = RigidbodyConstraints.None;
                rigid.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
                rigid.linearVelocity = savedVelocity;
                pauseTimer = 0f;
            }
        }
    }

    private void CheckConvertCollision()
    {
        bool collide = false;
        Vector3 box = new Vector3(0.49f, 0, 0.5f);

        if (GameManager.instance.isSideView)
            box = new Vector3(0.49f, 0.5f, 0);

        do
        {
            RaycastHit[] rayHit = Physics.BoxCastAll(rigid.position, box, Vector3.zero, Quaternion.identity, 0, LayerMask.GetMask("Platform"));
            collide = ObjectExistInRaycast(rayHit);

            if(collide)
                transform.position += GetComponent<CustomGravity>().up;
        }
        while (collide);
    }

    // pause the player for given duration
    public void Pause(float duration)
    {
        isPaused = true;
        pauseTimer = duration;
        savedVelocity = rigid.linearVelocity;
        rigid.linearVelocity = Vector3.zero;
    }

    public void CallGPauseAction()
    {
        StartCoroutine(GPauseAction());
    }

    IEnumerator GPauseAction()
    {
        GameManager.instance.gpauseActive = true;

        updateAction -= CheckInvert;
        
        invertEvent!.Invoke();
        Debug.Log("1clear");

        yield return new WaitForSeconds(10f);
        Debug.Log("2clear");
        
        invertEvent!.Invoke();
        yield return new WaitForSeconds(2f);
        updateAction += CheckInvert;
        Debug.Log("3clear");

        GameManager.instance.gpauseActive = false;
    }

    // CHANGED: TriggerEnter is activated in the item's script
    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<NonConsum>()?.type == NonConsum.Type.Pause && !isPaused)
        {
            isPaused = true;
            pauseTimer = PAUSE_DURATION;
            savedVelocity = rigid.linearVelocity;
            rigid.linearVelocity = Vector3.zero;
        }

        if (other.GetComponent<NonConsum>()?.type == NonConsum.Type.GPause)
        {
            customGravity.GPause();
        }
    }
    */
}
