using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    private enum GravityState { defaultG, invertG, convertG };
    private GravityState _gravityState;

    [SerializeField] private Vector3 _currentGravity;

    public Vector3 up;
    public Vector3 down;
    
    private GravityState _gravityStateP
    {
        get => _gravityState;
        set
        {
            switch(value)
            {
                case GravityState.invertG:
                    _currentGravity = new Vector3(0, 0, 9.81f);
                    up = Vector3.back;
                    down = Vector3.forward;
                    break;
                case GravityState.convertG:
                    _currentGravity = new Vector3(0, -9.81f, 0);
                    up = Vector3.up;
                    down = Vector3.down;
                    break;
                case GravityState.defaultG:
                default:
                    _currentGravity = new Vector3(0, 0, -9.81f);
                    up = Vector3.forward;
                    down = Vector3.back;
                    break;
            }
        }
    }

    Player player;
    Rigidbody rigid;

    void Awake()
    {
        if(tag == "Player")
            player = GetComponent<Player>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _gravityStateP = _gravityState;
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isPlaying || player.onInnerWall)
        {
            return;
        }

        rigid.AddForce(_currentGravity, ForceMode.Acceleration);
    }

    public void SetToDefault()
    {
        _gravityStateP = GravityState.defaultG;
    }

    public void SetToInvert()
    {
        _gravityStateP = GravityState.invertG;
    }

    public void SetToConvert()
    {
        _gravityStateP = GravityState.convertG;
    }

}
