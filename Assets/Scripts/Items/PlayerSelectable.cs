using UnityEngine;
using static PlayerSelectableInterface;

public class PlayerSelectable : PlayerSelectableInterface
{
    public bool CheckColor(Collider other, int playerId)
    {
        if (playerId == 1 && other.CompareTag("Player1") || playerId == 2 && other.CompareTag("Player2"))
            return true;
        else return false;
    }
}
