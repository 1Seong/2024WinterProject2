using System;
using UnityEngine;

public class StageButton : MonoBehaviour
{
    public Episode episode;
    public int stageIndex;
    
    public void OnClick()
    {
        StageManager.instance.StageEnter(episode, stageIndex);
    }
}
