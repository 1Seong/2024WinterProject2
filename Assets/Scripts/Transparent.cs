using System.Collections;
using UnityEngine;

public class Transparent : MonoBehaviour
{
    [SerializeField] private float totalTime = 1f;
    public bool isActing;

    public void CallFade()
    {
        Debug.Log("Fade");
        StartCoroutine(Fade());
    }

    protected virtual IEnumerator Fade()
    {
        bool sideview = GameManager.instance.isSideView;
        Material mat = GetComponent<MeshRenderer>().material;

        isActing = true;

        for (float i = 0; i <= totalTime; i += Time.fixedDeltaTime)
        {
            Color color = mat.color;
            float amount = Mathf.Lerp(1f, 0f, i / totalTime);

            color.a = amount;
            mat.color = color;

            yield return new WaitForFixedUpdate(); // Wait for a fixed delta time
        }

        gameObject.SetActive(false);
        isActing = false;
    }
}

// TODO : refactor - convert actions