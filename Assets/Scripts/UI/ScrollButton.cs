using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ScrollButton : MonoBehaviour
{
    public void ScrollButtonAction()
    {
        string sceneName = EventSystem.current.currentSelectedGameObject.name;
        sceneName.Split('-');
        //누른 에피소드가 해금 되었다면
        if (DataManager.Instance.data.isUnlock[episode , stage])
            //해당 씬 로드
            SceneManager.LoadScene(sceneName);
    }
}