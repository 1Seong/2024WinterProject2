using UnityEngine;

public class Swap : Consumable
{
    protected override void Awake()
    {
        base.Awake();
        PlayerTriggerEvent += _ => SwapActivate();
    }

    private void SwapActivate()
    {
        var player1 = StageManager.instance.stage.player1;
        var player2 = StageManager.instance.stage.player2;

        var tempPosition = player1.transform.position;
        player1.transform.position = player2.transform.position;
        player2.transform.position = tempPosition;

        Debug.Log("Swap Item applied: Players swapped positions!");
        
    }
}
