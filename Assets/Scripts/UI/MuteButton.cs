using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    public enum muteType{ mute1, mute2, mute3 };
    public muteType mute;
    public AudioManager.EMixerType mixerType;
    static public bool isMute1 = false;
    static public bool isMute2 = false;
    public static bool isMute3 = false;
    public Sprite muteIcon, unmuteIcon;

    private void Start()
    {
        if (mute == muteType.mute1 && isMute1 || mute == muteType.mute2 && isMute2 || mute == muteType.mute3 && isMute3)
            GetComponent<Image>().sprite = muteIcon;
        else
            GetComponent<Image>().sprite = unmuteIcon;
    }

    public void MuteButtonAction()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        //Debug.Log("prev: " + isMute.ToString());
        switch (mute)
        {
            case muteType.mute1:
                isMute1 = !isMute1;
                break;
            case muteType.mute2:
                isMute2 = !isMute2;
                break;
            case muteType.mute3:
                isMute3 = !isMute3;
                break;
        }

        if (mute == muteType.mute1 && isMute1 || mute == muteType.mute2 && isMute2 || mute == muteType.mute3 && isMute3)
            GetComponent<Image>().sprite = muteIcon;
        else
            GetComponent<Image>().sprite = unmuteIcon;

        AudioManager.instance.ToggleMute(mixerType);
    }

    /*
    public void MaintainIcon()
    {
        if(isMute) { GetComponent<Image>().sprite = muteIcon; }
        else { GetComponent<Image>().sprite = unmuteIcon; }
    }
    */
}
