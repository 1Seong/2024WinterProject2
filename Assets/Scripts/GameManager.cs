using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# Game Control")]
    public bool isPlaying; // True when user is playing a stage
    public bool isSideView;
    public float cameraRotationTime;
    public bool gpauseActive = false;
    //public float gameTime;

    [Header("# Player Info")]
    public float speed;
    public float maxSpeed;

    [Header("# Game Objects")]
    public GameObject player1;
    public GameObject player2;


    private void Awake()
    {
        /*
         * Awake
         */

        instance = this;
        DontDestroyOnLoad(this);
        //Init();
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

    
}
