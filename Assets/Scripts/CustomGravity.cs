using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    private Vector3 customGravity;

    Player player;
    Rigidbody rigid;

    void Awake()
    {
        player = GetComponent<Player>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        customGravity = GameManager.instance.isTopView ? Physics.gravity : (!player.inverted ? Physics.gravity : -Physics.gravity);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameManager.instance.isPlaying || player.onInnerWall)
        {
            return;
        }

        rigid.AddForce(customGravity, ForceMode.Acceleration);
    }

    public void InvertGravity()
    {
        customGravity = -Physics.gravity;
    }

    public void ReapplyGravity()
    {
        customGravity = Physics.gravity;
    }
}
