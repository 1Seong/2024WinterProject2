using UnityEngine;

public class PushSpring : MonoBehaviour
{
    public float force = 0.5f;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Spring")) return;

        collision.rigidbody.AddForce(new Vector3(force, 0, 0), ForceMode.Force);
    }
}
