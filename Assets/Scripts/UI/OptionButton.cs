using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Legacy;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class OptionButton : MonoBehaviour
{
    public GameObject optionPanel;
    private bool isMasterMute;
    private bool isBgmMute;
    private bool isSfxMute;

    public Slider[] sliders;
    public GameObject[] icons;

    public GameObject toggleBar;

    private static float masterSliderValue = 1f;
    private static float bgmSliderValue = 1f;
    private static float sfxSliderValue = 1f;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        //Debug.Log("activated!");
        //MaintainSettings();
        masterSlider.value = masterSliderValue;
        bgmSlider.value = bgmSliderValue;
        sfxSlider.value = sfxSliderValue;

    }

    public void OpenPanel()
    {
        optionPanel.SetActive(true);
        GameManager.instance.Stop();
        //open up option panel
    }

    public void ClosePanel()
    {
        //transform.parent.gameObject.SetActive(false);
        optionPanel.SetActive(false);
        GameManager.instance.Resume();
    }

    public void Restart()
    {
        StageManager.instance.Reset();
        GameManager.instance.Resume();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if (GameManager.instance.isPlaying) OpenPanel();
            else ClosePanel();
        }
    }
    public void LoadHubStage()
    {
        ClosePanel();
        CircleTransition.Instance.LoadScene("HubStage");
        //SceneManager.LoadScene("HubStage");
    }

    public void NextStage()
    {
        StageManager.instance.EnterNextStage();
    }

    public void toggleDevMode()
    {
        toggleBar.GetComponent<Transform>().localScale *= -1;
        DataManager.Instance.changeIsDevMode();
        if (SceneManager.GetActiveScene().name == "HubStage")
        {
            Restart();
        }
    }

    public void MasterSliderChanged(float value)
    {
        //Debug.Log("Slider value " + value.ToString());
        masterSliderValue = value;
        AudioManager.instance.SetMasterVolume(value);
    }
    public void BgmSliderChanged(float value)
    {
        bgmSliderValue = value;
        AudioManager.instance.SetBgmVolume(value);
    }
    public void SfxSliderChanged(float value)
    {
        sfxSliderValue = value;
        AudioManager.instance.SetSfxVolume(value);
    }

    /*
    public void MaintainSettings()
    {
        for(int i = 0;i<3;i++)
        {
            Debug.Log(AudioManager.instance.volumes[i]);
            sliders[i].value = AudioManager.instance.volumes[i];
            icons[i].GetComponent<MuteButton>().MaintainIcon();
        } 
        
        if (DataManager.Instance.getIsDevMode())
                toggleBar.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        else
            toggleBar.GetComponent<Transform>().localScale = new Vector3(-1, -1, -1);
    }
    */
}
