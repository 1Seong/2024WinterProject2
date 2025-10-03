using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public event Action GPauseOffEvent;

    [Header("# Game Control")]
    public bool isPlaying; // True when user is playing a stage
    public bool isSideView;
    public float cameraRotationTime;

    [SerializeField]private bool _gpauseActive = false;
    public bool GpauseActive
    {
        get => _gpauseActive;
        set
        {
            _gpauseActive = value;
            if (!value)
                GPauseOffEvent?.Invoke();
        }
    }
    //public float gameTime;

    [Header("# Player Info")]
    public float speed;
    public float maxSpeed;

    [Header("# Game Objects")]
    public GameObject player1;
    public GameObject player2;

    [Header("# Debug Options")]
    public bool dynamicInnerWallInstantiation = false;


    private void Awake()
    {
        /*
         * Awake
         */

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //Init();
    }

    private void Init()
    {
        DataManager.Instance.LoadGameData();

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
