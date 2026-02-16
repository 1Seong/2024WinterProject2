using UnityEngine;

public class SpringScript : ItemBehavior
{
    public int springJumpUnit;
    public float bias = 1f;
    private float gravity, intial, initialVelocity;
    private static bool isActive1, isActive2;
    private AudioSource springSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    protected override void Awake()
    {
        base.Awake();
        PlayerTriggerEvent += SpringActivate;
        springSound = GetComponent<AudioSource>();

        isActive1 = false;
        isActive2 = false;
    }

    private void SpringActivate(Collider other)
    {
        if (other.CompareTag("Player1") && isActive1 || other.CompareTag("Player2") && isActive2) return;

        if (!other.GetComponent<PlayerJump>().isJumping) return;

        if (other.CompareTag("Player1"))
            isActive1 = true;
        else if(other.CompareTag("Player2"))
            isActive2 = true;

        int appliedJumpUnit = other.GetComponent<Player>().frog ? springJumpUnit + 1 : springJumpUnit;

        other.GetComponent<PlayerJump>().isJumping = true;
        var anims = other.GetComponentsInChildren<Animator>();
        foreach (var anim in anims)
            anim.SetBool("JumpPad", true);


        Rigidbody objRb = other.GetComponent<Rigidbody>();
        gravity = Physics.gravity.magnitude;
        Debug.Log("applied jump unit : " + appliedJumpUnit);
        initialVelocity = Mathf.Sqrt(2 * gravity * appliedJumpUnit);
        float force = objRb.mass * initialVelocity + bias;

        Debug.Log("force : " + force);
        objRb.linearVelocity = Vector3.zero;
        objRb.AddForce(other.GetComponent<CustomGravity>().up * force, ForceMode.Impulse);

        if (other.CompareTag("Player1"))
            Invoke("active1Off", 0.1f);
        else if (other.CompareTag("Player2"))
            Invoke("active2Off", 0.1f);

        springSound.Play();
    }

    private void active1Off()
    {
        isActive1 = false;
    }

    private void active2Off()
    {
        isActive2 = false;
    }
}
