using UnityEngine;

public class Player : MonoBehaviour
{
    public int jumpUnit;
    public bool inverted;

    Rigidbody rigid;
    BoxCollider coll;

    private bool requestJump = false;
    private bool isJumping = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        coll = GetComponent<BoxCollider>();
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
    }

    private void PerformJump()
    {
        isJumping = true;

        float gravity = Mathf.Abs(Physics.gravity.z);

        float initialVelocity = Mathf.Sqrt(2 * gravity * jumpUnit);
        float force = rigid.mass * initialVelocity + 0.5f;

        if (inverted)
            force = -force;

        rigid.AddForce(Vector3.forward * force, ForceMode.Impulse);
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
        if (!inverted && rigid.linearVelocity.z < 0.0f || inverted && rigid.linearVelocity.z > 0.0f)
        {
            Landing();
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
        Vector3 targetVec = inverted ? Vector3.forward : Vector3.back;

        Debug.DrawRay(rigid.position, targetVec, Color.yellow);

        RaycastHit[] rayHit = Physics.BoxCastAll(rigid.position, new Vector3(0.5f, 0, 0.1f), targetVec, Quaternion.identity, 0.5f, LayerMask.GetMask("Platform"));

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
