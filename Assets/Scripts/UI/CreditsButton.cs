using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsButton : MonoBehaviour
{
    public void LoadHubStage()
    {
        CircleTransition.Instance.LoadScene("HubStage");
        //SceneManager.LoadScene("HubStage");
    }

}
