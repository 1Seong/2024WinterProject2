using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# Game Control")]
    //public bool isPlaying;
    //public float gameTime;

    [Header("# Stage Info")]
    public int episodeNum;
    public int episodeId;
    public int stageId;

    [Header("# Player Info")]
    public float speed;

    [Header("# Game Objects")]
    public Player player1;
    public Player player2;
    public GameObject grid;

    private List<GameObject>[] stages;

    private void Awake()
    {
        instance = this;

        stages = new List<GameObject>[episodeNum];

        for(int i = 0; i != episodeNum; ++i)
        {
            stages[i] = new List<GameObject> ();
            
            //initialize stage
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
        //isPlaying = false;

        Time.timeScale = 0;
    }

    public void Resume()
    {
        //isPlaying = true;

        Time.timeScale = 1;
    }

    public void EnterStage()
    {
        stages[episodeId][stageId].SetActive(true);
        PlayerReposition();
        
    }

    public void StageClear()
    {
        ExitStage();
    }

    public void ExitStage()
    {
        stages[episodeId][stageId].SetActive(false);
    }

    public void Reset()
    {
        
    }

    private void PlayerReposition()
    {


        player1.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        player2.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
    }
}
