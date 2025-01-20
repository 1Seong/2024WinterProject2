using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public struct CurrentStage
    {
        public Episode episode;
        public int stageIndex;
        public StageData data;
    }

    public static StageManager instance;

    [SerializeField] private StageData[] episode1;
    [SerializeField] private StageData[] episode2;
    [SerializeField] private StageData[] episode3;
    [SerializeField] private StageData[] episode4;
    [SerializeField] private StageData[] episode5;

    public CurrentStage currentStageInfo;

    private Dictionary<Episode, StageData[]> _epStagePair;

    private Action _stageEnterAction;
    public Action stageEnterAction
    {
        get => _stageEnterAction;
        set => _stageEnterAction = value;
    }

    private Action _stageExitAction;
    public Action stageExitAction
    {
        get => _stageExitAction;
        set => _stageExitAction = value;
    }

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);

        _epStagePair = new Dictionary<Episode, StageData[]>();
        _epStagePair.Add(Episode.Episode1, episode1);
        _epStagePair.Add(Episode.Episode2, episode2);
        _epStagePair.Add(Episode.Episode3, episode3);
        _epStagePair.Add(Episode.Episode4, episode4);
        _epStagePair.Add(Episode.Episode5, episode5);
    }

    public void StageEnter(Episode episode, int index)
    {
        currentStageInfo.episode = episode;
        currentStageInfo.stageIndex = index;
        currentStageInfo.data = _epStagePair[currentStageInfo.episode][currentStageInfo.stageIndex];

        stageEnterAction.Invoke();

        GameManager.instance.isPlaying = true;
        LoadStage();
    }

    public void StageExit()
    {
        stageExitAction.Invoke();

        GameManager.instance.isPlaying = false;
        LoadSelectStage();
    }

    private void LoadStage()
    {
        string sceneName = currentStageInfo.data.stageName;

        SceneManager.LoadScene(sceneName);
    }

    private void LoadSelectStage()
    {

    }

    public void Reset()
    {
        /*
         * Reset the game
         */
        LoadStage();
    }
}