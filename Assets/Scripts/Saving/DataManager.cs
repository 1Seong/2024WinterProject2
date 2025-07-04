using UnityEngine;
using System.IO;
using System;

public class DataManager : MonoBehaviour
{
    /////////////////singleton 패턴 구현////////////////
    static GameObject container;
    static DataManager instance;
    public static DataManager Instance
    {
        get
        {
            if (!instance)
            {
                container = new GameObject();
                container.name = "DataManager";
                instance = container.AddComponent(typeof(DataManager)) as DataManager;
                DontDestroyOnLoad(container);
            }
            return instance;
        }
    }
    //////////////////////////////////////////////////
    [SerializeField] private Data data;
    string GameDataFileName = "GameData.json";
    

    private void Awake()
    {
        LoadGameData();
    }


    public void LoadGameData()
    {
        // persistantDataPath Windows에서의 예시) C:\Users\[사용자 이름]\AppData\LocalLow\DefaultCompany\[프로젝트 이름]
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;
        // 저장된 게임이 있다면
        if (File.Exists(filePath))
        {
            // 저장된 파일 읽어오고 Json을 클래스 형식으로 전환해서 할당
            string FromJsonData = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<Data>(FromJsonData);
            Debug.Log("기존 Game Data 불러오기 완료");
        }
        // 없다면 새로 만들고 첫 스테이지를 unlock
        else
        {
            data = new Data();
            data.isUnlock[0] = true;
            SaveGameData();
            Debug.Log("새 Game Data 생성 완료");
        }
    }

    private void SaveGameData()
    {
        // 클래스를 Json 형식으로 전환 (true : 가독성 좋게 작성)
        string ToJsonData = JsonUtility.ToJson(data, true);
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;

        // 이미 저장된 파일이 있다면 덮어쓰고, 없다면 새로 만들어서 저장
        File.WriteAllText(filePath, ToJsonData);

        // 올바르게 저장됐는지 확인 (자유롭게 변형)
        print("저장 완료");
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
    }

    public void ChapterUnlock(Episode episode, int stageNum)
    {
        data.isUnlock[(int)episode * 5 + stageNum] = true;
        SaveGameData();
    }

    public bool getIsUnlocked(Episode episode, int stageNum)
    {
        //for (int i = 0; i < 25; i++)
        //        Debug.Log(data.isUnlock[i]);
        return data.isUnlock[(int)episode * 5 + stageNum];
    }
}