using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsButton : MonoBehaviour
{
    public void LoadCredits()
    {
        CircleTransition.Instance.LoadScene("Credits");
    }
    public void LoadHubStage()
    {
        CircleTransition.Instance.LoadScene("HubStage");
    }
}
