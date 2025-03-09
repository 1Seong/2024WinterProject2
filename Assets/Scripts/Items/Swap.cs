using UnityEngine;

public class Swap : Consumable
{
    GameObject player1, player2;

    protected override void Awake()
    {
        base.Awake();
        PlayerTriggerEvent += _ => SwapActivate();
    }

    void Start()
    {
        player1 = GameManager.instance.player1;
        player2 = GameManager.instance.player2;
    }

    private void SwapActivate()
    {
        if (player1 != null && player2 != null)
        {
            Vector3 tempPosition = player1.transform.position;
            player1.transform.position = player2.transform.position;
            player2.transform.position = tempPosition;

            Debug.Log("Swap Item applied: Players swapped positions!");
        }
    }
}
