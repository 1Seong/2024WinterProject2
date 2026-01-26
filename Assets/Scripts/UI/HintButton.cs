using DG.Tweening;
using TMPro;
using UnityEngine;

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
    Tween scrollTween;
    float moveDistance;

    [SerializeField]bool isActive = false;
    [SerializeField]bool isActing = false;
    public Transform hint;
    public float dur = 0.3f;

    public void Start()
    {
        textRect = tmp.rectTransform;
        startPos = textRect.anchoredPosition;

        var textWidth = tmp.preferredWidth;
        var maskWidth = maskRect.rect.width;
        moveDistance = Mathf.Max(0f, textWidth - maskWidth);
    }

    public void OnClick()
    {
        if (isActing || hint == null) return;

        isActing = true;

        if (!isActive) // open
        {
            isActive = true;
            hint.DOScale(1f, dur).SetEase(Ease.OutBack).OnComplete(setIsActingFalse);

        }
        else // close
        {
            isActive = false;
            hint.DOScale(0f, dur).SetEase(Ease.InBack).OnComplete(setIsActingFalse);
        }

        
    }

    private void Update()
    {
        if (!GameManager.instance.isPlaying || isActing) return;

        if (Input.GetKeyDown(KeyCode.H)) OnClick();
    }

    void setIsActingFalse()
    {
        isActing = false;
    }

    void slideText()
    {
        setIsActingFalse();

        if (moveDistance <= 0f)
            return;

        float duration = moveDistance / speed;

        scrollTween = textRect
            .DOAnchorPosX(startPos.x - moveDistance, duration)
            .SetDelay(startDelay)
            .SetEase(ease)
            .SetLoops(-1, LoopType.Restart);
    }

    void stopSlidingText()
    {
        setIsActingFalse();

        scrollTween.Kill();
    }
}
