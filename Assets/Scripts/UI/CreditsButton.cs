using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void EnterCreditsScene()
    {
        CircleTransition.Instance.LoadScene("Credits");
    }
}
