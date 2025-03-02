using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Spring : MonoBehaviour
{
    Rigidbody rigid;
    BoxCollider coll;
    CustomGravity customGravity;
    Vector3 compressVector;
    //float compressFloat = 0.4f;
    public bool isIdle = true;

    public int springJumpUnit = 4;

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
      
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnCollisionEnter(Collision collision)
    {
        Vector3 originalPos = transform.parent.position + new Vector3(0.0f, 0.0f, 0.4f);
        var obj = collision.gameObject;
        if (obj.tag == "Player" && isIdle)
        {
            springAction(obj);
            transform.position = originalPos;
        }
    }
    
    
    IEnumerator compress(GameObject obj)
    {
        Debug.Log("compress start");
        Vector3 centerPos = transform.parent.position;
        
        for(float i = 0; i <= 1.0f; i += Time.fixedDeltaTime) {

            transform.position = transform.position - new Vector3(0.0f, 0.0f, 0.01f);
            yield return null;
        }

        //Vector3 compressVec = transform.position - new Vector3(0.0f, 0.0f, compressSize);
        //Vector3 speed = Vector3.zero;
        //for (float f = 0.0f; f < compressSize; f++)
        //{

        //}
        //transform.position = Vector3.SmoothDamp(compressVec, transform.position, ref speed, 10.0f);
        //////////////////////////////////////////// remove at final////////////////////////////////////////
       
        StartCoroutine(extend(obj));
    }
    IEnumerator extend(GameObject obj)
    {
        Debug.Log("Extend start");
        for (float i = 0; i <= 1.0f; i += Time.fixedDeltaTime)
        {
            transform.position = transform.position + new Vector3(0.0f, 0.0f, 0.01f);
            yield return null;
        } 

        float gravity = Physics.gravity.magnitude;
        float initialVelocity = Mathf.Sqrt(2 * gravity * springJumpUnit);
        float force = rigid.mass * initialVelocity + 0.5f;
        
        Debug.Log("Jump start");
        obj.GetComponent<Rigidbody>().AddForce(Vector3.forward * force , ForceMode.Impulse);
        isIdle = true;
    }

    void springAction(GameObject obj)
    {
        isIdle = false;
        StartCoroutine(compress(obj));
    }
}
