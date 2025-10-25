using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AutoScroll : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("�ڵ����� ��ũ�� �� ��(width �Ǵ� height)")]
    [SerializeField] private float mScrollSpeed = 100.0f;  // ��ũ�� �ӵ�
    [Header("�ڵ� ��ũ���� ������ ������ �ð�")]
    [SerializeField] private float mScrollDelay = 2f;  // �ڵ� ��ũ�� ���� ������

    [SerializeField] private RectTransform mScrollRectTransform;

    private ScrollRect mScrollRect; 
    private Coroutine? mCoAutoScroll; 
    private bool mIsPointerEnter = false; 
    private float scrollSpeed;

    void Awake()
    {
        mScrollRect = GetComponent<ScrollRect>();
        scrollSpeed = mScrollSpeed;
    }


    private void OnEnable()
    {
        ToggleAutoScroll(true);
    }


    private void OnDisable()
    {
        ToggleAutoScroll(false);
    }

    private void ToggleAutoScroll(bool isEnable)
    {
        mScrollRect.velocity = Vector2.zero;

        if (mCoAutoScroll is not null)
            StopCoroutine(mCoAutoScroll);

        if (isEnable)
        {
            mCoAutoScroll = StartCoroutine(CoAutoScroll());
            mIsPointerEnter = false;
        }
    }

    private IEnumerator CoAutoScroll()
    {
        yield return new WaitForSecondsRealtime(mScrollDelay);

        while (true)
        {
            mScrollRect.verticalNormalizedPosition -= scrollSpeed * Time.deltaTime / mScrollRectTransform.sizeDelta.y;
            // ��ũ���� �� ������ �����ߴٸ� ������ ����
            if (mScrollRect.verticalNormalizedPosition <= 0f || mScrollRect.verticalNormalizedPosition >= 1f)
                scrollSpeed = 0.0f;
            else
                scrollSpeed = mScrollSpeed; 
            // ������ �����Ͱ� ������ ���¿��� Ŭ�� �Ǵ� �巡�׸� �ߴٸ�?
            if (mIsPointerEnter && (Input.GetMouseButton(0) || Input.GetAxis("Mouse ScrollWheel") > 0))
                ToggleAutoScroll(false);

            yield return null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mIsPointerEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mIsPointerEnter = false;
        ToggleAutoScroll(true);
    }
}
