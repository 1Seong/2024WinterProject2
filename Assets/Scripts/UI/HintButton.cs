using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;

public class HintButton : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public RectTransform maskRect;

    protected RectTransform textRect;
    protected Vector2 startPos;
    protected bool cached = false;

    [SerializeField] protected bool isActive = false;
    [SerializeField] protected bool isActing = false;
    public Transform hint;
    public float dur = 0.35f;
    public float targetScale = 0.76f;

    protected virtual void Start()
    {
        textRect = tmp.rectTransform;
        startPos = textRect.anchoredPosition;

        tmp.GetComponent<LocalizeStringEvent>().StringReference.SetReference("New Table", SceneManager.GetActiveScene().name);
    }

    public void OnClick()
    {
        if (isActing || hint == null) return;

        isActing = true;

        if (!isActive) // open
        {
            isActive = true;
            hint.DOScale(targetScale, dur).SetEase(Ease.OutBack).OnComplete(toggleOnImpl);

        }
        else // close
        {
            isActive = false;
            hint.DOScale(0f, dur).SetEase(Ease.InBack).OnComplete(toggleOffImpl);
        }
    }

    private void Update()
    {
        if (!GameManager.instance.isPlaying || isActing) return;
        if (SceneManager.GetActiveScene().name == "HubStage") return;

        if (Input.GetKeyDown(KeyCode.H)) OnClick();
    }

    protected void setIsActingFalse()
    {
        isActing = false;
    }
    
    // template method pattern
    protected virtual void toggleOnImpl() { setIsActingFalse(); }

    protected virtual void toggleOffImpl() { setIsActingFalse(); }

    public virtual void LangSwitchCallback() { }
}
