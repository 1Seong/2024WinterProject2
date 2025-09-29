using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class CircleTransition : MonoBehaviour
{
    public static CircleTransition Instance;
    public Image circleImage;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void LoadScene(string sceneName)
    {
        circleImage.transform.localScale = Vector3.zero;
        circleImage.transform.DOScale(30f, 1f).OnComplete(() =>
        {
            SceneManager.LoadScene(sceneName);
            circleImage.transform.localScale = Vector3.one * 30f;
            circleImage.transform.DOScale(0f, 1f);
        });
    }

    public void LoadScene(int sceneBuildId)
    {
        circleImage.transform.localScale = Vector3.zero;
        circleImage.transform.DOScale(30f, 1f).OnComplete(() =>
        {
            SceneManager.LoadScene(sceneBuildId);
            circleImage.transform.localScale = Vector3.one * 30f;
            circleImage.transform.DOScale(0f, 1f);
        });
    }
}

