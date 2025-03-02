using UnityEngine;
using static PlayerSelectableInterface;

public class PlayerSelectable : PlayerSelectableInterface
{
    public bool CheckColor(Collider other, int playerId)
    {
        if (playerId == 1 && other.tag == "Player1" || playerId == 2 && other.tag == "Player2")
            return true;
        else return false;
    }
}
