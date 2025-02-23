using Unity.VisualScripting;
using UnityEngine;
using static PlayerSelectableInterface;

public class Door : MonoBehaviour, PlayerSelectableInterface
{
    public PlayerColor Color { get; set; }

    [SerializeField] PlayerColor color;

    private float goalTime = 2.0f;
    private float enterTime, stayTime;
    public bool isComplete;

    private void Start()
    {
        Color = color;
        isComplete = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((PlayerSelectableInterface)this).CheckColor(other) == false) return;
        enterTime = Time.time;
    }

    private void OnTriggerStay(Collider other)
    {
        if (((PlayerSelectableInterface)this).CheckColor(other) == false) return;

        stayTime = Time.time - enterTime;
        if (stayTime >= goalTime)
        {
            isComplete = true;
            StageManager.instance.CheckStageClear();
        }
    }
}
