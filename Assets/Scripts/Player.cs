using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int jumpUnit;
    public bool inverted;

    Rigidbody rigid;
    BoxCollider coll;

    private bool requestJump = false;
    private bool isJumping = false;
    private Transform stage;
    private bool originUseGravity;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        coll = GetComponent<BoxCollider>();
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

        if(Input.GetKeyDown(KeyCode.E) && !isJumping)
        {
            ConvertView();
        }
    }

    private void CheckInvert()
    {
        float targetZ = -1.0f + GameManager.instance.currentStage.invertLineZ;

        if(!inverted && rigid.position.z > targetZ)
        {
            inverted = true;
            rigid.useGravity = false;
        }
        else if(inverted &&  rigid.position.z < targetZ)
        {
            inverted = false;
            rigid.useGravity = true;
        }
    }

    private void ConvertView()
    {
        bool topview = GameManager.instance.isTopView;

        //Camera Setting

        StartCoroutine(CameraRotate());
        

        //Wall Setting
        if(topview)
        {
            stage.GetChild(0).gameObject.SetActive(false);
            stage.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            stage.GetChild(0).gameObject.SetActive(true);
            stage.GetChild(1).gameObject.SetActive(true);
        }

        //Physics Setting
        float gravity = Physics.gravity.magnitude;

        if(topview)
        {
            Physics.gravity = new Vector3(0, -gravity, 0);

            originUseGravity = rigid.useGravity;
            rigid.useGravity = true;
            rigid.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            Physics.gravity = new Vector3(0, 0, -gravity);

            rigid.useGravity = originUseGravity;
            rigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

        }

        GameManager.instance.isTopView = !topview;
    }

    IEnumerator CameraRotate()
    {
        bool topview = GameManager.instance.isTopView;
        float radius = GameManager.instance.cameraRotationR;

        float currentPosY = Camera.main.transform.position.y;
        float currentPosZ = Camera.main.transform.position.z;
        float targetPosY = topview ? currentPosY - radius : currentPosY + radius;
        float targetPosZ = topview ? currentPosZ - radius : currentPosZ + radius;

        float currentRotX = Camera.main.transform.rotation.eulerAngles.x;
        float targetRot = topview ? 0.0f : 90.0f;

        float totalTime = GameManager.instance.cameraRotationTime;

        for(float i = 0; i < totalTime; i += Time.fixedDeltaTime)
        {
            float posY = Mathf.Lerp(currentPosY, targetPosY, i / totalTime);
            float posZ = Mathf.Lerp(currentPosZ, targetPosZ, i / totalTime);
            float rotX = Mathf.Lerp(currentRotX, targetRot, i / totalTime);

            Camera.main.transform.position = new Vector3(0, posY, posZ);
            Camera.main.transform.rotation = Quaternion.Euler(new Vector3(rotX, 0, 0));
            yield return new WaitForFixedUpdate();
        }

        Camera.main.transform.position = new Vector3(0, targetPosY, targetPosZ);
        Camera.main.transform.rotation = Quaternion.Euler(new Vector3(targetRot, 0, 0));
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

        //Player Moving
        Vector3 dirVec = rigid.position;
        float h = Input.GetAxisRaw("Horizontal");

        dirVec.x += GameManager.instance.speed * h * Time.fixedDeltaTime;

        rigid.MovePosition(dirVec);

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
        Vector3 box = new Vector3(0.5f, 0, 0.1f);

        if (!GameManager.instance.isTopView)
        {
            targetVec = Vector3.down;
            box = new Vector3(0.5f, 0.1f, 0);
        }

        Debug.DrawRay(rigid.position, targetVec, Color.yellow);

        RaycastHit[] rayHit = Physics.BoxCastAll(rigid.position, box, targetVec, Quaternion.identity, 0.5f, LayerMask.GetMask("Platform"));

        if (rayHit.Length != 0)
        {

            if (rayHit[0].distance < 0.6f)
            {
                Debug.Log(rayHit[0].collider.name);
                isJumping = false;
            }
        }
    }
}
