using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class ColliderConvertStretch : MonoBehaviour
{
    private void Start()
    {
        Stage.convertEvent += ConvertSpringCollider;
    }

    private void OnDestroy()
    {
        Stage.convertEvent -= ConvertSpringCollider;
    }

    private void ConvertSpringCollider()
    {
        if (GameManager.instance.isSideView) // top -> side
            GetComponent<BoxCollider>().size = new Vector3(0.9f, 0.9f, 16f);
        else // side -> top
            GetComponent<BoxCollider>().size = new Vector3(0.9f, 1f, 0.9f);
    }
}
