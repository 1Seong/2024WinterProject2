using System;
using UnityEngine;

public class StageButton : MonoBehaviour
{
    private StageManager _stageManager;

    public Episode episode;
    public int stageIndex;


    private void Awake()
    {
        _stageManager = StageManager.instance;
    }

    public void OnClick()
    {
        _stageManager.StageEnter(episode, stageIndex);
    }
}
