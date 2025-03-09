using UnityEngine;

public class PlayerNonSelect : PlayerSelectableInterface
{
    public bool CheckColor(Collider other, int playerId) { return true; }
}
