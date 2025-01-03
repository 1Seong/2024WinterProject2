using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    private Vector3 customGravity = -Physics.gravity;

    Player player;
    Rigidbody rigid;

    void Awake()
    {
        player = GetComponent<Player>();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameManager.instance.isPlaying || !player.inverted)
        {
            return;
        }
        rigid.AddForce(customGravity, ForceMode.Acceleration);
    }
}
