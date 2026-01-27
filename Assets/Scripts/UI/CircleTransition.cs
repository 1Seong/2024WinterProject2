using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class CircleTransition : MonoBehaviour
{
    public static CircleTransition Instance;
    public Image circleImage;

    private float targetScale = 13f;

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

    private void Start()
    {
        circleImage.transform.localScale = Vector3.one * targetScale;
        circleImage.transform.DOScale(0f, 0.6f);
    }

    public void LoadScene(string sceneName)
    {
        var scale = targetScale;

        circleImage.transform.localScale = Vector3.zero;
        circleImage.transform.DOScale(targetScale, 0.6f).SetEase(Ease.InOutSine).SetUpdate(true).OnComplete(() => StartCoroutine(LoadAsync(sceneName, scale)));
    }

    public void LoadScene(int sceneBuildId)
    {
        var scale = targetScale;

        circleImage.transform.localScale = Vector3.zero;
        circleImage.transform.DOScale(targetScale, 0.6f).SetEase(Ease.InOutSine).SetUpdate(true).OnComplete(() => StartCoroutine(LoadAsync(sceneBuildId, scale)));
    }

    IEnumerator LoadAsync(string sceneName, float scale)
    {
        var op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        // 씬 로딩은 백그라운드에서 진행
        while (op.progress < 0.9f)
            yield return null;

        // 씬 전환 시점 제어
        op.allowSceneActivation = true;

        // 다음 프레임부터 축소 애니메이션
        yield return null;
        yield return null;

        circleImage.transform.localScale = Vector3.one * scale;
        circleImage.transform.DOScale(0f, 0.6f)
            .SetEase(Ease.InOutSine).SetUpdate(true);
    }

    IEnumerator LoadAsync(int sceneBuildId, float scale)
    {
        var op = SceneManager.LoadSceneAsync(sceneBuildId);
        op.allowSceneActivation = false;

        // 씬 로딩은 백그라운드에서 진행
        while (op.progress < 0.9f)
            yield return null;

        // 씬 전환 시점 제어
        op.allowSceneActivation = true;

        // 다음 프레임부터 축소 애니메이션
        yield return null;
        yield return null;

        circleImage.transform.localScale = Vector3.one * scale;
        circleImage.transform.DOScale(0f, 0.6f)
            .SetEase(Ease.InOutSine).SetUpdate(true);
    }
}

