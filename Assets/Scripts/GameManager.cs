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
    private int episodeNum; // The number of episodes
    public int episodeId; // Current episode index
    public int stageId; // Current stage index
    public Stage[][] stages; // Episode x Stages
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
        // 현재 에피소드, 스테이지 정보 저장
        episodeId = episode;
        stageId = stage;
        
        // 현재 스테이지 활성화
        currentStage = stages[episodeId][stageId];
        currentStage.gameObject.SetActive(true);
        
        // 플레이어 초기화
        PlayerReposition();

        // 플레이어 상태 초기화(아이템)
        player1.ResetPlayer();
        if (currentStage.player2Exist)
            player2.ResetPlayer();

        // Do some camera setting
        isTopView = true; // 기본 TopView로 설정
        player1.ConversionPhysicsSetting();
        if (currentStage.player2Exist)
            player2.ConversionPhysicsSetting();
        
        // 게임 진행 상태 활성화
        Resume();
        Debug.Log($"Stage {stageId} of Episode {episodeId} has started.");
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

        // 현재 스테이지 클리어 처리
        Debug.Log($"Stage {stageId} of Episode {episodeId} is cleared!");
        
        // 다음 스테이지 확인
        bool isLastStage = stageId == stages[episodeId].Length - 1;
        if (isLastStage)
        {
            Debug.Log("All stages in this episode completed!");
            // 에피소드 종료 처리
            ExitStage(); // 스테이지 종료 및 정리
            // 여기에서 전체 에피소드 완료 UI를 표시하거나 다음 에피소드 로드를 처리할 수 있음
        }
        else
        {
            // 다음 스테이지로 이동
            EnterStage(episodeId, stageId + 1);
        }
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

        // 현재 스테이지 비활성화
        if (currentStage != null)
        {
            currentStage.gameObject.SetActive(false);
        }

        // 플레이어 상태 초기화
        player1.gameObject.SetActive(false);
        if (currentStage.player2Exist)
        {
            player2.gameObject.SetActive(false);
        }

        // 플레이어 상태 초기화(아이템)
        player1.ResetPlayer();
        if (player2 != null)
            player2.ResetPlayer();

        // 게임 상태 초기화
        Debug.Log("Exited the stage.");
    }

    public void Reset()
    {
        /*
         * Reset the game
         * 
         * Please complete this method
         * You can modify original code
         */
        Debug.Log("Game Reset!");
        Stop();

        // 전체 초기화 로직 (필요시 추가)
        foreach (var episode in stages)
        {
            foreach (var stage in episode)
            {
                stage.gameObject.SetActive(false); // 모든 스테이지 비활성화
            }
        }

        // 플레이어 상태 초기화(아이템)
        player1.ResetPlayer();
        if (player2 != null)
            player2.ResetPlayer();

        // 첫 번째 에피소드와 스테이지로 이동
        EnterStage(0, 0);
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
        player1.transform.position = new Vector3(currentStage.startPosX1, 0, currentStage.startPosZ1);
        player1.inverted = false; // 초기 중력 상태
        player1.isJumping = false;
        player1.onBottom = true;

        if (currentStage.player2Exist)
        {
            player2.gameObject.SetActive(true);
            player2.transform.position = new Vector3(currentStage.startPosX2, 0, currentStage.startPosZ2);
            player2.inverted = false;
            player2.isJumping = false;
            player2.onBottom = true;
        }
        else
        {
            player2.gameObject.SetActive(false);
        }
    }
}
