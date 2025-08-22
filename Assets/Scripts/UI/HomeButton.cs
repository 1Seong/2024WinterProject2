using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButton : MonoBehaviour
{
   public void EnterHubStage()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("HubStage");
    }
}
