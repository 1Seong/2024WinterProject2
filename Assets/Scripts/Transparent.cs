using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Transparent : MonoBehaviour
{
    [SerializeField] private float totalTime = 1f;
    public bool isActing;
    [SerializeField] protected float _maxAlpha = 1f;

    protected Collider coll;
    protected Material[] mats;
    protected SpriteRenderer[] renderers;
    protected Tilemap[] tilemaps;

    public bool active = true;

    protected void Awake()
    {
        MeshRenderer mr;
        coll = GetComponent<Collider>();
        if (TryGetComponent(out mr))
            mats = mr.materials;

        renderers = GetComponentsInChildren<SpriteRenderer>();
        tilemaps = GetComponentsInChildren<Tilemap>();
    }

    public void CallFade()
    {
        //Debug.Log("Fade");
        StartCoroutine(Fade());
    }

    public void CallEmerge()
    {
        if (active) return;
        StartCoroutine(Emerge());
    }

    protected IEnumerator Emerge()
    {
        isActing = true;

        for (float i = 0; i <= totalTime; i += Time.fixedDeltaTime)
        {
            if (mats is not null)
                foreach (var mat in mats)
                {
                    Color color = mat.color;
                    float amount = Mathf.Lerp(0f, _maxAlpha, i / totalTime); //appear

                    color.a = amount;
                    mat.color = color;
                }

            if (renderers is not null)
                foreach (var r in renderers)
                {
                    Color color = r.color;
                    float amount = Mathf.Lerp(0f, _maxAlpha, i / totalTime); //appear

                    color.a = amount;
                    r.color = color;
                }

            if (tilemaps is not null)
                foreach (var t in tilemaps)
                {
                    Color color = t.color;
                    float amount = Mathf.Lerp(0f, _maxAlpha, i / totalTime); //appear
                  
                    color.a = amount;
                    t.color = color;
                }

            yield return new WaitForFixedUpdate(); // Wait for a fixed delta time
        }
        isActing = false;
        active = true;
    }

    protected IEnumerator Fade()
    {
        isActing = true;

        for (float i = 0; i <= totalTime; i += Time.fixedDeltaTime)
        {
            if (mats is not null)
                foreach (var mat in mats)
                {
                    Color color = mat.color;
                    float amount = Mathf.Lerp(_maxAlpha, 0f, i / totalTime); //appear

                    color.a = amount;
                    mat.color = color;
                }

            if (renderers is not null)
                foreach (var r in renderers)
                {
                    Color color = r.color;
                    float amount = Mathf.Lerp(_maxAlpha, 0f, i / totalTime); //appear

                    color.a = amount;
                    r.color = color;
                }

            if (tilemaps is not null)
                foreach (var t in tilemaps)
                {
                    Color color = t.color;
                    float amount = Mathf.Lerp(_maxAlpha, 0f, i / totalTime); //appear

                    color.a = amount;
                    t.color = color;
                }

            yield return new WaitForFixedUpdate(); // Wait for a fixed delta time
        }
        isActing = false;
        active = false;

        if(gameObject.CompareTag("Player1") || gameObject.CompareTag("Player2"))
            gameObject.SetActive(false);
    }
}

// TODO : refactor - convert actions