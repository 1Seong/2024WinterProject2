using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public int jumpUnit; // Jump unit - ex) when unit is 'n' -> you can jump over a wall with 'n' height
    public bool inverted;

    Rigidbody rigid;
    BoxCollider coll;
    CustomGravity customGravity;

    private bool isPaused = false;
    private float pauseTimer = 0f;
    private const float PAUSE_DURATION = 5f;
    private Vector3 savedVelocity; //used for Pause

    private bool requestJump = false; // true when user enters jump button - request jump to fixedUpdate
    public bool isJumping = false; // cannot perform another jump when isJumping is true
    public bool onBottom = true; // user can convert the view only when player is on bottom wall (or top, background depend on inverted and viewpoint)
    private bool hitInnerWall = false; // boolean for check horizontal collision with inner walls
    public bool onInnerWall = false; // boolean for check vertical collision with inner walls
    

    private void Awake()
    {
        /*
         * Awake
         */
        rigid = GetComponent<Rigidbody>();
        coll = GetComponent<BoxCollider>();
        customGravity = GetComponent<CustomGravity>();
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        
    }

    private void Update()
    {
        /*
         * Update
         */
        if (!GameManager.instance.isPlaying)
            return;

        //Handle pause effect
        if (isPaused)
        {
            HandlePauseEffect();
            return;  // Skip other updates while paused
        }

        // Jump
        RequestJump();

        // Check Invert condition
        CheckInvert();

        //Stop speed when button is up
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.linearVelocity = new Vector3(rigid.linearVelocity.normalized.x * 0.0f, rigid.linearVelocity.y, rigid.linearVelocity.z);
        }

        //Check Inner Wall Horizontally
        CheckInnerWallHoriz();

        //Check Inner Wall Vertically
        CheckInnerWallVert();
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

    private void CheckInvert()
    {
        /*
         * Check inversion condition
         */
        if (!GameManager.instance.isTopView)
            return;
        
        /*
        float targetZ = -1.0f + GameManager.instance.currentStage.invertLineZ;

        if(!inverted && rigid.position.z > targetZ)
        {
            inverted = true;
            customGravity.InvertGravity(); // Invert gravity
        }
        else if(inverted &&  rigid.position.z < targetZ)
        {
            inverted = false;
            customGravity.InvertGravity();
        }
        */
    }

    public void ConversionPhysicsSetting()
    {
        /*
         * Physics Setting for conversion
         */
        float gravity = Physics.gravity.magnitude;

        if(GameManager.instance.isTopView)
        {
            Physics.gravity = new Vector3(0, -gravity, 0);
            customGravity.ReapplyGravity();
            rigid.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            Physics.gravity = new Vector3(0, 0, -gravity);

            if (inverted)
            {
                customGravity.InvertGravity();
            }
            else
                customGravity.ReapplyGravity();

            rigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        }
    }

    private void CheckInnerWallHoriz()
    {
        /*
         * Custom horizontal collision with inner walls
         */
        Vector3 box = GameManager.instance.isTopView ? new Vector3(0.5f, 0.1f, 0.49f) : new Vector3(0.5f, 0.49f, 0.1f);
        Vector3 targetVec = Input.GetAxisRaw("Horizontal") > 0 ? Vector3.right : Vector3.left;

        // Use box cast to check inner walls
        RaycastHit[] rayHit = Physics.BoxCastAll(rigid.position, box, targetVec, Quaternion.identity, 0.5f, LayerMask.GetMask("Platform"));
   
        if (rayHit.Length != 0 && rayHit[0].transform.tag == "Inner" && rayHit[0].distance < 0.08f)
        {
            hitInnerWall = true;
        }
        else
        {
            hitInnerWall = false;
        }
    }

    private void CheckInnerWallVert()
    {
        /*
         * Custom vertical collision with inner walls
         */

        // Do not check collision when player is moving up
        if (GameManager.instance.isTopView)
        {
            if (!inverted && rigid.linearVelocity.z > 0.0f || inverted && rigid.linearVelocity.z < 0.0f)
            {
                onInnerWall = false;
                return;
            }
        }
        else
        {
            if (rigid.linearVelocity.y > 0.0f)
            {
                onInnerWall = false;
                return;
            }
        }

        Vector3 targetVec = inverted ? Vector3.forward : Vector3.back;
        Vector3 box = new Vector3(0.49f, 0.1f, 0.5f);

        if (!GameManager.instance.isTopView)
        {
            targetVec = Vector3.down;
            box = new Vector3(0.49f, 0.5f, 0.1f);
        }

        RaycastHit[] rayHit = Physics.BoxCastAll(rigid.position, box, targetVec, Quaternion.identity, 0.5f, LayerMask.GetMask("Platform"));

        if (rayHit.Length != 0 && rayHit[0].transform.tag == "Inner" && rayHit[0].distance < 0.07f)
        {
            onInnerWall = true;
        }
        else
        {
            onInnerWall = false;
        }
    }

    private void FixedUpdate()
    {
        /*
         * Fixed update
         */
        if (!GameManager.instance.isPlaying || isPaused)
            return;

        //Player Moving
        PlayerMoving();

        //restrict max min velocity
        RestrictSpeed();
           
        //Landing Platform
        CheckLanding();

        //check on inner wall
        CheckOnInnerWall();

        //Jumping
        if (requestJump)
        {
            PerformJump();
            requestJump = false;
        }
    }

    private void PlayerMoving()
    {
        /*
         * Move player horizontally
         */
        float h = Input.GetAxisRaw("Horizontal");

        Vector3 dirVec = Vector3.right * GameManager.instance.speed * h;

        if (!hitInnerWall)
            rigid.AddForce(dirVec, ForceMode.Impulse);
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

    private void CheckLanding()
    {
        /*
         * Check landing condition
         */
        if (GameManager.instance.isTopView)
        {
            if (!inverted && rigid.linearVelocity.z < 0.0f || inverted && rigid.linearVelocity.z > 0.0f)
            {
                Landing();
            }
        }
        else
        {
            if (rigid.linearVelocity.y < 0.0f)
            {
                Landing();
            }
        }
    }

    private void Landing()
    {
        /*
         * Check collision with any platform and make isJumping to false
         */
        isJumping = true;

        Vector3 targetVec = inverted ? Vector3.forward : Vector3.back;
        Vector3 box = new Vector3(0.49f, 0, 0.5f);

        if (!GameManager.instance.isTopView)
        {
            targetVec = Vector3.down;
            box = new Vector3(0.49f, 0.5f, 0);
        }

        //Debug.DrawRay(rigid.position, targetVec, Color.yellow);

        RaycastHit[] rayHit = Physics.BoxCastAll(rigid.position, box, targetVec, Quaternion.identity, 0.5f, LayerMask.GetMask("Platform"));

        if (rayHit.Length != 0)
        {

            if (rayHit[0].distance < 0.1f)
            {
               
                isJumping = false;
            }
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
            if (GameManager.instance.isTopView)
                rigid.linearVelocity = new Vector3(rigid.linearVelocity.x, rigid.linearVelocity.y, 0);
            else
                rigid.linearVelocity = new Vector3(rigid.linearVelocity.x, 0, rigid.linearVelocity.z);
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

        if (GameManager.instance.isTopView)
        {
            if(inverted)
                force = -force;

            rigid.AddForce(Vector3.forward * force, ForceMode.Impulse);
        }
        else
        {
            rigid.AddForce(Vector3.up * force, ForceMode.Impulse);
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
        if(GameManager.instance.isTopView)
        {
            if (!inverted && collision.gameObject.tag != "Bottom")
                return;
            else if (inverted && collision.gameObject.tag != "Top")
                return;
        }
        else
        {
            if (collision.gameObject.tag != "Background")
                return;
        }

        onBottom = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        /*
         * Make onBottom to false if player escape from bottom platform
         */
        if (GameManager.instance.isTopView)
        {
            if (!inverted && collision.gameObject.tag != "Bottom")
                return;
            else if (inverted && collision.gameObject.tag != "Top")
                return;
        }
        else
        {
            if (collision.gameObject.tag != "Background")
                return;
        }

        onBottom = false;
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
