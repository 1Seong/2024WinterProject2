using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using static PlayerSelectableInterface;

public class HubDoor : MonoBehaviour
    //TODO: Door.cs�� ��� ���� refactor ���

{
    enum PlayerColor { pink = 1, blue }
    [SerializeField] private PlayerColor color;
    [SerializeField] public Episode ep;
    [SerializeField] public int stage;
    //[SerializeField] private HubDoor[] blueDoors;
    [SerializeField] private HubDoor[] pinkDoors;
    [SerializeField] private GameObject lockObject;
    
    private bool locked = false;
    private bool epSelected = false;
    private GrayScript gs;

    private Animator anim;
    

    PlayerSelectableInterface playerSelectable = new PlayerSelectable();

    private float goalTime = 2.0f;
    private float enterTime, stayTime;

    private void Awake()
    {
       anim = GetComponentInChildren<Animator>();
       gs = GetComponent<GrayScript>();
       if(gs == null)
            Debug.LogWarning("GrayScript Not Found");
    }
    private void Start()
    {
        if (DataManager.Instance.getIsDevMode())
        {
            unlockDoor();
            return;
        }
        // ���� ��ũ���̰ų� �Ķ����ε� ���� �ر� �ȵǾ��ٸ� ��ٴ�
        if (color == PlayerColor.pink) lockDoor();
        else if (!DataManager.Instance.getIsUnlocked(ep, stage))
        {
            Debug.Log(ep.ToString() + " " + stage.ToString() + " locking blue door...");
            lockDoor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerSelectable.CheckColor(other, (int)color) == false) return;
        enterTime = Time.time;
        if(!DataManager.Instance.getIsDevMode() && color == PlayerColor.blue ) // DevMode�� �ƴҶ��� pink doors Ȯ��
            {
            for (int i = 0; i < 5; i++)
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
        if(!locked)
            anim.SetBool("Open", true);
    }

    private void OnTriggerExit(Collider other)
    {
        anim.SetBool("Open", false);
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
            other.GetComponent<PlayerMove>().enabled = false;
            other.GetComponent<Transparent>().CallFade();
            //other.gameObject.SetActive(false);
            //StageManager.instance.CheckStageClear();

            if(color == PlayerColor.blue)
            {
                foreach ( HubDoor door in pinkDoors)
                {
                    door.epSelected = true;
                    door.GetComponent<HubDoor>().ep = this.ep;
                }
            }
            else if(color == PlayerColor.pink)
            {
                Debug.Log("enter stage no: " + stage);
                StageManager.instance.StageEnter(ep, stage);
            }
        }
    }

    private void lockDoor()
    {
        if (locked) return;
        lockObject.SetActive(true);
        locked = true;
        gs.turnGray();
        Debug.Log(ep.ToString() + " " + stage.ToString() + " Locked");
    }
    private void unlockDoor()
    {
        if (!locked) return;
        lockObject.SetActive(false);
        locked = false;
        gs.turnDeGray();
        //Debug.Log(ep.ToString() + " " + stage.ToString() + " Unlocked");
    }

    public void ApplyDevMode()
    {
        Debug.Log("message recieved!");
        if (DataManager.Instance.getIsDevMode())
        {
            unlockDoor();
            return;
        }
        // ���� ��ũ���̰ų� �Ķ����ε� ���� �ر� �ȵǾ��ٸ� ��ٴ�
        if (color == PlayerColor.pink) lockDoor();
        else if (!DataManager.Instance.getIsUnlocked(ep, stage))
        {
            Debug.Log(ep.ToString() + " " + stage.ToString() + " locking blue door...");
            lockDoor();
        }
    }
}
