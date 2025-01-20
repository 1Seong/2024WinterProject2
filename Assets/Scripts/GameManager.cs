using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# Game Control")]
    public bool isPlaying; // True when user is playing a stage
    public bool isTopView;
    public float cameraRotationTime;
    public float cameraRotationR; // Camera rotation radius
    //public float gameTime;

    [Header("# Stage Info")]
    public Stage currentStage;

    [Header("# Player Info")]
    public float speed;
    public float maxSpeed;

    [Header("# Game Objects")]
    public Player player1;
    public Player player2;
    public GameObject grid;


    private void Awake()
    {
        /*
         * Awake
         */

        instance = this;

        Init();
    }

    private void Init()
    {
        /*
         *  Additional initialization
         */
        
       

        

        //initialize stages
        
    }

    private void Update()
    {
        /*
         * Update
         */

        /*
         * if(!isPlaying)
         *  return;
         *  
         *  gameTime += Time.deltaTime; // Update game time
         */
    }

    public void Stop()
    {
        /*
         * Stop or pause the game
         */
        isPlaying = false;

        Time.timeScale = 0;
    }

    public void Resume()
    {
        /*
         * Resume the game
         */
        isPlaying = true;

        Time.timeScale = 1;
    }

    public void EnterStage(int episode, int stage)
    {
        /*
         * Should be called when user enter a stage
         * 
         * Please complete this method
         * You can modify original code
         */
       

       

        PlayerReposition();

        // Do some camera setting

        Resume();
    }

    public void StageClear()
    {
        /*
         * Should be called when user clears a stage
         * 
         * Please complete this method
         * You can modify original code
         */
        isPlaying = false;

        //gameClearUI
    }

    public void ExitStage()
    {
        /*
         * Should be called when user exits a stage
         * 
         * Please complete this method
         * You can modify original code
         */
        Stop();

        player1.gameObject.SetActive(true);
        player2.gameObject.SetActive(true);
        
    }

    private void PlayerReposition()
    {
        /*
         * Reposition players
         * 
         * Please Complete this method
         * You can modify original code
         */
        player1.gameObject.SetActive(true);
        player2.gameObject.SetActive(true);

        player1.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        player2.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
    }
}
