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
            List<Movable> pauseTarget;
            if(color == PlayerColor.blue)
            {
                movables = StageManager.instance.stage.defaultMovables;
                pauseTarget = StageManager.instance.stage.invertMovables;
                GpauseBlueEvent?.Invoke();
            }
            else
            {
                movables = StageManager.instance.stage.invertMovables;
                pauseTarget = StageManager.instance.stage.defaultMovables;
                GpausePinkEvent?.Invoke();
            }
            
            foreach (var i in movables)
                i.CallGPauseAction();

            foreach (var i in pauseTarget)
                i.CallPauseInvert();
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
