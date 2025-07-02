using Unity.VisualScripting;
using UnityEngine;

public class Frog : Consumable
{
    public int frogJumpUnit = 2;
    public GameObject frogHatPrefab;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player1" && other.tag != "Player2") return;
        if (other.GetComponent<Player>().frog) return;
        base.OnTriggerEnter(other);
    }

    protected override void Awake()
    {
        base.Awake();
        PlayerTriggerEvent += FrogActivate;
    }

    private void FrogActivate(Collider other)
    {
        PlayerJump playerJ = other.GetComponent<PlayerJump>();
        Player player = other.GetComponent<Player>();
        playerJ.jumpUnit = frogJumpUnit;
        Debug.Log("Frog Item applied: Jump unit set to " + playerJ.jumpUnit);

         
        player.frogHat = frogHatPrefab;
            
        

        Debug.Log("Frog Item applied: Jump unit increased!");
    }
}

