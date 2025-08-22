using System.Collections;
using UnityEngine;

public class GrayScript : MonoBehaviour
{
    public Material mat;
    public float duration = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mat = GetComponentInChildren<SpriteRenderer>().material;
        //turnGray();
    }

    public void turnGray()
    {
       // Debug.Log("gray start!");
        StartCoroutine(GrayRoutine());
    }

    public void turnDeGray()
    {
        //Debug.Log("degray start!");
        StartCoroutine(deGrayRoutine());
    }

    private IEnumerator GrayRoutine()
    {
        float t = duration;
        while (t > 0.0f)
        {
            mat.SetFloat("_GrayAmount", t);
            t -= Time.deltaTime;
            yield return null;
        }

        mat.SetFloat("_GrayAmount", 0);
        //Debug.Log("gray complete!");
    }

    private IEnumerator deGrayRoutine()
    {
        float t = 0;
        while (t < duration)
        {
            mat.SetFloat("_GrayAmount", t);
            t += Time.deltaTime;
            yield return null;
        }

        mat.SetFloat("_GrayAmount", 1);
        //Debug.Log("degray complete!");
    }
}
