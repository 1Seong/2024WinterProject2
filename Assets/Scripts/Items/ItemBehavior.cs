using System;
using UnityEditor.Animations;
using UnityEngine;

public abstract class ItemBehavior : MonoBehaviour
{
    public enum PlayerColor { noneSelectable, pink, blue };
    [SerializeField]protected PlayerColor color;

    private Animator[] animator;
    private SpriteRenderer[] spriter;
    [SerializeField] private RuntimeAnimatorController blueAc;
    [SerializeField] private RuntimeAnimatorController redAc;
    [SerializeField] private Sprite blueSprite;
    [SerializeField] private Sprite redSprite;

    protected PlayerSelectableInterface playerSelectable;

    protected event Action<Collider> PlayerTriggerEvent;

    protected virtual void Awake()
    {
        animator = GetComponentsInChildren<Animator>();
        spriter = GetComponentsInChildren<SpriteRenderer>();

        if (color == PlayerColor.noneSelectable)
            playerSelectable = new PlayerNonSelect();
        else
            playerSelectable = new PlayerSelectable();

        if(animator != null && animator.Length != 0)
        {
            if (color == PlayerColor.blue)
                foreach(var a in animator)
                    a.runtimeAnimatorController = blueAc;
            else if (color == PlayerColor.pink)
                foreach (var a in animator)
                    a.runtimeAnimatorController = redAc;
        }
        else
        {
            if (color == PlayerColor.blue)
                foreach(var s in spriter)
                    s.sprite = blueSprite;
            else if (color == PlayerColor.pink)
                foreach (var s in spriter)
                    s.sprite = redSprite;
        }
    }

    virtual protected void OnTriggerEnter(Collider other)
    {
        if (playerSelectable is PlayerSelectable && !PerformPlayerCheck(other, (int)color)) return;

        //Debug.Log("parent trigger!");
        if (other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
        { 
            PlayerTriggerEvent?.Invoke(other);
        }
    }

    protected bool PerformPlayerCheck(Collider other, int id)
    {
        return playerSelectable.CheckColor(other, id);
    }
}
