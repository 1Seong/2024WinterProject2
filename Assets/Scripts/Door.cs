using Unity.VisualScripting;
using UnityEngine;
using static PlayerSelectableInterface;

public class Door : MonoBehaviour
{
    enum PlayerColor { pink, blue }
    [SerializeField] private  PlayerColor color;

    PlayerSelectableInterface playerSelectable = new PlayerSelectable();

    private float goalTime = 2.0f;
    private float enterTime, stayTime;
    public bool isComplete;

    private void Start()
    {
        isComplete = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerSelectable.CheckColor(other, (int)color) == false) return;
        enterTime = Time.time;
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerSelectable.CheckColor(other, (int)color) == false) return;

        stayTime = Time.time - enterTime;
        if (stayTime >= goalTime)
        {
            isComplete = true;
            StageManager.instance.CheckStageClear();
        }
    }
}
