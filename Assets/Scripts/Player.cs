using System;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : Movable
{
    public bool frog = false;

    private int dir = 0;
    private GameObject _frogHat;

    public GameObject frogHat
    {
        set
        {
            _frogHat = value;
            if(value != null) Instantiate(value, transform);
            frog = true;
        }
        get => _frogHat;
    }

    // TODO : refactor - do raycasting once and extract info from each classes
    protected override void IceAction()
    {
        Vector3 targetVec = customGravity.down;
        Vector3 box = new Vector3(0.49f, 0.1f, 0.5f);
        bool iceExist;

        Func<RaycastHit[], bool> IceExist = (RaycastHit[] rayHit) =>
        {
            if (rayHit.Length == 0) return false;

            foreach (var i in rayHit)
                if (i.distance < 0.2f && i.collider.CompareTag("Ice"))
                    return true;

            return false;
        };

        Func<RaycastHit[], bool> PlatformExist = (RaycastHit[] rayHit) =>
        {
            if (rayHit.Length == 0) return false;

            foreach (var i in rayHit)
                if (i.distance < 0.1f && !i.collider.CompareTag(tag))
                    return true;

            return false;
        };

        if (GameManager.instance.isSideView)
            box = new Vector3(0.49f, 0.5f, 0.1f);

        Debug.DrawRay(rigid.position, targetVec, Color.green);

        RaycastHit[] rayHit = Physics.BoxCastAll(rigid.position, box, targetVec, Quaternion.identity, 0.5f, LayerMask.GetMask("Platform"));

        iceExist = IceExist(rayHit);

        int newDir = rigid.linearVelocity.x > 0 ? 1 : (rigid.linearVelocity.x == 0 ? 0 : -1);

        if (!onIce && iceExist && rigid.linearVelocity.x != 0) // onIce : false -> true
        {
            onIce = true;
           
            GetComponent<PlayerMove>().enabled = false;
        }
        else if (onIce && (PlatformExist(rayHit) && !iceExist || newDir * dir <= 0)) // onIce : true -> false
        {
            Debug.Log("On Ice false");
            rigid.linearVelocity = Vector3.zero;
            onIce = false;
            
            GetComponent<PlayerMove>().enabled = true;
        }

        dir = newDir;
    }
}
