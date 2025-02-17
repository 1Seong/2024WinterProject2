using UnityEngine;

public class Swap : ItemBehavior
{
    GameObject player1, player2;

    void Start()
    {
        isConsumable = true;
        player1 = GameManager.instance.player1;
        player2 = GameManager.instance.player2;
    }

    new void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        Debug.Log("child triggered!");
        SwapActivate();
        base.OnTriggerEnter(other);
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
