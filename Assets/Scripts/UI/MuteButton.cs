using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    public AudioManager.EMixerType mixerType;
    public bool isMute = false;
    public Sprite muteIcon, unmuteIcon;
    public void MuteButtonAction()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        Debug.Log("prev: " + isMute.ToString());
        if (isMute)
            GetComponent<Image>().sprite = unmuteIcon;
        else
            GetComponent<Image>().sprite = muteIcon;
        isMute = !isMute;
        AudioManager.instance.ToggleMute(mixerType);
    }

    public void MaintainIcon()
    {
        if(isMute) { GetComponent<Image>().sprite = muteIcon; }
        else { GetComponent<Image>().sprite = unmuteIcon; }
    }
}
