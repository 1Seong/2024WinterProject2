using DG.Tweening;
using UnityEngine;

public class HintButton : MonoBehaviour
{
    [SerializeField]bool isActive = false;
    [SerializeField]bool isActing = false;
    public Transform hint;
    public float dur = 0.3f;

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
}
