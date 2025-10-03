using System.Collections;
using UnityEngine;

public class RotateWhileConvert : MonoBehaviour
{
    //[SerializeField] private bool inverted;
    private bool isActing;
    private Vector3 originalPos;

    private void Start()
    {
        originalPos = transform.position;
        transform.position = new Vector3(originalPos.x, 0f, originalPos.z);
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
        Collider coll;

        

        if (TryGetComponent(out coll))
            coll.enabled = false;
        isActing = true;
        
        /*
        if(!sideview)
        {
            if (inverted) transform.Translate(new Vector3(0, 0, -0.5f), Space.World);
            else transform.Translate(new Vector3(0, 0, -6.5f), Space.World);
        }
        */
       

        

        transform.position = originalPos;
        //if (tag.Equals("Door") && sideview) transform.Translate(new Vector3(0, 0.5f, 0));
        for (float i = 0; i <= totalTime; i += Time.fixedDeltaTime)
        {
            transform.Rotate(new Vector3(targetRot / ((totalTime / Time.fixedDeltaTime) + 1), 0, 0));
           
            yield return new WaitForFixedUpdate(); // Wait for a fixed delta time
        }

        //if (tag.Equals("Door") && !sideview) transform.Translate(new Vector3(0, -0.5f, 0));
        /*
        if (sideview)
        {
            if (inverted) transform.Translate(new Vector3(0, 0, 0.5f), Space.World);
            else transform.Translate(new Vector3(0, 0, 6.5f), Space.World);
        }
        */

        if (sideview)
        {
            transform.position = new Vector3(originalPos.x, originalPos.y, 7.5f);
            if(tag.Equals("Door")) transform.Translate(new Vector3(0, 0, 0.5f));
        }
        else
        {
            transform.position = new Vector3(originalPos.x, 0f, originalPos.z);
        }
        if (TryGetComponent(out coll))
            coll.enabled = true;

        isActing = false;
    }
}
