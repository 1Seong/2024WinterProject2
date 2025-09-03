using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    static AudioManager instance;
    AudioSource bgm;
    public AudioMixer mixer;
    public AudioMixerGroup masterGroup;
    public AudioMixerGroup bgmGroup;
    public AudioMixerGroup sfxGroup;

    private bool isMasterMute = false;
    private bool isBgmMute = false;
    private bool isSfxMute = false;

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
    }

    private void Start()
    {
        bgm = GetComponent<AudioSource>();
        bgm.Play(); 
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

    public void ToggleMasterVolume()
    {
        
    }

}
