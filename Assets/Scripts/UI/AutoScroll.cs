using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AutoScroll : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("자동으로 스크롤 할 양(width 또는 height)")]
    [SerializeField] private float mScrollSpeed = 100.0f;  // 스크롤 속도
    [Header("자동 스크롤을 시작할 딜레이 시간")]
    [SerializeField] private float mScrollDelay = 2f;  // 자동 스크롤 시작 딜레이

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
            // 스크롤의 끝 영역에 도달했다면 방향을 반전
            if (mScrollRect.verticalNormalizedPosition <= 0f || mScrollRect.verticalNormalizedPosition >= 1f)
                scrollSpeed = 0.0f;
            else
                scrollSpeed = mScrollSpeed; 
            // 영역에 포인터가 진입한 상태에서 클릭 또는 드래그를 했다면?
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
