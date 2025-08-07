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
    public GameObject ClearPanel;
    public PhysicsMaterial physicsMat; // Physics material (No friction)

    public List<Door> doors;

    private Dictionary<Episode, StageData[]> _epStagePair;
    public GameObject canvas;

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

        //stageClearEvent
    }
    private void Start()
    {
        //init for Title Stage
        currentStageInfo.episode = Episode.Episode1;
        currentStageInfo.stageIndex = 0;
        
        stageClearEvent += ()=> UnlockNextStage();
    }

    public void StageEnter(Episode episode, int index)
    {
        currentStageInfo.episode = episode;
        currentStageInfo.stageIndex = index;
        currentStageInfo.data = _epStagePair[currentStageInfo.episode][currentStageInfo.stageIndex];

        if(GameManager.instance) GameManager.instance.isPlaying = true;
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
        Debug.Log("stage cleared!");
        int episode = (int)currentStageInfo.episode;
        int index = currentStageInfo.stageIndex;
        //���� ����
        GameManager.instance.isPlaying = false;
        //UIǥ��
        ClearPanel.SetActive(true);
        
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

    public void UnlockNextStage()
    {
        Episode nextEpisode = currentStageInfo.episode;
        int nextIndex = currentStageInfo.stageIndex;
        if (nextIndex == 4)
        {
            nextEpisode = nextEpisode + 1;
            nextIndex = 0;
        }
        else
        {
            nextIndex++;
        }
        DataManager.Instance.ChapterUnlock(nextEpisode, nextIndex);
    }

    public void EnterNextStage()
    {
        Episode nextEpisode = currentStageInfo.episode;
        int nextIndex = currentStageInfo.stageIndex;
        if(nextIndex == 4)
        {
            nextEpisode = nextEpisode + 1;
            nextIndex = 0;
        }
        else
        {
            nextIndex++;
        }
        
        Debug.Log("Loading " + nextEpisode.ToString()+ ("-") + nextIndex.ToString()+1 );

        StageEnter(nextEpisode, nextIndex);
    }

    public void Reset()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}