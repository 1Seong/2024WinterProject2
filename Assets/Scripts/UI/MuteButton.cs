using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    public AudioManager.EMixerType mixerType;
    public bool isMute = false;
    public Sprite muteIcon, unmuteIcon;
    public void MuteButtonAction()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        isMute = !isMute;
        if (isMute)
            GetComponent<Image>().sprite = unmuteIcon;
        else
            GetComponent<Image>().sprite = muteIcon;
        AudioManager.instance.ToggleMute(mixerType);
    }
}
