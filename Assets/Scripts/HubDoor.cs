using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using static PlayerSelectableInterface;

public class HubDoor : MonoBehaviour
    //TODO: Door.cs와 상속 관련 refactor 고려

{
    enum PlayerColor { pink = 1, blue }
    [SerializeField] private PlayerColor color;
    [SerializeField] public Episode ep;
    [SerializeField] public int stage;
    //[SerializeField] private HubDoor[] blueDoors;
    [SerializeField] private HubDoor[] pinkDoors;
    
    private bool locked = false;
    private bool epSelected = false;
    private GrayScript gs;

    PlayerSelectableInterface playerSelectable = new PlayerSelectable();

    private float goalTime = 2.0f;
    private float enterTime, stayTime;

    private void Awake()
    {
       gs = GetComponent<GrayScript>();
       if(gs == null)
            Debug.LogWarning("GrayScript Not Found");
    }
    private void Start()
    {
        // 문이 핑크색이거나 파란색인데 아직 해금 안되었다면 잠근다
        if (color == PlayerColor.pink) lockDoor();
        else if (!DataManager.Instance.getIsUnlocked(ep, stage))
        {
            Debug.Log(ep.ToString() + " " + stage.ToString() + " blue door is locked");
            lockDoor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerSelectable.CheckColor(other, (int)color) == false) return;
        enterTime = Time.time;
        if( color == PlayerColor.blue )
        {
            for (int i = 0; i < 5; i ++)
            {
                if (pinkDoors.Length != 5) return;
                pinkDoors[i].ep = ep;
                pinkDoors[i].stage = i;
                if (DataManager.Instance.getIsUnlocked(ep, i)) 
                {
                    pinkDoors[i].unlockDoor();
                }
                else 
                    pinkDoors[i].lockDoor();
            }
                
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (locked) return; 
        if (color == PlayerColor.pink && !epSelected) return;
        if (playerSelectable.CheckColor(other, (int)color) == false) return;

        stayTime = Time.time - enterTime;
        if (stayTime >= goalTime)
        {
            GetComponent<Collider>().enabled = false;
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            other.GetComponent<Transparent>().CallFade();
            //StageManager.instance.CheckStageClear();

            if(color == PlayerColor.blue)
            {
                foreach ( HubDoor door in pinkDoors)
                {
                    door.epSelected = true;
                }
            }
            else if(color == PlayerColor.pink)
            {
                StageManager.instance.StageEnter(ep, stage);
            }
        }
    }

    private void lockDoor()
    {
        if (locked) return;
        locked = true;
        gs.turnGray();
        //Debug.Log(ep.ToString() + " " + stage.ToString() + " Locked");
    }
    private void unlockDoor()
    {
        if (!locked) return;
        locked = false;
        gs.turnDeGray();
        //Debug.Log(ep.ToString() + " " + stage.ToString() + " Unlocked");
    }
}
