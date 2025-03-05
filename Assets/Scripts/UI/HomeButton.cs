using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButton : MonoBehaviour
{
    public void GoEpisodeBoard()
    {
        SceneManager.LoadScene("EpisodeBoard");
    }
}
