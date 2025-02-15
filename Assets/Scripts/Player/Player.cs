using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private Action updateAction;

    public event Action invertEvent;

    Rigidbody rigid;
    CustomGravity customGravity;

    private bool isPaused = false;
    private float pauseTimer = 0f;
    private const float PAUSE_DURATION = 5f;
    private Vector3 savedVelocity; //used for Pause

    public bool onBottom = true; // user can convert the view only when player is on bottom wall (or top, background depend on inverted and viewpoint)
    public bool hitInnerWall = false; // boolean for check horizontal collision with inner walls
    public bool onInnerWall = false; // boolean for check vertical collision with inner walls

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        customGravity = GetComponent<CustomGravity>();
    }

    private void Start()
    {
        updateAction += CheckInvert;
        updateAction += CheckInnerWallHoriz;
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
        
        float targetZ = 2.5f;

        if(customGravity.gravityState == GravityState.defaultG && rigid.position.z > targetZ || customGravity.gravityState == GravityState.invertG && rigid.position.z < targetZ)
        {
            invertEvent?.Invoke();
        }
        
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

    private void FixedUpdate()
    {
        if (!GameManager.instance.isPlaying)
            return;

        //check on inner wall
        CheckOnInnerWall();
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
        /*
         * Make onBottom to true if player make contact with bottom platform
         */
        switch (customGravity.gravityState)
        {
            case GravityState.defaultG:
                if (collision.gameObject.tag == "Bottom")
                    onBottom = true;
                break;
            case GravityState.invertG:
                if (collision.gameObject.tag == "Top")
                    onBottom = true;
                break;
            case GravityState.convertG:
                if (collision.gameObject.tag == "Background")
                    onBottom = true;
                break;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        /*
         * Make onBottom to false if player escape from bottom platform
         */
        switch (customGravity.gravityState)
        {
            case GravityState.defaultG:
                if (collision.gameObject.tag == "Bottom")
                    onBottom = false;
                break;
            case GravityState.invertG:
                if (collision.gameObject.tag == "Top")
                    onBottom = false;
                break;
            case GravityState.convertG:
                if (collision.gameObject.tag == "Background")
                    onBottom = false;
                break;
        }
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

        if (other.GetComponent<NonConsum>()?.type == NonConsum.Type.GPause)
        {
            customGravity.GPause();
        }
    }

}
