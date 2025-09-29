using UnityEngine;

public class GPausePlane : MonoBehaviour
{
    public void ObjectDestroy()
    {
        Destroy(transform.parent.gameObject);
    }
}
