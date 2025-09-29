using UnityEngine;
using UnityEngine.SceneManagement;

public class CrewsButton : MonoBehaviour
{
    public void LoadCrewsScene()
    {
        CircleTransition.Instance.LoadScene("Crews");
        //SceneManager.LoadScene("Crews");
    }
}
