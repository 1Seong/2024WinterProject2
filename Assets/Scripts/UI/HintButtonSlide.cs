using DG.Tweening;
using UnityEngine;

public class HintButtonSlide : HintButton
{
    public float speed = 30f;
    public float startDelay = 0.5f;
    public float endDelay = 0.5f;
    public Ease ease = Ease.Linear;
    public float targetDisSpacing = 50f;

    Sequence scrollTween;
    float moveDistance;

    private void OnDisable()
    {
        scrollTween = null;
    }

    public override void LangSwitchCallback()
    {
        cached = false;

        tmp.ForceMeshUpdate();

        if (isActive)
            slideText();
    }

    protected override void toggleOnImpl()
    {
        slideText();
    }

    protected override void toggleOffImpl()
    {
        base.toggleOffImpl();
    }

    void slideText()
    {
        setIsActingFalse();
        stopSlidingText();

        textRect.anchoredPosition = startPos;

        if (!cached) // 캐시가 안되어있으면 거리 계산
        {
            var textWidth = tmp.textBounds.size.x;
            var maskWidth = maskRect.rect.width;

            Debug.Log(textWidth.ToString() + " " + maskWidth.ToString());
            moveDistance = Mathf.Max(0f, textWidth - maskWidth);

            cached = true;
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
}
