using System.Collections;
using UnityEngine;

public class RotateTransparent : Transparent
{
    private enum mode { fade, instant }
    [SerializeField] private mode _mode;
    [SerializeField] private bool _isSideViewObject;

    private void Start()
    {
        Stage.convertEvent += CallFade;
    }

    protected override IEnumerator Fade()
    {
        bool sideview = GameManager.instance.isSideView;
        float totalTime = GameManager.instance.cameraRotationTime;
        Material mat = GetComponent<MeshRenderer>().material;

        isActing = true;

        if (!sideview && !_isSideViewObject || sideview && _isSideViewObject) //when appear
            gameObject.SetActive(true);

        for (float i = 0; i <= totalTime; i += Time.fixedDeltaTime)
        {
            if (_mode == mode.fade)
            {
                Color color = mat.color;
                float amount = Mathf.Lerp(0f, 1f, i / totalTime); //appear
                if (sideview && _isSideViewObject || !sideview && !_isSideViewObject) //disappear
                    amount = 1f - amount;

                color.a = amount;
                mat.color = color;
            }
            yield return new WaitForFixedUpdate(); // Wait for a fixed delta time
        }

        if (sideview && !_isSideViewObject || !sideview && _isSideViewObject) //when disappear
            gameObject.SetActive(false);

        isActing = false;
    }

    private void OnDestroy()
    {
        Stage.convertEvent -= CallFade;
    }
}
