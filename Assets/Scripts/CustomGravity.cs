using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    private Vector3 customGravity = new Vector3(0, 0, 9.81f);

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
        if (!player.inverted)
        {
            return;
        }
        rigid.AddForce(customGravity, ForceMode.Acceleration);
    }
}
