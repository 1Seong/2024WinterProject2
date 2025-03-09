using System;
using UnityEngine;

public class Player : Movable
{
    private void OnEnable()
    {
        StageManager.instance.stageClearEvent += GetComponent<Transparent>().CallFade;
    }

    protected override void IceAction()
    {
        Vector3 targetVec = customGravity.down;
        Vector3 box = new Vector3(0.49f, 0, 0.5f);
        bool iceExist;

        Func<RaycastHit[], bool> IceExist = (RaycastHit[] rayHit) =>
        {
            if (rayHit.Length == 0) return false;

            foreach (var i in rayHit)
                if (i.distance < 0.1f && i.transform.tag == "Ice")
                    return true;

            return false;
        };

        Func<RaycastHit[], bool> PlatformExist = (RaycastHit[] rayHit) =>
        {
            if (rayHit.Length == 0) return false;

            foreach (var i in rayHit)
                if (i.distance < 0.1f)
                    return true;

            return false;
        };

        if (GameManager.instance.isSideView)
            box = new Vector3(0.49f, 0.5f, 0);

        //Debug.DrawRay(rigid.position, targetVec, Color.yellow);

        RaycastHit[] rayHit = Physics.BoxCastAll(rigid.position, box, targetVec, Quaternion.identity, 0.5f, LayerMask.GetMask("Platform"));

        iceExist = IceExist(rayHit);

        if (!onIce && iceExist && rigid.linearVelocity.x != 0) // onIce : false -> true
        {
            onIce = true;
           
            GetComponent<PlayerMove>().enabled = false;
        }
        else if (onIce && PlatformExist(rayHit) && !iceExist) // onIce : true -> false
        {
            onIce = false;
            rigid.linearVelocity = Vector3.zero;
            
            GetComponent<PlayerMove>().enabled = true;
        }
    }

    private void OnDestroy()
    {
        StageManager.instance.stageClearEvent -= GetComponent<Transparent>().CallFade;
    }
}
