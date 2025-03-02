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
    }

    private void SpringActivate(Collider other)
    {
        Rigidbody objRb = other.GetComponent<Rigidbody>();
        gravity = Physics.gravity.magnitude;
        initialVelocity = Mathf.Sqrt(2 * gravity * springJumpUnit);
        float force = objRb.mass * initialVelocity + 0.5f;
        objRb.AddForce(other.GetComponent<CustomGravity>().up * force, ForceMode.Impulse);  
    }
}
