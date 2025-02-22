using System.Collections;
using UnityEngine;
using static PlayerSelectableInterface;

public class GPause : ItemBehavior, PlayerSelectableInterface
{
    Movable player;

    public PlayerColor Color { get; set; }
    [SerializeField] private PlayerColor color;

    private void Start()
    {
        Color = color;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (((PlayerSelectableInterface)this).CheckColor(other) == false) return;
        if (GameManager.instance.gpauseActive) return;
        Debug.Log("child triggered!");
        player = other.GetComponent<Movable>();
        PlayerTriggerEvent += _ => player.CallGPauseAction();
        base.OnTriggerEnter(other);
    }


    /////////////////// NOT USED ////////////////////
    /*
    private void GPauseActivate(GameObject obj)
    {   

        gravity = obj.GetComponent<Player>();
        if (gravity != null)
        {
            Debug.Log("calling CustomGravity");
            gravity.CallGPauseAction();
        }
    }
    */
}
