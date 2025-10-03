using Unity.VisualScripting;
using UnityEngine;

public class Frog : Consumable
{
    public int frogJumpUnit = 2;
    public GameObject frogHatPrefab;
    private AudioSource frogSound;

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
        frogSound = GetComponent<AudioSource>();
    }

    private void FrogActivate(Collider other)
    {
        PlayerJump playerJ = other.GetComponent<PlayerJump>();
        Player player = other.GetComponent<Player>();
        playerJ.jumpUnit = frogJumpUnit;
        Debug.Log("Frog Item applied: Jump unit set to " + playerJ.jumpUnit);

        var anims = other.GetComponentsInChildren<Animator>();
        foreach(var anim in anims)
        {
            anim.SetTrigger("Frog");
        }
        //player.frogHat = frogHatPrefab;
        frogSound.Play();    
        

        Debug.Log("Frog Item applied: Jump unit increased!");
    }
}

