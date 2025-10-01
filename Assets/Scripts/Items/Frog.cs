using Unity.VisualScripting;
using UnityEngine;

public class Frog : Consumable
{
    public int frogJumpUnit = 2;
    public GameObject frogHatPrefab;
    private bool isActive = false;

    protected override void OnTriggerEnter(Collider other)
    {
        if (isActive) return;
        if (other.tag != "Player1" && other.tag != "Player2") return;
        if (other.GetComponent<Player>().frog) return;
        isActive = true;
        base.OnTriggerEnter(other);
    }

    protected override void Awake()
    {
        base.Awake();
        PlayerTriggerEvent += FrogActivate;
    }

    private void FrogActivate(Collider other)
    {
        other.GetComponent<Player>().frog = true;
        PlayerJump playerJ = other.GetComponent<PlayerJump>();
        
        playerJ.jumpUnit = frogJumpUnit;
        Debug.Log("Frog Item applied: Jump unit set to " + playerJ.jumpUnit);

        var anims = other.GetComponentsInChildren<Animator>();
        foreach(var anim in anims)
        {
            anim.SetBool("Frog", true);
        }
        //player.frogHat = frogHatPrefab;
            
        

        Debug.Log("Frog Item applied: Jump unit increased!");
    }
}

