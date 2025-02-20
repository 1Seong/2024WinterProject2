using UnityEngine;

public class Door2 : MonoBehaviour
{
    //enum DoorColor { Red, Blue };
    //[SerializeField] DoorColor color;
    public Door1 door1;
    private float goalTime = 2.0f;
    private float enterTime, stayTime;
    public bool isComplete;
    StageManager sm;


    private void Start()
    {
        isComplete = false;
        sm = StageManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player2")
            enterTime = Time.time;
    }

    private void OnTriggerStay(Collider other)
    {
        //check if trigger object is Player1
        if (other.gameObject.name != "Player2") return;
        //check if player is on ground
        PlayerJump jump = other.GetComponent<PlayerJump>();
        if (jump.isJumping) return;

        stayTime = Time.time - enterTime;
        if (stayTime >= goalTime)
        {
            isComplete = true;
            if (door1 != null && door1.isComplete)
            {
                Debug.Log("Stage Clear!");
                sm.StageClear();
            }
        }

    }
}
