using System;
using UnityEngine;

public abstract class ItemBehavior : MonoBehaviour
{
    public enum PlayerColor { noneSelectable, pink, blue };
    [SerializeField]protected PlayerColor color;

    protected PlayerSelectableInterface playerSelectable;

    protected event Action<Collider> PlayerTriggerEvent;

    protected virtual void Awake()
    {
        if (color == PlayerColor.noneSelectable)
            playerSelectable = new PlayerNonSelect();
        else
            playerSelectable = new PlayerSelectable();
    }

    virtual protected void OnTriggerEnter(Collider other)
    {
        if (playerSelectable is PlayerSelectable && !PerformPlayerCheck(other, (int)color)) return;

        Debug.Log("parent trigger!");
        if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2")
        { 
            PlayerTriggerEvent?.Invoke(other);
        }
    }

    protected bool PerformPlayerCheck(Collider other, int id)
    {
        return playerSelectable.CheckColor(other, id);
    }
}
