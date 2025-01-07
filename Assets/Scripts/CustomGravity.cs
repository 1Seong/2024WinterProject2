using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    private Vector3 customGravity;

    Player player;
    Rigidbody rigid;

    void Awake()
    {
        /*
         * Awake
         */
        player = GetComponent<Player>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        /*
         * Start
         */
        customGravity = GameManager.instance.isTopView ? Physics.gravity : (!player.inverted ? Physics.gravity : -Physics.gravity);
    }

    void FixedUpdate()
    {
        /*
         * Fixed Update
         */
        if (!GameManager.instance.isPlaying || player.onInnerWall)
        {
            return;
        }

        rigid.AddForce(customGravity, ForceMode.Acceleration);
    }

    public void InvertGravity()
    {
        /*
         * Invert Gravity
         */
        customGravity = -Physics.gravity;
    }

    public void ReapplyGravity()
    {
        /*
         * Reinitialize customGravity
         */
        customGravity = Physics.gravity;
    }
}
