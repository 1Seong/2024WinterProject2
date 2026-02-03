using UnityEngine;
using UnityEngine.UI;

public class VertAutoScroll : MonoBehaviour
{
    public ScrollRect scrollRect;
    public float scrollSpeed = 0.1f; // 초당 이동량

    void Update()
    {
        scrollRect.verticalNormalizedPosition -= scrollSpeed * Time.deltaTime;

        if (scrollRect.verticalNormalizedPosition <= 0f)
        {
            scrollRect.verticalNormalizedPosition = 1f;
        }
    }
}
