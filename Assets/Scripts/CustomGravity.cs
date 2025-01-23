using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    private enum GravityState { defaultG, invertG, convertG };
    private GravityState _gravityState;

    [SerializeField] private Vector3 _currentGravity;

    public Vector3 up;
    public Vector3 down;

    public static event Action gPauseAtDefaultEvent;
    public static event Action gPauseAtInvertEvent;

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

    private GravityState _stateBeforeConvert;

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

        Stage.convertEvent += ConvertAction;
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
        rigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }

    public void SetToInvert()
    {
        _gravityStateP = GravityState.invertG;
        rigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }

    public void SetToConvert()
    {
        _stateBeforeConvert = _gravityStateP;

        _gravityStateP = GravityState.convertG;
        rigid.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }

    private void Inversion()
    {
        //월선할때 호출
        Action callGPauseAction = () => StartCoroutine(GPauseAction());

        if (_gravityStateP == GravityState.defaultG)
        {
            gPauseAtDefaultEvent -= callGPauseAction;
            gPauseAtInvertEvent += callGPauseAction;
        }
        else if (_gravityStateP == GravityState.invertG)
        {
            gPauseAtDefaultEvent += callGPauseAction;
            gPauseAtInvertEvent -= callGPauseAction;
        }
        else return;
        
        InvertAction();
    }

    private void InvertAction()
    {
        if (_gravityStateP == GravityState.defaultG)
            SetToInvert();
        else
            SetToDefault();
    }

    private void ConvertAction()
    {
        if(GameManager.instance.isTopView)
            SetToConvert();
        else
        {
            if(_stateBeforeConvert == GravityState.defaultG)
                SetToDefault();
            else
                SetToInvert();
        }
    }

    private void GPause()
    {
        //GPause 활성화될때 호출
        if (_gravityStateP == GravityState.defaultG)
            gPauseAtDefaultEvent?.Invoke();
        else if (_gravityStateP == GravityState.invertG)
            gPauseAtInvertEvent?.Invoke();
    }

    IEnumerator GPauseAction()
    {
        InvertAction();
        yield return new WaitForSeconds(10f);
        InvertAction();
    }
}
