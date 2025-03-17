using System.Collections;
using UnityEngine;
using static PlayerSelectableInterface;

public class GPause : Consumable
{
    protected override void Awake()
    {
        base.Awake();
        PlayerTriggerEvent += _ =>
        {
            foreach (var i in StageManager.instance.stage.movables)
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
