using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SpringMove : Movable
{
    protected override void Start()
    {
        base.Start();
        updateAction += CheckConsideredAsWall;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (onIce) return;

        if(collision.collider.tag == "Player1" ||  collision.collider.tag == "Player2")
        {
            GetComponent<Rigidbody>().linearVelocity = new Vector3(0f, rigid.linearVelocity.y, rigid.linearVelocity.z);
        }
    }

    protected override void IceAction()
    {
        Vector3 targetVec = customGravity.down;
        Vector3 box = new Vector3(0.49f, 0, 0.5f);
        bool iceExist;

        if (GameManager.instance.isSideView)
            box = new Vector3(0.49f, 0.5f, 0);

        //Debug.DrawRay(rigid.position, targetVec, Color.yellow);

        RaycastHit[] rayHit = Physics.BoxCastAll(rigid.position, box, targetVec, Quaternion.identity, 0.5f, LayerMask.GetMask("Platform"));

        iceExist = ObjectExistInRaycast(rayHit, "Ice");

        if (!onIce && iceExist && rigid.linearVelocity.x != 0) // onIce : false -> true
        {
            onIce = true;

           
        }
        else if (onIce && ObjectExistInRaycast(rayHit) && !iceExist) // onIce : true -> false
        {
            onIce = false;
            rigid.linearVelocity = Vector3.zero;

            
        }
    }

    // TODO : refactor - this is also raycast thing
    private void CheckConsideredAsWall()
    {
        Vector3 box = !GameManager.instance.isSideView ? new Vector3(0.49f, 0.1f, 0.49f) : new Vector3(0.49f, 0.49f, 0.1f);

        RaycastHit[] rayHitLeft = Physics.BoxCastAll(rigid.position, box, Vector3.left, Quaternion.identity, 0.5f, LayerMask.GetMask("Platform"));
        RaycastHit[] rayHitRight = Physics.BoxCastAll(rigid.position, box, Vector3.right, Quaternion.identity, 0.5f, LayerMask.GetMask("Platform"));

        bool existLeft = IsExistWallsAndSpringInRayHit(rayHitLeft);
        bool existRight = IsExistWallsAndSpringInRayHit(rayHitRight);

        if (existLeft || existRight)
        {
            rigid.constraints |= RigidbodyConstraints.FreezePositionX;
        }
        else
        {
            rigid.constraints &= ~RigidbodyConstraints.FreezePositionX;
        }
    }

    private bool IsExistWallsAndSpringInRayHit(RaycastHit[] rayHit)
    {
        if (rayHit.Length != 0)
        {
            foreach (var hit in rayHit)
            {
                if ((hit.collider.CompareTag("Inner") || hit.collider.CompareTag("SideWall") || hit.collider.CompareTag("Spring")) && hit.distance < 0.01f)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
