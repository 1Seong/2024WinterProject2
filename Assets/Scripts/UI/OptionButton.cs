using UnityEngine;
public class OptionButton : MonoBehaviour
{
    public GameObject optionPanel;
    public void OpenPanel()
    {
        optionPanel.SetActive(true);
        Time.timeScale = 0;
        //open up option panel
    }

    public void ClosePanel()
    {
        //transform.parent.gameObject.SetActive(false);
        optionPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
