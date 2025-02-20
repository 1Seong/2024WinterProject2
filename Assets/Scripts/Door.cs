using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    enum DoorColor {Red, Blue};
    [SerializeField] DoorColor color;
    [SerializeField] Door otherDoor;
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
        if (other.tag == "Player")
            if ((other.gameObject.name == "Player1" && color == DoorColor.Red) || (other.gameObject.name == "Player2" && color == DoorColor.Blue))
                enterTime = Time.time;
    }

    private void OnTriggerStay(Collider other)
    {
        if ((other.gameObject.name == "Player1" && color == DoorColor.Red) || (other.gameObject.name == "Player2" && color == DoorColor.Blue))
        {
            stayTime = Time.time - enterTime;
            if (stayTime >= goalTime)
            {
                isComplete = true;
                if (otherDoor.isComplete)
                {
                    Debug.Log("Stage Clear!");
                    sm.StageClear();
                }
            }
        }
    }
    


 
}
