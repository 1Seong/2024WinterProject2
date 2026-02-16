using UnityEngine;

public class DoorForceClose : MonoBehaviour
{
    Door door;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Door")) return;

        door = other.GetComponent<Door>();
    }

    
}
