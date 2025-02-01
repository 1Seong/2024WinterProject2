using UnityEngine;

public class SpringTest : ItemBehavior
{
    public int springJumpUnit = 4;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isConsumable = true;
    }

    new void OnTriggerEnter(Collider other)
    {
        Debug.Log("child triggered!");
        
        SpringActivate(other.gameObject);
        base.OnTriggerEnter(other);
    }

    private void SpringActivate(GameObject obj)
    {
        if (obj.tag == "player")
        {
            Rigidbody objRb = obj.GetComponent<Rigidbody>();
            float gravity = Physics.gravity.magnitude;
            float initialVelocity = Mathf.Sqrt(2 * gravity * springJumpUnit);
            float force = objRb.mass * initialVelocity + 0.5f;
            if (!GameManager.instance.isSideView)
                obj.GetComponent<Rigidbody>().AddForce(Vector3.forward * force, ForceMode.Impulse);
            else
                obj.GetComponent<Rigidbody>().AddForce(Vector3.back * force, ForceMode.Impulse);
        } 
    }

}
