using System.Collections;
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
        {
            StartCoroutine(CheckConvertCollision(new Vector3(0.9f, 0.9f, 16f)));
        }
        else // side -> top
        {
            StartCoroutine(CheckConvertCollision(new Vector3(0.9f, 1f, 0.9f)));
        }
    }

    private IEnumerator CheckConvertCollision(Vector3 targetV)
    {
        bool collide = false;
        Vector3 box = new Vector3(0.36f, 0.36f, 0.36f);
        var rigid = GetComponent<Rigidbody>();

        Collider[] hits = Physics.OverlapBox(rigid.position, box, Quaternion.identity, LayerMask.GetMask("Platform"));
        collide = ObjectExistInRaycast(hits);

        while (collide)
        {
            //Debug.Log("Collide!");
            rigid.position += GetComponent<CustomGravity>().up;
            yield return null;
            hits = Physics.OverlapBox(rigid.position, box, Quaternion.identity, LayerMask.GetMask("Platform"));
            collide = ObjectExistInRaycast(hits);
        }
        GetComponent<BoxCollider>().size = targetV;
    }

    protected bool ObjectExistInRaycast(Collider[] hits)
    {
        if (hits == null || hits.Length == 0) return false;

        foreach (var i in hits)
            if (!i.CompareTag(tag) && !i.CompareTag("Ice") && !i.CompareTag("Player2"))
                return true;

        return false;
    }
}
