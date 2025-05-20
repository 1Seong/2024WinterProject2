using UnityEngine;

public class SpringMove : Movable
{
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
}
