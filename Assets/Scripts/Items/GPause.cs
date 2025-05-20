using System.Collections;
using UnityEngine;
using static PlayerSelectableInterface;

public class GPause : Consumable
{
    protected override void Awake()
    {
        base.Awake();
        PlayerTriggerEvent += (Collider other) =>
        {
            var movables = color == PlayerColor.blue ? StageManager.instance.stage.defaultMovables : StageManager.instance.stage.invertMovables;
            
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
