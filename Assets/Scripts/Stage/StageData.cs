using UnityEngine;

public enum Episode { Episode1, Episode2, Episode3, Episode4, Episode5 };

[CreateAssetMenu(fileName = "StageData", menuName = "Scriptable Objects/StageData")]
public class StageData : ScriptableObject
{
    [Header("StageInfo")]

    public Episode episodeNum;

    [TextArea] public string stageName;

    public GameObject wallPrefab;
    public PhysicsMaterial physicsMat; // Physics material (No friction)

    [Header("PlayerInfo")]

    public Vector2 startPos1;

    public bool player2Exist; // Does player 2 exist in this stage?

    public Vector2 startPos2;
}