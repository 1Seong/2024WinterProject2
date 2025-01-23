using UnityEngine;

public enum Episode { Episode1, Episode2, Episode3, Episode4, Episode5 };

[CreateAssetMenu(fileName = "StageData", menuName = "Scriptable Objects/StageData")]
public class StageData : ScriptableObject
{
    [Header("StageInfo")]

    public Episode episodeNum;
    public int stageIndex;
    public string stageName;

    public bool topview;
    public bool conversionActive;

    public static GameObject wallPrefab;
    public static PhysicsMaterial physicsMat; // Physics material (No friction)

    [Header("PlayerInfo")]

    public Vector3 startPos1;

    public bool player2Exist; // Does player 2 exist in this stage?

    public Vector3 startPos2;
}
