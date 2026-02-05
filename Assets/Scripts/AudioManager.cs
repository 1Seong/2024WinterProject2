using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public enum EMixerType { MasterVolume, BgmVolume, SfxVolume }
    public static AudioManager instance;
    public AudioMixer mixer;
    
    public bool[] isMute = new bool[3]; // 0:Master, 1:BGM, 2:SFX
    public float[] volumes = new float[3];

    [SerializeField] private AudioClip[] itemClips;

    public enum Item { battery, swap};
    AudioSource footstep;
    AudioSource itemAudio;
    [SerializeField] AudioSource creditBGMAudio;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        footstep = transform.GetChild(0).GetComponent<AudioSource>();
        itemAudio = transform.GetChild(1).GetComponent<AudioSource>();
    }

    private void Start()
    {
        //bgm = GetComponent<AudioSource>();
        //bgm.Play(); 
        
    }
    public void SetMasterVolume(float value)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }

    public void SetBgmVolume(float value)
    {
        mixer.SetFloat("BgmVolume", Mathf.Log10(value) * 20);
    }

    public void SetSfxVolume(float value)
    {
        mixer.SetFloat("SfxVolume", Mathf.Log10(value) * 20);
    }

    public void ToggleMute(EMixerType mixerType)
    {
        int n = (int)mixerType;
        if (!isMute[n])
        {
            isMute[n] = true;
            mixer.GetFloat(mixerType.ToString(), out float volume);
            volumes[n] = volume;
            mixer.SetFloat(mixerType.ToString(), -80.0f);
        }
        else
        {
            isMute[n] = false;
            mixer.SetFloat(mixerType.ToString(), volumes[n]);
        }
    }

    public void PlayFootstep()
    {
        if (!footstep.isPlaying)
        {
            footstep.Play();
        }
    }
    public void StopFootstep()
    {
        footstep.Stop();
    }
    public void PlayItemClip(Item item)
    {
        itemAudio.PlayOneShot(itemClips[(int)item]);
    }

    public void PlayCreditBGM()
    {
        creditBGMAudio.Play();
    }

    public void StopCreditBGM()
    {
        creditBGMAudio.Stop();
    }
}
