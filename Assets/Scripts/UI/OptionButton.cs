using Unity.VisualScripting;
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
        GameObject toggleBar = EventSystem.current.currentSelectedGameObject;
        toggleBar.GetComponent<Transform>().localScale *= -1;
        DataManager.Instance.changeIsDevMode();
        BroadcastMessage("ApplyDevMode", null, SendMessageOptions.DontRequireReceiver);
    }

    public void MasterSlider(float value)
    {
        //Debug.Log("Slider value " + value.ToString());
        AudioManager.instance.SetMasterVolume(value);
    }
    public void BgmSlider(float value)
    {
        AudioManager.instance.SetBgmVolume(value);
    }
    public void SfxSlider(float value)
    {
        AudioManager.instance.SetSfxVolume(value);
    }
}
