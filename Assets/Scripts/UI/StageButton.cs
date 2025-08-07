using UnityEngine;

public class StageButton : MonoBehaviour
{
    public Episode episode;
    public int stageIndex;

    public void OnClick()
    {
        Debug.Log(episode);
        Debug.Log(stageIndex);
        StageManager.instance.StageEnter(episode, stageIndex);
    }
}
