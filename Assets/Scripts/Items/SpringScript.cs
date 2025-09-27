using UnityEngine;

public class SpringScript : ItemBehavior
{
    public int springJumpUnit;
    public float bias = 1f;
    private float gravity, intial, initialVelocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    protected override void Awake()
    {
        base.Awake();
        PlayerTriggerEvent += SpringActivate;
    }

    private void SpringActivate(Collider other)
    {
        int appliedJumpUnit = other.GetComponent<Player>().frog ? springJumpUnit + 1 : springJumpUnit;

        other.GetComponent<PlayerJump>().isJumping = true;

        Rigidbody objRb = other.GetComponent<Rigidbody>();
        gravity = Physics.gravity.magnitude;
        Debug.Log("applied jump unit : " + appliedJumpUnit);
        initialVelocity = Mathf.Sqrt(2 * gravity * appliedJumpUnit);
        float force = objRb.mass * initialVelocity + bias;

        Debug.Log("force : " + force);
        objRb.linearVelocity = Vector3.zero;
        objRb.AddForce(other.GetComponent<CustomGravity>().up * force, ForceMode.Impulse);  
    }
}
