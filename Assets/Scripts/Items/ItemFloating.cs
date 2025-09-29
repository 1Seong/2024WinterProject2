using UnityEngine;
using DG.Tweening;

public class ItemFloating : MonoBehaviour
{
    public float FloatDistance = 0.1f;
    public float Speed = 1.2f;
    private Vector3 basePos;

    private void Start()
    {
        basePos = transform.position;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * Speed) * FloatDistance;
        transform.position = basePos + transform.forward * offset;
    }
}
