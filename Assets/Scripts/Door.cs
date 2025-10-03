using System;
using Unity.VisualScripting;
using UnityEngine;
using static PlayerSelectableInterface;

public class Door : MonoBehaviour
{
    //TODO: Door.cs와 상속 관련 refactor 고려
    public AudioSource openSound;
    enum PlayerColor { pink = 1, blue }
    [SerializeField] private PlayerColor color;

    PlayerSelectableInterface playerSelectable = new PlayerSelectable();

    private float goalTime = 1.5f;
    private float enterTime, stayTime;
    public bool isComplete;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        openSound = GetComponent<AudioSource>();
    }

    private void Start()
    {
        isComplete = false;
        StageManager.instance.doors.Add(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerSelectable.CheckColor(other, (int)color) == false) return;

        anim.SetBool("Open", true);
        enterTime = Time.time;

        openSound.time = 1.0f;
        openSound.Play();
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerSelectable.CheckColor(other, (int)color) == false) return;

        stayTime = Time.time - enterTime;
        if (stayTime >= goalTime)
        {
            isComplete = true;
            GetComponent<Collider>().enabled = false;
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            other.GetComponent<PlayerMove>().enabled = false;
            other.GetComponent<Transparent>().CallFade();
            Invoke("CheckStageClear", 1f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (playerSelectable.CheckColor(other, (int)color) == false) return;

        anim.SetBool("Open", false);
        openSound.Stop();
    }

    private void CheckStageClear()
    {
        StageManager.instance.CheckStageClear();
    }
}
