using DG.Tweening;
using System;
using UnityEngine;

public class HubDoor : MonoBehaviour
    //TODO: Door.cs와 상속 관련 refactor 고려
{
    public AudioSource openSound;
    enum PlayerColor { pink = 1, blue }
    [SerializeField] private PlayerColor color;
    [SerializeField] public Episode ep;
    [SerializeField] public int stage;
    //[SerializeField] private HubDoor[] blueDoors;
    [SerializeField] private HubDoor[] pinkDoors;
    [SerializeField] private GameObject lockObject;
    [SerializeField] private SpriteRenderer pinkBlockingSprite;
    [SerializeField] private SpriteRenderer blueBlockSprite;
    [SerializeField] private SpriteRenderer episodeSprite;
    [SerializeField] private SpriteRenderer stageSprite;
    
    private bool locked = false;
    private bool epSelected = false;
    private GrayScript gs;

    private Animator anim;

    public static event Action<Episode> OnEpSelected;

    PlayerSelectableInterface playerSelectable = new PlayerSelectable();

    private float goalTime = 2.0f;
    private float enterTime, stayTime;

    private void Awake()
    {
       anim = GetComponentInChildren<Animator>();
       gs = GetComponent<GrayScript>();
       if(gs == null)
            Debug.LogWarning("GrayScript Not Found");
        openSound = GetComponent<AudioSource>();
    }
    private void Start()
    {
        ApplyDevMode();

        OnEpSelected += hideBlueDoor;
    }

    private void OnDestroy()
    {
        OnEpSelected -= hideBlueDoor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerSelectable.CheckColor(other, (int)color) == false) return;
        if (color == PlayerColor.pink && !epSelected) return;
        enterTime = Time.time;
        if(!DataManager.Instance.getIsDevMode() && color == PlayerColor.blue ) // DevMode가 아닐때만 pink doors 확인
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
        if (!locked)
        {
            anim.SetBool("Open", true);
            openSound.time = 1.0f;
            openSound.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        anim.SetBool("Open", false);
        openSound.Stop();
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
                foreach ( var door in pinkDoors)
                {
                    door.epSelected = true;
                    door.showPinkDoor();
                    episodeSprite.DOFade(0f, 0.4f).OnComplete(() =>
                    {
                        stageSprite.DOFade(1f, 0.4f);
                    });
                    door.ep = this.ep;
                }

                OnEpSelected?.Invoke(ep);
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
        if (DataManager.Instance.getIsDevMode())
        {
            unlockDoor();
            return;
        }
        // 문이 핑크색이거나 파란색인데 아직 해금 안되었다면 잠근다
        if (color == PlayerColor.pink) lockDoor();
        else if (!DataManager.Instance.getIsUnlocked(ep, stage))
        {
            Debug.Log(ep.ToString() + " " + stage.ToString() + " locking blue door...");
            lockDoor();
        }
    }

    public void showPinkDoor()
    {
        pinkBlockingSprite.DOFade(0f, 0.4f);
    }

    private void hideBlueDoor(Episode ep)
    {
        if (ep == this.ep) return;

        blueBlockSprite.DOFade(0.8f, 0.4f);
    }
}
