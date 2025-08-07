using UnityEngine;
using System.IO;
using System;

public class DataManager : MonoBehaviour
{
    /////////////////singleton ���� ����////////////////
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
        // persistantDataPath Windows������ ����) C:\Users\[����� �̸�]\AppData\LocalLow\DefaultCompany\[������Ʈ �̸�]
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;
        // ����� ������ �ִٸ�
        if (File.Exists(filePath))
        {
            // ����� ���� �о���� Json�� Ŭ���� �������� ��ȯ�ؼ� �Ҵ�
            string FromJsonData = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<Data>(FromJsonData);
            Debug.Log("���� Game Data �ҷ����� �Ϸ�");
        }
        // ���ٸ� ���� ����� ù ���������� unlock
        else
        {
            data = new Data();
            data.isUnlock[0] = true;
            SaveGameData();
            Debug.Log("�� Game Data ���� �Ϸ�");
        }
    }

    private void SaveGameData()
    {
        // Ŭ������ Json �������� ��ȯ (true : ������ ���� �ۼ�)
        string ToJsonData = JsonUtility.ToJson(data, true);
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;

        // �̹� ����� ������ �ִٸ� �����, ���ٸ� ���� ���� ����
        File.WriteAllText(filePath, ToJsonData);

        // �ùٸ��� ����ƴ��� Ȯ�� (�����Ӱ� ����)
        print("���� �Ϸ�");
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