using UnityEngine;

public class CreditsDoor : MonoBehaviour
{
    public AudioSource openSound;
    private Animator anim;


    PlayerSelectableInterface playerSelectable = new PlayerSelectable();

    private float goalTime = 2.0f;
    private float enterTime, stayTime;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        openSound = GetComponent<AudioSource>();
    }
    private void Start()
    {
        if (!DataManager.Instance.getIsUnlocked(Episode.Episode5, 4) && !DataManager.Instance.getIsDevMode())
        {
            Debug.Log("Credit Door Disabled");
            this.gameObject.SetActive(false);
        }
        else
            Debug.Log("Credit Door Abled");
    }

    private void OnTriggerEnter(Collider other)
    {
        enterTime = Time.time;
        anim.SetBool("Open", true);
        openSound.time = 1.0f;
        openSound.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        anim.SetBool("Open", false);
        openSound.Stop();
    }

    private void OnTriggerStay(Collider other)
    {
        stayTime = Time.time - enterTime;
        if (stayTime >= goalTime)
        {
            GetComponent<Collider>().enabled = false;
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            other.GetComponent<PlayerMove>().enabled = false;
            other.GetComponent<Transparent>().CallFade();
            //other.gameObject.SetActive(false);
            //StageManager.instance.CheckStageClear();

            Debug.Log("enter Credits");
            CircleTransition.Instance.LoadScene("Credits");
        }
    }
}