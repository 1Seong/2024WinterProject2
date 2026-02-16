using DG.Tweening;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class OptionButton : MonoBehaviour
{
    public GameObject optionPanel;

    public Slider[] sliders;
    public GameObject[] icons;

    public GameObject toggleBar;
    public RectTransform toggleLanHandle;
    private bool toggleIsActing = false;

    static bool isEng = true;

    static float masterValue = 1f;
    static float bgmValue = 1f;
    static float sfxValue = 1f;

    private void Start()
    {
        //Debug.Log("activated!");
        //MaintainSettings();
        if(DataManager.Instance.getIsDevMode())
            toggleBar.GetComponent<Transform>().localScale *= -1;

        if (!isEng)
            toggleLanHandle.anchoredPosition = new Vector2(19f, 0f);

        sliders[0].SetValueWithoutNotify(masterValue);
        sliders[1].SetValueWithoutNotify(bgmValue);
        sliders[2].SetValueWithoutNotify(sfxValue);
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
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.instance.isPlaying) OpenPanel();
            else ClosePanel();
        }
        else if (Input.GetKeyDown(KeyCode.R)) Restart();
    }
    public void LoadHubStage()
    {
        AudioManager.instance.StopCreditBGM();
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

    public void toggleLanguage()
    {
        if (toggleIsActing) return;

        toggleIsActing = true;
        isEng = !isEng;

        var idx = isEng ? 0 : 1;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[idx];

        var targetPos = -toggleLanHandle.anchoredPosition.x;
        toggleLanHandle.DOAnchorPosX(targetPos, 0.3f).SetUpdate(true).OnComplete(() => { toggleIsActing = false; });
    }

    public void MasterSliderChanged(float value)
    {
        //Debug.Log("Slider value " + value.ToString());
        masterValue = value;
        AudioManager.instance.SetMasterVolume(value);
    }
    public void BgmSliderChanged(float value)
    {
        bgmValue = value;
        AudioManager.instance.SetBgmVolume(value);
    }
    public void SfxSliderChanged(float value)
    {
        sfxValue = value;
        AudioManager.instance.SetSfxVolume(value);
    }

    //public void MaintainSettings()
    //{
    //    for(int i = 0;i<3;i++)
    //    {
    //        Debug.Log(AudioManager.instance.volumes[i]);
    //        sliders[i].value = AudioManager.instance.volumes[i];
    //        icons[i].GetComponent<MuteButton>().MaintainIcon();
    //    } 
        
    //    if (DataManager.Instance.getIsDevMode())
    //            toggleBar.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
    //    else
    //        toggleBar.GetComponent<Transform>().localScale = new Vector3(-1, -1, -1);
    //}
}
