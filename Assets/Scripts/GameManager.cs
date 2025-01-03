using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# Game Control")]
    public bool isPlaying;
    //public float gameTime;

    [Header("# Stage Info")]
    public int episodeNum;
    public int episodeId;
    public int stageId;
    public Stage[][] stages;
    public Stage currentStage;

    [Header("# Player Info")]
    public float speed;

    [Header("# Game Objects")]
    public Player player1;
    public Player player2;
    public GameObject grid;


    private void Awake()
    {
        instance = this;

        Init();
    }

    private void Init()
    {
        int i = 0;
        episodeNum = grid.transform.childCount;

        Transform[] episodes = new Transform[episodeNum];

        //initialize episodes
        foreach(Transform child in grid.transform)
        {
            episodes[i++] = child;
        }

        //initialize stages
        stages = new Stage[episodeNum][];

        for (i = 0; i != episodeNum; ++i)
        {
            stages[i] = episodes[i].GetComponentsInChildren<Stage>(true);
        }
    }

    private void Update()
    {
        /*
         * if(!isPlaying)
         *  return;
         *  
         *  gameTime += Time.deltaTime;
         */
    }

    public void Stop()
    {
        isPlaying = false;

        Time.timeScale = 0;
    }

    public void Resume()
    {
        isPlaying = true;

        Time.timeScale = 1;
    }

    public void EnterStage(int episode, int stage)
    {
        episodeId = episode;
        stageId = stage;

        currentStage = stages[episodeId][stageId];
        currentStage.gameObject.SetActive(true);

        PlayerReposition();

        Resume();
    }

    public void StageClear()
    {
        isPlaying = false;

        //clearUI
    }

    public void ExitStage()
    {
        Stop();

        player1.gameObject.SetActive(true);
        player2.gameObject.SetActive(true);
        stages[episodeId][stageId].gameObject.SetActive(false);
    }

    public void Reset()
    {
        
    }

    private void PlayerReposition()
    {
        player1.gameObject.SetActive(true);
        player2.gameObject.SetActive(true);

        player1.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        player2.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
    }
}
