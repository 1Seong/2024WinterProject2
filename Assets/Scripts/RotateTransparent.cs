using System.Collections;
using UnityEngine;

public class RotateTransparent : Transparent
{
    private enum mode { fade, instant }
    [SerializeField] private mode _mode;
    [SerializeField] private bool _isSideViewObject;
    [SerializeField] private float _maxAlpha = 1f;

    private Material[] mats = null;

    private void Awake()
    {
        if(GetComponent<MeshRenderer>() != null)
            mats = GetComponent<MeshRenderer>().materials;
    }

    private void Start()
    {
        Stage.convertEvent += CallFade;

        if (_isSideViewObject)
        {
            if(mats != null)
                foreach(var mat in mats)
                {
                    Color color = mat.color;
                    color.a = 0f;
                    mat.color = color;
                }
        }

    }

    protected override IEnumerator Fade()
    {
        bool sideview = GameManager.instance.isSideView;
        float totalTime = GameManager.instance.cameraRotationTime;

        isActing = true;

        if (!sideview && !_isSideViewObject || sideview && _isSideViewObject) //when appear
            gameObject.GetComponent<Collider>().enabled = true;

        for (float i = 0; i <= totalTime; i += Time.fixedDeltaTime)
        {
            if (_mode == mode.fade)
            {
                foreach(var mat in mats)
                {
                    Color color = mat.color;
                    float amount = Mathf.Lerp(0f, _maxAlpha, i / totalTime); //appear
                    if (sideview && !_isSideViewObject || !sideview && _isSideViewObject) //disappear
                        amount = _maxAlpha - amount;

                    color.a = amount;
                    mat.color = color;
                }
            }
            yield return new WaitForFixedUpdate(); // Wait for a fixed delta time
        }

        if(_mode == mode.instant)
        {
            if(mats != null)
                foreach(var mat in mats)
                {
                    Color color = mat.color;

                    if (sideview && !_isSideViewObject || !sideview && _isSideViewObject) // disappear
                    {
                        color.a = 0f;
                    }
                    else // appear
                    {
                        color.a = 1f;
                    }

                    mat.color = color;
                }
        }

        if (sideview && !_isSideViewObject || !sideview && _isSideViewObject) //when disappear
            gameObject.GetComponent<Collider>().enabled = false;

        isActing = false;
    }

    private void OnDestroy()
    {
        Stage.convertEvent -= CallFade;
    }
}
