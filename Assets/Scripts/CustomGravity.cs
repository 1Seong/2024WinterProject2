using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public enum GravityState { defaultG, invertG, convertG };

public class CustomGravity : MonoBehaviour
{
    [SerializeField] private GravityState _gravityState;

    private Vector3 _currentGravity;

    public Vector3 up;
    public Vector3 down;

    public static event Action gPauseAtDefaultEvent;
    public static event Action gPauseAtInvertEvent;

    public GravityState gravityState
    {
        get => _gravityState;
        set
        {
            _gravityState = value;
            switch (value)
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

    Movable movable;
    Rigidbody rigid;

    void Awake()
    {
        
        movable = GetComponent<Movable>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        gravityState = _gravityState;
        //it may cause problem when stage start in top view state
        //if (gravityState == GravityState.defaultG)
        //    gPauseAtDefaultEvent += CallGPauseAction;
        //else if (gravityState == GravityState.invertG)
        //    gPauseAtInvertEvent += CallGPauseAction;

        Stage.convertEvent += ConvertAction;

        if(movable != null)
            movable.invertEvent += Inversion;
    }

    private void OnDestroy()
    {
        Stage.convertEvent -= ConvertAction;
        //gPauseAtDefaultEvent -= CallGPauseAction;
        //gPauseAtInvertEvent -= CallGPauseAction;
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isPlaying || movable != null && movable.onInnerWall)
            return;

        rigid.AddForce(_currentGravity, ForceMode.Acceleration);
    }

    public void SetToDefault()
    {
        gravityState = GravityState.defaultG;
        rigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }

    public void SetToInvert()
    {
        gravityState = GravityState.invertG;
        rigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }

    public void SetToConvert()
    {
        _stateBeforeConvert = gravityState;

        gravityState = GravityState.convertG;
        rigid.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }

    private void Inversion()
    {
        //if (gravityState == GravityState.defaultG)
        //{
        //    gPauseAtDefaultEvent -= CallGPauseAction;
        //    gPauseAtInvertEvent += CallGPauseAction;
        //}
        //else if (gravityState == GravityState.invertG)
        //{
        //    gPauseAtDefaultEvent += CallGPauseAction;
        //    gPauseAtInvertEvent -= CallGPauseAction;
        //}
        //else return;
        
        InvertAction();
    }

    private void InvertAction()
    {
        Debug.Log("InvertAction Invoked");
        if (gravityState == GravityState.defaultG)
            SetToInvert();
        else
            SetToDefault();
    }

    private void ConvertAction()
    {
        if(GameManager.instance.isSideView)
            SetToConvert();
        else
        {
            if(_stateBeforeConvert == GravityState.defaultG)
                SetToDefault();
            else
                SetToInvert();
        }
    }

    
    public void GPause()
    {
        if (gravityState == GravityState.defaultG)
            gPauseAtDefaultEvent?.Invoke();
        else if (gravityState == GravityState.invertG)
            gPauseAtInvertEvent?.Invoke();
    }
    

    //public void CallGPauseAction()
    //{
    //    StartCoroutine(GPauseAction());
    //}

    //IEnumerator GPauseAction()
    //{
    //    Inversion();
    //    Debug.Log("1clear");
    //    yield return new WaitForSeconds(10f);
    //    Debug.Log("2clear");
    //    Inversion();
    //    Debug.Log("3clear");
    //}
}
