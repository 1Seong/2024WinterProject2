using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ScrollButton : MonoBehaviour
{
    public void ScrollButtonAction()
    {
        string sceneName = EventSystem.current.currentSelectedGameObject.name;
        sceneName.Split('-');
        //���� ���Ǽҵ尡 �ر� �Ǿ��ٸ�
        if (DataManager.Instance.data.isUnlock[episode , stage])
            //�ش� �� �ε�
            SceneManager.LoadScene(sceneName);
    }
}