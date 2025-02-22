using UnityEngine;

public interface PlayerSelectableInterface
{
    enum PlayerColor { pink, blue };
    PlayerColor Color { get; set; }

    bool CheckColor(Collider other)
    {
        if (Color == PlayerColor.pink && other.tag == "Player1" || Color == PlayerColor.blue && other.tag == "Player2")
            return true;
        else return false;
    }
}
