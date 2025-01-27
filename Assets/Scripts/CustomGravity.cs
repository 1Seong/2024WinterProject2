using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    private Vector3 customGravity;

    Player player;
    Rigidbody rigid;

    void Awake()
    {
        /*
         * Awake
         */
        player = GetComponent<Player>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        /*
         * Start
         */
        customGravity = GameManager.instance.isTopView ? (!player.inverted ? Physics.gravity : -Physics.gravity) : Physics.gravity;
    }

    void FixedUpdate()
    {
        /*
         * Fixed Update
         */
        if (!GameManager.instance.isPlaying || player.onInnerWall)
        {
            return;
        }

        rigid.AddForce(customGravity, ForceMode.Acceleration);
    }

    public void InvertGravity()
    {
        /*
         * Invert Gravity
         */
        customGravity = -Physics.gravity;
    }

    public void ReapplyGravity()
    {
<<<<<<< Updated upstream
        /*
         * Reinitialize customGravity
         */
        customGravity = Physics.gravity;
=======
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
        //GPause 활성화될때 호출
        if (gravityState == GravityState.defaultG)
            gPauseAtDefaultEvent?.Invoke();
        else if (gravityState == GravityState.invertG)
            gPauseAtInvertEvent?.Invoke();
    }

    private void CallGPauseAction()
    {
        StartCoroutine(GPauseAction());
    }

    IEnumerator GPauseAction()
    {
        InvertAction();
        yield return new WaitForSeconds(10f);
        InvertAction();
>>>>>>> Stashed changes
    }
}
