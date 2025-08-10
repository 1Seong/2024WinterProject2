using UnityEngine;
using UnityEngine.SceneManagement;
public class OptionButton : MonoBehaviour
{
    public GameObject optionPanel;
    public void OpenPanel()
    {
        optionPanel.SetActive(true);
        GameManager.instance.Stop();
        //open up option panel
    }

    public void ClosePanel()
    {
        //transform.parent.gameObject.SetActive(false);
        optionPanel.SetActive(false);
        GameManager.instance.Resume();
    }

    public void Restart()
    {
        StageManager.instance.Reset();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if (GameManager.instance.isPlaying) OpenPanel();
            else ClosePanel();
        }
    }
    public void LoadHubStage()
    {
        ClosePanel();
        SceneManager.LoadScene("HubStage");
    }

    public void NextStage()
    {
        StageManager.instance.EnterNextStage();
    }
}
