using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ScrollButton : MonoBehaviour
{
    public void ScrollButtonAction()
    {
        string sceneName = EventSystem.current.currentSelectedGameObject.name;
        
        //���� Scene�� 'Episode Board'�� Stage�� ���� 'Episode(����)' Scene���� �Ѿ
        if (SceneManager.GetActiveScene().name == "EpisodeBoard")
        {
            string ep = sceneName.Replace("Episode", "");
            Episode episode = (Episode)int.Parse(ep) - 1;
            if (DataManager.Instance.getIsUnlocked(episode, 0))
            {
                CircleTransition.Instance.LoadScene(sceneName);
                //SceneManager.LoadScene(sceneName);
            }
            else Debug.Log("Locked Episode");
        }

        //�ƴ϶�� ���� 'Episode(����)' Scene�̹Ƿ� ���õ� Stage�� ������.
        else
        {
            //Ŭ���� Scroll View Content�� ������Ʈ �̸��� �������� �����ؾ� �� Scene�� Episode, stage ������ parse
            string[] split_data = sceneName.Split('-');
            Episode episode = (Episode)int.Parse(split_data[0]) - 1;
            int stage = int.Parse(split_data[1]) - 1;

            //�ش� ���������� �ر� �Ǿ��ٸ� �ش� �� �ε�
            if (DataManager.Instance.getIsUnlocked(episode, stage))
            {
                Debug.Log("Enter " + episode.ToString() + "-" + stage.ToString());
                StageManager.instance.StageEnter(episode, stage);
            }
            else Debug.Log("Locked stage");
        }
        
    }


}