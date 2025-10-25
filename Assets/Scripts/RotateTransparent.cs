using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RotateTransparent : Transparent
{
    private enum mode { fade, instant }
    [SerializeField] private mode _mode;
    [SerializeField] private bool _isSideViewObject;

    public void StopCallFade()
    {
        Stage.convertEvent -= CallFade;
    }

    public void DoCallFade()
    {
        Stage.convertEvent += CallFade;
    }

    private void Start()
    {
        Stage.convertEvent += CallFade;

        if (_isSideViewObject)
        {
            if(coll != null)
                coll.enabled = false;

            if(mats is not null)
                foreach(var mat in mats)
                {
                    Color color = mat.color;
                    color.a = 0f;
                    mat.color = color;
                }
            if(renderers is not null)
                foreach (var r in renderers)
                {
                    Color color = r.color;
                    color.a = 0f;
                    r.color = color;
                }
            if(tilemaps is not null)
                foreach (var tilemap in tilemaps)
                {
                    Color color = tilemap.color;
                    color.a = 0f;
                    tilemap.color = color;
                }
            if (GetComponent<ParticleSystem>() != null)
            {
                var main = GetComponent<ParticleSystem>().main;
                var color = main.startColor.color;

                color.a = 0f;
                main.startColor = color;
            }
                
        }

    }

    public new void CallFade()
    {
        bool sideview = GameManager.instance.isSideView;

        if(coll != null)
        {
            if (!sideview && !_isSideViewObject || sideview && _isSideViewObject) //when appear
                coll.enabled = true;

            if (sideview && !_isSideViewObject || !sideview && _isSideViewObject) //when disappear
                coll.enabled = false;
        }

        if (_mode == mode.fade)
            StartCoroutine(Fade());
        else
        {
            if (mats is not null)
                foreach (var m in mats)
                    instantImpl(m);

            if (renderers is not null)
                foreach (var r in renderers)
                    instantImpl(r);

            if (tilemaps is not null)
                foreach (var t in tilemaps)
                    instantImpl(t);

            if(GetComponent<ParticleSystem>() != null)
                instantImpl(GetComponent<ParticleSystem>());

        }

    }

    protected new IEnumerator Fade()
    {
        bool sideview = GameManager.instance.isSideView;
        float totalTime = GameManager.instance.cameraRotationTime;

        isActing = true;

        for (float i = 0; i <= totalTime; i += Time.fixedDeltaTime)
        {
            if(mats is not null)
                foreach(var mat in mats)
                {
                    Color color = mat.color;
                    float amount = Mathf.Lerp(0f, _maxAlpha, i / totalTime); //appear
                    if (sideview && !_isSideViewObject || !sideview && _isSideViewObject) //disappear
                        amount = _maxAlpha - amount;

                    color.a = amount;
                    mat.color = color;
                }

            if (renderers is not null)
                foreach (var r in renderers)
                {
                    Color color = r.color;
                    float amount = Mathf.Lerp(0f, _maxAlpha, i / totalTime); //appear
                    if (sideview && !_isSideViewObject || !sideview && _isSideViewObject) //disappear
                        amount = _maxAlpha - amount;

                    color.a = amount;
                    r.color = color;
                }

            if(tilemaps is not null)
                foreach (var t in tilemaps)
                {
                    Color color = t.color;
                    float amount = Mathf.Lerp(0f, _maxAlpha, i / totalTime); //appear
                    if (sideview && !_isSideViewObject || !sideview && _isSideViewObject) //disappear
                        amount = _maxAlpha - amount;

                    color.a = amount;
                    t.color = color;
                }
            if (ps != null)
            {
                var main = GetComponent<ParticleSystem>().main;
                var color = main.startColor.color;
                float amount = Mathf.Lerp(0f, _maxAlpha, i / totalTime);
                if (sideview && !_isSideViewObject || !sideview && _isSideViewObject) //disappear
                    amount = _maxAlpha - amount;
                color.a = amount;
                main.startColor = color;
            }

            yield return new WaitForFixedUpdate(); // Wait for a fixed delta time
        }

        isActing = false;
    }

    private void instantImpl(Material o)
    {
        var color = o.color;
        var sideview = GameManager.instance.isSideView;

        if (sideview && !_isSideViewObject || !sideview && _isSideViewObject) // disappear
        {
            color.a = 0f;
        }
        else // appear
        {
            color.a = 1f;
        }

        o.color = color;
    }

    private void instantImpl(SpriteRenderer o)
    {
        var color = o.color;
        var sideview = GameManager.instance.isSideView;

        if (sideview && !_isSideViewObject || !sideview && _isSideViewObject) // disappear
        {
            color.a = 0f;
        }
        else // appear
        {
            color.a = 1f;
        }

        o.color = color;
    }

    private void instantImpl(Tilemap o)
    {
        var color = o.color;
        var sideview = GameManager.instance.isSideView;

        if (sideview && !_isSideViewObject || !sideview && _isSideViewObject) // disappear
        {
            color.a = 0f;
        }
        else // appear
        {
            color.a = 1f;
        }

        o.color = color;
    }

    private void instantImpl(ParticleSystem o)
    {
        var main = o.main;
        var color = main.startColor.color;
        var sideview = GameManager.instance.isSideView;

        if (sideview && !_isSideViewObject || !sideview && _isSideViewObject) // disappear
        {
            color.a = 0f;
            o.Clear();
        }
        else // appear
        {
            color.a = 1f;
        }

        main.startColor = color;
    }

    private void OnDestroy()
    {
        Stage.convertEvent -= CallFade;
    }
}
