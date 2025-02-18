using UnityEngine;

public class SpringScript : ItemBehavior
{
    public int springJumpUnit;
    private float gravity, intial, initialVelocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isConsumable = false;
    }

    new void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        Debug.Log("child triggered!");
        SpringActivate(other.gameObject);
        base.OnTriggerEnter(other);
    }

    private void SpringActivate(GameObject obj)
    {
        if (obj.tag == "Player")
        {
            Rigidbody objRb = obj.GetComponent<Rigidbody>();
            gravity = Physics.gravity.magnitude;
            initialVelocity = Mathf.Sqrt(2 * gravity * springJumpUnit);
            float force = objRb.mass * initialVelocity + 0.5f;
            if (!GameManager.instance.isSideView)
                obj.GetComponent<Rigidbody>().AddForce(Vector3.forward * force, ForceMode.Impulse);
            else
                obj.GetComponent<Rigidbody>().AddForce(Vector3.back * force, ForceMode.Impulse);
        } 
    }

}
