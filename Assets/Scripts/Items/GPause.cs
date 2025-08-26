using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerSelectableInterface;

public class GPause : Consumable
{
    public static event Action GpauseBlueEvent;
    public static event Action GpausePinkEvent;

    protected override void Awake()
    {
        base.Awake();
        PlayerTriggerEvent += (Collider other) =>
        {
            List<Movable> movables;
            if(color == PlayerColor.blue)
            {
                movables = StageManager.instance.stage.defaultMovables;
                GpauseBlueEvent?.Invoke();
            }
            else
            {
                movables = StageManager.instance.stage.invertMovables;
                GpausePinkEvent?.Invoke();
            }
            
            foreach (var i in movables)
                i.CallGPauseAction();
        };
    }

    /////////////////// NOT USED ////////////////////
    /*
    private void GPauseActivate(GameObject obj)
    {   

        gravity = obj.GetComponent<Player>();
        if (gravity != null)
        {
            Debug.Log("calling CustomGravity");
            gravity.CallGPauseAction();
        }
    }
    */
}
