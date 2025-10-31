using UnityEngine;

public class Swap : Consumable
{
    private AudioSource swapSound;
    protected override void Awake()
    {
        base.Awake();
        PlayerTriggerEvent += _ => SwapActivate();
        swapSound = GetComponent<AudioSource>();
    }

    private void SwapActivate()
    {
        var player1 = StageManager.instance.stage.player1;
        var player2 = StageManager.instance.stage.player2;
        var player1Rb = player1.GetComponent<Rigidbody>();
        var player2Rb = player2.GetComponent<Rigidbody>();

        var tempPosition = player1.transform.position;
        var tempVel = player1Rb.linearVelocity;
        player1.transform.position = player2.transform.position;
        player2.transform.position = tempPosition;
        player1Rb.linearVelocity = player2Rb.linearVelocity;
        player2Rb.linearVelocity = tempVel;

        //var tempVel = player1.

        Debug.Log("Swap Item applied: Players swapped positions!");
        AudioManager.instance.PlayItemClip(AudioManager.Item.swap);
    }
}
