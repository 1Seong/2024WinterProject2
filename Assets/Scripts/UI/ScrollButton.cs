using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ScrollButton : MonoBehaviour
{
    public void ScrollButtonAction()
    {
        string sceneName = EventSystem.current.currentSelectedGameObject.name;
        
        //현재 Scene이 'Episode Board'면 Stage를 고르는 'Episode(숫자)' Scene으로 넘어감
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

        //아니라면 현재 'Episode(숫자)' Scene이므로 선택된 Stage로 진입함.
        else
        {
            //클릭된 Scroll View Content의 오브젝트 이름을 바탕으로 진입해야 할 Scene의 Episode, stage 정보를 parse
            string[] split_data = sceneName.Split('-');
            Episode episode = (Episode)int.Parse(split_data[0]) - 1;
            int stage = int.Parse(split_data[1]) - 1;

            //해당 스테이지가 해금 되었다면 해당 씬 로드
            if (DataManager.Instance.getIsUnlocked(episode, stage))
            {
                Debug.Log("Enter " + episode.ToString() + "-" + stage.ToString());
                StageManager.instance.StageEnter(episode, stage);
            }
            else Debug.Log("Locked stage");
        }
        
    }


}