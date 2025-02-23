using System.Collections;
using UnityEngine;

public class RotateTransparent : Transparent
{
    [SerializeField] private bool isSideViewObject;

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

        if (!sideview && !isSideViewObject || sideview && isSideViewObject) //when appear
            gameObject.SetActive(true);

        for (float i = 0; i <= totalTime; i += Time.fixedDeltaTime)
        {
            Color color = mat.color;
            float amount = Mathf.Lerp(0f, 1f, i / totalTime); //appear
            if (sideview && isSideViewObject || !sideview && !isSideViewObject) //disappear
                amount = 1f - amount;

            color.a = amount;
            mat.color = color;

            yield return new WaitForFixedUpdate(); // Wait for a fixed delta time
        }

        if (sideview && !isSideViewObject || !sideview && isSideViewObject) //when disappear
            gameObject.SetActive(false);

        isActing = false;
    }

    private void OnDestroy()
    {
        Stage.convertEvent -= CallFade;
    }
}
