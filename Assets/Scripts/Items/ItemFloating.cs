using UnityEngine;
using DG.Tweening;

public class ItemFloating : MonoBehaviour
{
    public float FloatDistance = 0.1f;
    public float Speed = 1.2f;
    public bool IsForward = true;
    private Vector3 basePos;
    private Vector3 baseDir;

    private void Start()
    {
        baseDir = IsForward ? transform.forward : transform.up;
        basePos = transform.position;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * Speed) * FloatDistance;
        transform.position = basePos + baseDir * offset;
    }
}
