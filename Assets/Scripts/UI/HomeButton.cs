using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButton : MonoBehaviour
{
    public void LoadEpisodeBoard()
    {
        SceneManager.LoadScene("EpisodeBoard");
    }
}
