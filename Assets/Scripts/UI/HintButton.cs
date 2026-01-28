using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;

public class HintButton : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public RectTransform maskRect;

    public float speed = 30f;
    public float startDelay = 0.5f;
    public float endDelay = 0.5f;
    public Ease ease = Ease.Linear;

    RectTransform textRect;
    Vector2 startPos;
    Sequence scrollTween;
    float moveDistance;
    bool disCached = false;

    [SerializeField]bool isActive = false;
    [SerializeField]bool isActing = false;
    public Transform hint;
    public float dur = 0.35f;
    public float targetScale = 0.76f;
    public float targetDisSpacing = 50f;

    public void Start()
    {
        textRect = tmp.rectTransform;
        startPos = textRect.anchoredPosition;

        tmp.GetComponent<LocalizeStringEvent>().StringReference.SetReference("New Table", SceneManager.GetActiveScene().name);
    }

    public void OnDisable()
    {
        scrollTween = null;
    }

    public void OnClick()
    {
        if (isActing || hint == null) return;

        isActing = true;

        if (!isActive) // open
        {
            isActive = true;
            hint.DOScale(targetScale, dur).SetEase(Ease.OutBack).OnComplete(slideText);

        }
        else // close
        {
            isActive = false;
            hint.DOScale(0f, dur).SetEase(Ease.InBack).OnComplete(stopSlidingText);
        }

        
    }

    private void Update()
    {
        if (!GameManager.instance.isPlaying || isActing) return;
        if (SceneManager.GetActiveScene().name == "HubStage") return;

        if (Input.GetKeyDown(KeyCode.H)) OnClick();
    }

    void setIsActingFalse()
    {
        isActing = false;
    }

    void slideText()
    {
        setIsActingFalse();
        stopSlidingText();

        textRect.anchoredPosition = startPos;

        if (!disCached) // 캐시가 안되어있으면 거리 계산
        {
            var textWidth = tmp.textBounds.size.x;
            var maskWidth = maskRect.rect.width;

            Debug.Log(textWidth.ToString() + " " + maskWidth.ToString());
            moveDistance = Mathf.Max(0f, textWidth - maskWidth);

            disCached = true;
        }

        if (moveDistance <= 0f)
            return;

        float duration = moveDistance / speed;

        scrollTween = DOTween.Sequence()
            .AppendInterval(startDelay)
            .Append(
                textRect.DOAnchorPosX(startPos.x - moveDistance - targetDisSpacing, duration)
                        .SetEase(Ease.Linear)
            )
            .AppendInterval(endDelay)
            .SetEase(ease)
            .SetLoops(-1, LoopType.Restart);
    }

    void stopSlidingText()
    {
        setIsActingFalse();

        if (scrollTween.IsActive())
        {
            scrollTween.Kill();
            //Debug.Log("Killed");
            textRect.anchoredPosition = startPos;
        }
    }

    public void ResetSlide(string _)
    {
        disCached = false;

        tmp.ForceMeshUpdate();

        if(isActive)
            slideText();
    }
}
