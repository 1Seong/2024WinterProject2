using UnityEngine;

public class SimpleSpring : MonoBehaviour
{
    public int springJumpUnit = 4;
    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var obj = other.gameObject;
        if (obj.tag == "Player")
        {
            springAction(obj);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void springAction(GameObject obj)
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
