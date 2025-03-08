using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ScrollButton : MonoBehaviour
{
    public void ScrollButtonAction()
    {
        string sceneName = EventSystem.current.currentSelectedGameObject.name;
        SceneManager.LoadScene(sceneName);
    }
}
