using System.Collections;
using UnityEngine;

public class RotateWhileConvert : MonoBehaviour
{
    [SerializeField] private bool isEnabled;
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
        float targetRot = (sideview && !inverted || !sideview && inverted) ? -90.0f : 90.0f;
        float totalTime = GameManager.instance.cameraRotationTime;

        isActing = true;

        for (float i = 0; i <= totalTime; i += Time.fixedDeltaTime)
        {
            if (!sideview) transform.Translate(new Vector3(0, -1, 0));
            transform.Rotate(new Vector3(targetRot, 0, 0));
            if (sideview) transform.Translate(new Vector3(0, 1, 0));

            yield return new WaitForFixedUpdate(); // Wait for a fixed delta time
        }

        isActing = false;
    }
}
