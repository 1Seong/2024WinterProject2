using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Spring : MonoBehaviour
{
    Rigidbody rigid;
    BoxCollider coll;
    CustomGravity customGravity;
    Vector3 compressVector;
    float compressFloat = 0.4f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        coll = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {

    }

    void Start()
    {
        //Vector3 compressVec = rigid.transform.position - new Vector3(0.0f, 0.0f, 0.2f);
        //Vector3 speed = Vector3.zero;
        //rigid.transform.position = Vector3.SmoothDamp(rigid.transform.position, new Vector3(0.0f, 0.0f, 0.0f), ref speed, 0.1f);
        StartCoroutine(compress(compressFloat));
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        var obj = collision.gameObject;
        if (obj.tag == "Player")
        {
            StartCoroutine(compress(compressFloat));
        }
    }
    
    ///////////////// coroutines for Spring activation /////////////////
    IEnumerator compress(float compressSize)
    {
        Debug.Log("compress start");
        yield return null;
        Vector3 compressVec = transform.position - new Vector3(0.0f, 0.0f, compressSize);
        Vector3 speed = Vector3.zero;
        transform.position = Vector3.SmoothDamp(compressVec, transform.position, ref speed, 10.0f);
       
        StartCoroutine(extend(compressSize));
    }
    IEnumerator extend(float extendSize)
    {
        Debug.Log("Extend start");
        yield return new WaitForSeconds(0.5f);
        Vector3 extendVec = transform.position + new Vector3(0.0f, 0.0f, extendSize);
        Vector3 speed = Vector3.zero;
        transform.position = Vector3.SmoothDamp(extendVec, transform.position, ref speed, 10.0f);
    }

}
