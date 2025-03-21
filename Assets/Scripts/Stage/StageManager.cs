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
    public Stage stage;
    public GameObject wallPrefab;
    public PhysicsMaterial physicsMat; // Physics material (No friction)

    public List<Door> doors;

    private Dictionary<Episode, StageData[]> _epStagePair;

    public event Action stageEnterEvent;
    public event Action stageClearEvent;
    public event Action stageExitEvent;
    
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);

        currentStageInfo = new CurrentStage();

        _epStagePair = new Dictionary<Episode, StageData[]>
        {
            { Episode.Episode1, episode1 },
            { Episode.Episode2, episode2 },
            { Episode.Episode3, episode3 },
            { Episode.Episode4, episode4 },
            { Episode.Episode5, episode5 }
        };

        //debug code
        stageClearEvent += () => LoadStage("SeongWon0");
    }

    public void StageEnter(Episode episode, int index)
    {
        currentStageInfo.episode = episode;
        currentStageInfo.stageIndex = index;
        currentStageInfo.data = _epStagePair[currentStageInfo.episode][currentStageInfo.stageIndex];

        stageEnterEvent?.Invoke();

        GameManager.instance.isPlaying = true;
        LoadStage();
    }

    public void CheckStageClear()
    {
        foreach(var door in doors)
        {
            if (!door.isComplete) return;
        }
        StageClear();
    }

    public void StageClear()
    {
        GameManager.instance.isPlaying = false;

        stageClearEvent?.Invoke();
    }

    public void StageExit()
    {
        if(GameManager.instance.isPlaying)
            GameManager.instance.isPlaying = false;

        doors.Clear();
        stageExitEvent?.Invoke();

        LoadSelectStage();
    }

    private void LoadStage()
    {
        string sceneName = currentStageInfo.data.stageName;

        SceneManager.LoadScene(sceneName);
    }

    private void LoadStage(string sceneName)
    {
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