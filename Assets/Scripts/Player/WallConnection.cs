using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class WallConnection : MonoBehaviour
{
    private enum Mode { Player, Spring }
    [SerializeField] private Mode _mode;

    [SerializeField] private Collider _leftColl;
    [SerializeField] private Collider _rightColl;

    private Rigidbody _rigid;
    private bool _contactWithPlayer = false;
    [SerializeField] private bool _doUpdate = true;

    [SerializeField]private bool _connectedToWall = false;
    public bool connectedToWall
    {
        get => _connectedToWall;
        set
        {
            _connectedToWall = value;
            if(_mode == Mode.Spring)
            {
                if (value)
                    _rigid.constraints |= RigidbodyConstraints.FreezePositionX;
                else
                    _rigid.constraints &= ~RigidbodyConstraints.FreezePositionX;
            }
        }
    }

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!_doUpdate) return;
        Vector3 box = !GameManager.instance.isSideView ? new Vector3(0.03f, 0.1f, 0.44f) : new Vector3(0.03f, 0.44f, 7.5f);

        RaycastHit[] rayHitLeft = Physics.BoxCastAll(_rigid.position + new Vector3(-0.5f, 0f, 0f), box, Vector3.left, Quaternion.identity, 0.5f, LayerMask.GetMask("Platform"));
        RaycastHit[] rayHitRight = Physics.BoxCastAll(_rigid.position + new Vector3(0.5f, 0f, 0f), box, Vector3.right, Quaternion.identity, 0.5f, LayerMask.GetMask("Platform"));

        RaycastHit? existLeft = ExistWallsInRayHit(rayHitLeft);
        RaycastHit? existRight = ExistWallsInRayHit(rayHitRight);

        if(existLeft != null)
        {
            _leftColl = existLeft.Value.collider;
            connectedToWall = true;
        }
        else
        {
            _leftColl = null;
        }
        if(existRight != null)
        {
            _rightColl = existRight.Value.collider;
            connectedToWall = true;
        }
        else
        {
            _rightColl = null;
        }
        if (existLeft == null && existRight == null)
            connectedToWall = false;
    }

    private RaycastHit? ExistWallsInRayHit(RaycastHit[] rayHit)
    {
        if (rayHit.Length != 0)
        {
            foreach (var hit in rayHit)
            {
                // "platform" is considered as a wall
                // except - "Player1", "Player2", and "Spring" - need to check if they are connected to a wall
                if (hit.distance > 0.001f) continue;
                if (hit.collider.CompareTag("Ice")) continue;
                if (hit.collider.CompareTag("Spring") || hit.collider.CompareTag("Player1") || hit.collider.CompareTag("Player2"))
                {
                    if (hit.collider.transform == transform || hit.transform == transform) continue; // ignore self detection

                    if (hit.collider.GetComponent<WallConnection>().connectedToWall)
                    {
                        if (hit.collider.GetComponent<WallConnection>().ConnectedWith(GetComponent<Collider>())) // prevent deadlock
                            continue;
                        else
                            return hit;
                    }
                }
                else // other platforms
                    return hit;
            }
        }
        return null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (_mode == Mode.Spring)
        {
            if (!collision.collider.CompareTag("Player1") && !collision.collider.CompareTag("Player2")) return;

            SetUpdate(true);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (_mode == Mode.Player)
        {
            if (!collision.collider.CompareTag("Spring")) return;

            var springCon = collision.collider.GetComponent<WallConnection>();
            var moveDir = Input.GetAxisRaw("Horizontal");
            //Debug.Log("stay - moveDir " + moveDir);
            if(moveDir > 0 && !springCon.IsExistRight())
            {
                springCon.SetUpdate(false);
                springCon.connectedToWall = false;
            }
            else if(moveDir < 0 && !springCon.IsExistLeft())
            {
                springCon.SetUpdate(false);
                springCon.connectedToWall = false;
            }
        }
    }

    public bool IsExistLeft() 
    {
        if (_leftColl == null)
            return false;
        else
        {
            if (_leftColl.CompareTag("Spring"))
            {
                if (_leftColl.GetComponent<WallConnection>().IsExistLeft())
                    return true;
                else
                    return false;
            }
            else
                return true;
        }
    }

    public bool IsExistRight()
    {
        if (_rightColl == null)
            return false;
        else
        {
            if (_rightColl.CompareTag("Spring"))
            {
                if (_rightColl.GetComponent<WallConnection>().IsExistRight())
                    return true;
                else
                    return false;
            }
            else
                return true;
        }
    }

    public bool ConnectedWith(Collider coll)
    {
        if (coll == _leftColl || coll == _rightColl) return true;
        else return false;
    }

    public void SetUpdate(bool b) { _doUpdate = b; }
}
