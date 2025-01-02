using UnityEngine;

public class Player : MonoBehaviour
{
    public int jumpUnit;

    Rigidbody rigid;
    BoxCollider coll;

    private bool isJumping = false;
    private bool inverted = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        coll = GetComponent<BoxCollider>();
    }
    
    private void Update()
    {
        //Jump
        if(Input.GetButtonDown("Jump") && !isJumping)
        {
            PerformJump();
        }
    }

    private void PerformJump()
    {
        isJumping = true;

        float gravity = Mathf.Abs(Physics.gravity.z);

        float initialVelocity = Mathf.Sqrt(2 * gravity * jumpUnit);
        float force = rigid.mass * initialVelocity + 0.5f;

        rigid.AddForce(Vector3.forward * force, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        Vector3 dirVec = rigid.position;
        float h = Input.GetAxisRaw("Horizontal");

        dirVec.x += GameManager.instance.speed * h * Time.fixedDeltaTime;

        rigid.MovePosition(dirVec);

        //Landing Platform
        if(rigid.linearVelocity.z < 0.0f)
        {
            Debug.DrawRay(rigid.position, Vector3.back, Color.yellow);

            RaycastHit[] rayHit = Physics.RaycastAll(rigid.position, Vector3.back, 0.8f, LayerMask.GetMask("Platform"));
            
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
}
