using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    public enum muteType{ mute1, mute2, mute3 };
    public muteType mute;
    public AudioManager.EMixerType mixerType;
    public Slider[] TargetSliders;
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

        if (mute == muteType.mute1 && isMute1 || mute == muteType.mute2 && isMute2 || mute == muteType.mute3 && isMute3) // MUTE
        {
            GetComponent<Image>().sprite = muteIcon;
            foreach(var i in TargetSliders)
            {
                i.interactable = false;
            }
        }
        else // UN-MUTE
        {
            GetComponent<Image>().sprite = unmuteIcon;

            if(mute == muteType.mute1)
            {
                TargetSliders[0].interactable = true;

                if(!isMute2)
                    TargetSliders[1].interactable = true;

                if (!isMute3)
                    TargetSliders[2].interactable = true;
            }
            else
            {
                if(!isMute1)
                    TargetSliders[0].interactable = true;
            }
        }

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
