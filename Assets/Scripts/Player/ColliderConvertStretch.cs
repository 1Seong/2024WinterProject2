using System;
using System.Collections;
using UnityEngine;

public class ColliderConvertStretch : MonoBehaviour
{
    private void Start()
    {
        Stage.convertEvent += ConvertSpringCollider;
    }

    private void OnDisable()
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
        var rigid = GetComponent<Rigidbody>();
        var gravity = GetComponent<CustomGravity>();
        var boxCollider = GetComponent<BoxCollider>();

        if (rigid == null || gravity == null || boxCollider == null)
        {
            Debug.LogError("Required component missing");
            yield break;
        }

        Vector3 box = new Vector3(0.36f, 0.36f, 0.36f);

        bool collide;
        Collider[] hits;

        while (true)
        {
            try
            {
                hits = Physics.OverlapBox(
                    rigid.position,
                    box,
                    Quaternion.identity,
                    LayerMask.GetMask("Platform")
                );

                collide = ObjectExistInRaycast(hits);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                yield break;
            }

            if (!collide)
                break;

            rigid.position += gravity.up;
            yield return null; // try-catch ¹Ù±ù
        }

        boxCollider.size = targetV;
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
