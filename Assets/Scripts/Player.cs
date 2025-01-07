using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public int jumpUnit;
    public bool inverted;

    Rigidbody rigid;
    BoxCollider coll;
    CustomGravity customGravity;

    private bool requestJump = false;
    private bool isJumping = false;
    private bool onBottom = true;
    private bool hitInnerWall = false;
    public bool onInnerWall = false;

    private Transform stage;
    

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        coll = GetComponent<BoxCollider>();
        customGravity = GetComponent<CustomGravity>();
    }

    private void Start()
    {
        stage = GameManager.instance.currentStage.transform;
    }

    private void OnEnable()
    {
        stage = GameManager.instance.currentStage.transform;
    }

    private void Update()
    {
        if (!GameManager.instance.isPlaying)
            return;

        //Jump
        if(Input.GetButtonDown("Jump") && !isJumping)
        {
            requestJump = true;
        }

        //Check Invert condition
        if(GameManager.instance.isTopView)
            CheckInvert();

        if(Input.GetKeyDown(KeyCode.E) && !isJumping && onBottom)
        {
            ConvertView();
        }

        //Stop Speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.linearVelocity = new Vector3(rigid.linearVelocity.normalized.x * 0.0f, rigid.linearVelocity.y, rigid.linearVelocity.z);
        }

        //Check Inner Wall Horizontally
        CheckInnerWallHoriz();

        //Check Inner Wall Vertically
        CheckInnerWallVert();
    }

    private void CheckInnerWallHoriz()
    {
        Vector3 box = GameManager.instance.isTopView ? new Vector3(0.5f, 0.1f, 0.49f) : new Vector3(0.5f, 0.49f, 0.1f);
        Vector3 targetVec = Input.GetAxisRaw("Horizontal") > 0 ? Vector3.right : Vector3.left;

        RaycastHit[] rayHit = Physics.BoxCastAll(rigid.position, box, targetVec, Quaternion.identity, 0.5f, LayerMask.GetMask("Platform"));
   
        if (rayHit.Length == 0 || rayHit[0].transform.tag != "Inner")
        {
            hitInnerWall = false;
            return;
        }

        if (rayHit[0].distance < 0.08f)
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

        if (rayHit.Length == 0 || rayHit[0].transform.tag != "Inner")
        {
            onInnerWall = false;
            return;
        }

        if (rayHit[0].distance < 0.07f)
        {
            onInnerWall = true;
        }
        else
        {
            onInnerWall = false;
        }
    }

    private void CheckInvert()
    {
        float targetZ = -1.0f + GameManager.instance.currentStage.invertLineZ;

        if(!inverted && rigid.position.z > targetZ)
        {
            inverted = true;
            customGravity.InvertGravity();
        }
        else if(inverted &&  rigid.position.z < targetZ)
        {
            inverted = false;
            customGravity.InvertGravity();
        }
    }

    private void ConvertView()
    {
        bool topview = GameManager.instance.isTopView;

        //Camera Setting

        GameManager.instance.currentStage.CallCameraRotate();
        

        //Physics Setting
        float gravity = Physics.gravity.magnitude;

        if(topview)
        {
            Physics.gravity = new Vector3(0, -gravity, 0);
            customGravity.ReapplyGravity();
            rigid.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

            if (inverted)
                rigid.position = new Vector3(rigid.position.x, rigid.position.y, 0);
        }
        else
        {
            Physics.gravity = new Vector3(0, 0, -gravity);

            if (inverted)
                customGravity.InvertGravity();
            else
                customGravity.ReapplyGravity();

            rigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        }
        GameManager.instance.isTopView = !topview;
    }

    private void PerformJump()
    {
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


    private void FixedUpdate()
    {
        if (!GameManager.instance.isPlaying)
            return;

        float max = GameManager.instance.maxSpeed;

        //Player Moving
        float h = Input.GetAxisRaw("Horizontal");

        Vector3 dirVec = Vector3.right * GameManager.instance.speed * h;

        if (!hitInnerWall)
            rigid.AddForce(dirVec, ForceMode.Impulse);
        else
            rigid.linearVelocity = new Vector3(0, rigid.linearVelocity.y, rigid.linearVelocity.z);

        //restrict max min velocity
        if (rigid.linearVelocity.x > max)
            rigid.linearVelocity = new Vector3(max, rigid.linearVelocity.y, rigid.linearVelocity.z);
        else if (rigid.linearVelocity.x < max * (-1))
            rigid.linearVelocity = new Vector3(max * (-1), rigid.linearVelocity.y, rigid.linearVelocity.z);
           
        //Landing Platform
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

        //check on inner wall
        if (onInnerWall)
        {
            if (GameManager.instance.isTopView)
                rigid.linearVelocity = new Vector3(rigid.linearVelocity.x, rigid.linearVelocity.y, 0);
            else
                rigid.linearVelocity = new Vector3(rigid.linearVelocity.x, 0, rigid.linearVelocity.z);
        }

        //Jumping
        if (requestJump)
        {
            PerformJump();
            requestJump = false;
        }
    }

    private void Landing()
    {
        isJumping = true;

        Vector3 targetVec = inverted ? Vector3.forward : Vector3.back;
        Vector3 box = new Vector3(0.49f, 0, 0.5f);

        if (!GameManager.instance.isTopView)
        {
            targetVec = Vector3.down;
            box = new Vector3(0.49f, 0.5f, 0);
        }

        Debug.DrawRay(rigid.position, targetVec, Color.yellow);

        RaycastHit[] rayHit = Physics.BoxCastAll(rigid.position, box, targetVec, Quaternion.identity, 0.5f, LayerMask.GetMask("Platform"));

        if (rayHit.Length != 0)
        {

            if (rayHit[0].distance < 0.1f)
            {
               
                isJumping = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
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
}
