using UnityEngine;

public class SpringScript : ItemBehavior
{
    public int springJumpUnit;
    private float gravity, intial, initialVelocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    protected override void Awake()
    {
        base.Awake();
        PlayerTriggerEvent += SpringActivate;
        Stage.convertEvent += ConvertSpringCollider;
    }

    private void OnDestroy()
    {
        Stage.convertEvent -= ConvertSpringCollider;
    }

    private void SpringActivate(Collider other)
    {
        int appliedJumpUnit = other.GetComponent<Player>().frog ? springJumpUnit + 1 : springJumpUnit;

        other.GetComponent<PlayerJump>().isJumping = true;

        Rigidbody objRb = other.GetComponent<Rigidbody>();
        gravity = Physics.gravity.magnitude;
        Debug.Log("applied jump unit : " + appliedJumpUnit);
        initialVelocity = Mathf.Sqrt(2 * gravity * appliedJumpUnit);
        float force = objRb.mass * initialVelocity + 0.5f;

        Debug.Log("force : " + force);
        objRb.linearVelocity = Vector3.zero;
        objRb.AddForce(other.GetComponent<CustomGravity>().up * force, ForceMode.Impulse);  
    }

    private void ConvertSpringCollider()
    {
        if(GameManager.instance.isSideView) // top -> side
            GetComponent<BoxCollider>().size = new Vector3(1f, 1f, 16f);
        else
            GetComponent<BoxCollider>().size = new Vector3(1f, 1f, 1f);
    }
}
