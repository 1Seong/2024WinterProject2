using System.Collections;
using UnityEngine;

public class RotateWhileConvert : MonoBehaviour
{
    [SerializeField] private bool inverted;
    private bool isActing;

    private void Start()
    {
        Stage.convertEvent += CallObjectRotate;
    }

    private void OnDestroy()
    {
        Stage.convertEvent -= CallObjectRotate;
    }

    private void CallObjectRotate()
    {
        StartCoroutine(ObjectRotate());
    }

    IEnumerator ObjectRotate()
    {
        /*
         * Rotate Camera and make bottom and top wall transparent
         */
        bool sideview = GameManager.instance.isSideView;
        float targetRot = sideview ? -90f : 90f;
        float totalTime = GameManager.instance.cameraRotationTime;
       
        isActing = true;

        if(!sideview)
        {
            if (inverted) transform.Translate(new Vector3(0, 0, -0.5f), Space.World);
            else transform.Translate(new Vector3(0, 0, -6.5f), Space.World);
        }

        if (sideview) transform.Translate(new Vector3(0, 0.5f, 0));

        for (float i = 0; i <= totalTime; i += Time.fixedDeltaTime)
        {
            transform.Rotate(new Vector3(targetRot / ((totalTime / Time.fixedDeltaTime) + 1), 0, 0));
           
            yield return new WaitForFixedUpdate(); // Wait for a fixed delta time
        }

        if (!sideview) transform.Translate(new Vector3(0, -0.5f, 0));

        if (sideview)
        {
            if (inverted) transform.Translate(new Vector3(0, 0, 0.5f), Space.World);
            else transform.Translate(new Vector3(0, 0, 6.5f), Space.World);
        }

        isActing = false;
    }
}
