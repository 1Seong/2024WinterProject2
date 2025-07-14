using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private static StageManager _instance;

    public static StageManager instance
    {
        get
        {
            if(_instance == null )
            {
                _instance  = FindFirstObjectByType<StageManager>();

                if(_instance == null)
                {
                    GameObject obj = new GameObject("StageManager");
                    _instance = obj.AddComponent<StageManager>();
                    
                    DontDestroyOnLoad(obj);
                }
            }

            return _instance;
        }
    }

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
        if(_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

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
        InitData(episode, index);

        stageEnterEvent?.Invoke();

        GameManager.instance.isPlaying = true;
        LoadStage();
    }

    private void InitData(Episode episode, int index)
    {
        currentStageInfo.episode = episode;
        currentStageInfo.stageIndex = index;
        currentStageInfo.data = _epStagePair[currentStageInfo.episode][currentStageInfo.stageIndex];
    }

    private static void FindAndInitData()
    {
        var name = SceneManager.GetActiveScene().name;

        if (name.StartsWith("Stage"))
        {
            var episodeStagePair = name.Split("-");
            episodeStagePair[0] = episodeStagePair[0].Substring(5);

            var episodeInt = int.Parse(episodeStagePair[0]);
            var episodeEnum = (Episode)episodeInt;

            var stageInt = int.Parse(episodeStagePair[1]);

            InitData(episodeEnum, stageInt);

            stageEnterEvent?.Invoke();

            GameManager.instance.isPlaying = true;
        }
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