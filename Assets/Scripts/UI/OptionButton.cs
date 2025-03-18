using UnityEngine;
public class OptionButton : MonoBehaviour
{
    public GameObject optionPanel;
    GameManager GM = GameManager.instance;
    public void OpenPanel()
    {
        optionPanel.SetActive(true);
        GM.Stop();
        //open up option panel
    }

    public void ClosePanel()
    {
        //transform.parent.gameObject.SetActive(false);
        optionPanel.SetActive(false);
        GM.Resume();
    }

    public void Restart()
    {
        //restart stage
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if (GM.isPlaying) OpenPanel();
            else ClosePanel();
        }
    }
}
