using System.Collections;
using UnityEngine;

public class CreditsDoor : MonoBehaviour
{
    public float duration = 2.0f;
    public AudioSource audioSrc;
    public AudioClip fadeClip;
    private Animator anim;
    private Material mat;


    PlayerSelectableInterface playerSelectable = new PlayerSelectable();

    private float goalTime = 2.0f;
    private float enterTime, stayTime;

    private void Awake()
    {
        mat = GetComponentInChildren<SpriteRenderer>().material;
        anim = GetComponentInChildren<Animator>();
        audioSrc = GetComponent<AudioSource>();
    }
    private void Start()
    {
        if (!DataManager.Instance.GetCreditUnlocked() && !DataManager.Instance.getIsDevMode())
        {
            Debug.Log("Credit Door Disabled");
            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Credit Door Abled");
            if (!DataManager.Instance.GetCreditDoorSoundPlayed())
                FadeIn();
            else
                mat.SetFloat("_LightAmount", 1.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        enterTime = Time.time;
        anim.SetBool("Open", true);
        audioSrc.time = 1.0f;
        audioSrc.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        anim.SetBool("Open", false);
        audioSrc.Stop();
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
            StageManager.instance.StageEnter(Episode.Credit, 0);
        }
    }

    public void FadeIn()
    {
        DataManager.Instance.CreditDoorSoundPlayed();
        StartCoroutine(FadeInRoutine());
    }

    private IEnumerator FadeInRoutine()
    {
        //GameManager.instance.Stop();

        audioSrc.PlayOneShot(fadeClip);
        float t = 0.0f;
        while (t < duration)
        {
            float fadeVal = t / duration;
            mat.SetFloat("_LightAmount", fadeVal);
            t += Time.unscaledDeltaTime;
            yield return null;
            //yield return new WaitForEndOfFrame();
        }
        mat.SetFloat("_LightAmount", 1.0f);

        //GameManager.instance.Resume();
        //Debug.Log("complete!");
    }
}