using System;
using System.Collections;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [Header("Stage Info")]
    public int startPosX1;
    public int startPosZ1;
    public int startPosX2;
    public int startPosZ2;

    public float invertLineZ;

    public GameObject wallPrefab;
    public PhysicsMaterial pmat;

    private Transform bottomWall;
    private Transform topWall;
    private Transform[] innerWall;

    private void Start()
    {
        bottomWall = transform.GetChild(0).GetChild(0);
        topWall = transform.GetChild(1).GetChild(0);
        innerWall = transform.GetChild(4).GetComponentsInChildren<Transform>();

        CreateWall();
    }

    public void CallCameraRotate()
    {
        StartCoroutine(CameraRotate());
    }

    IEnumerator CameraRotate()
    {
        bool topview = GameManager.instance.isTopView;
        float targetRot = topview ? -90.0f : 90.0f;
        float totalTime = GameManager.instance.cameraRotationTime;

        Material mat1 = bottomWall.GetComponent<MeshRenderer>().material;
        Material mat2 = topWall.GetComponent<MeshRenderer>().material;

        if (!topview)
        {
            bottomWall.gameObject.SetActive(true);
            topWall.gameObject.SetActive(true);
        }
        for (float i = 0; i <= totalTime; i += Time.fixedDeltaTime)
        {
            //Camera Rotation
            Camera.main.transform.RotateAround(new Vector3(0, 2, 2), Vector3.right, targetRot / ((totalTime / Time.fixedDeltaTime) + 1));

            //Wall Transparency
            Color color = mat1.color;
            float amount = Mathf.Lerp(0f, 1f, i / totalTime);
            if (topview)
                amount = 1f - amount;

            color.a = amount;
            mat1.color = color;
            mat2.color = color;

            yield return new WaitForFixedUpdate();
        }
        if (topview)
        {
            bottomWall.gameObject.SetActive(false);
            topWall.gameObject.SetActive(false);
        }
    }

    void CreateWall()
    {
        if (innerWall == null || wallPrefab == null) 
            return;

        for(int i = 1; i != innerWall.Length; ++i)
        {
            // Get mesh component
            MeshFilter targetMeshFilter = innerWall[i].GetComponent<MeshFilter>();

            if (targetMeshFilter == null) 
                return;

            Mesh targetMesh = targetMeshFilter.mesh;
            Vector3[] vertices = targetMesh.vertices;

            // Project vertices into planes
            Vector3[] projectedVerticesXY = new Vector3[vertices.Length];
            Vector3[] projectedVerticesXZ = new Vector3[vertices.Length];

            for (int j = 0; j != vertices.Length; ++j)
            {
                Vector3 worldVertex = innerWall[i].TransformPoint(vertices[j]);
                projectedVerticesXY[j] = new Vector3(worldVertex.x, worldVertex.y, 0);
                projectedVerticesXZ[j] = new Vector3(worldVertex.x, 0, worldVertex.z);
            }

            // Create new mesh
            Mesh wallMeshXY = new Mesh();
            Mesh wallMeshXZ = new Mesh();
            wallMeshXY.vertices = projectedVerticesXY;
            wallMeshXY.triangles = targetMesh.triangles;
            wallMeshXY.RecalculateBounds();
            wallMeshXY.RecalculateNormals();

            wallMeshXZ.vertices = projectedVerticesXZ;
            wallMeshXZ.triangles = targetMesh.triangles;
            wallMeshXZ.RecalculateBounds();
            wallMeshXZ.RecalculateNormals();

            AddThickness(wallMeshXY, 0.2f, true);
            AddThickness(wallMeshXZ, 0.2f, false);

            // Create new Object
            GameObject wallXY = Instantiate(wallPrefab);
            wallXY.transform.position = Vector3.zero;
            wallXY.transform.rotation = Quaternion.identity;

            GameObject wallXZ = Instantiate(wallPrefab);
            wallXZ.transform.position = Vector3.zero;
            wallXZ.transform.rotation = Quaternion.identity;

            // Apply mesh to object
            MeshFilter wallMeshFilterXY = wallXY.GetComponent<MeshFilter>();
            wallMeshFilterXY.mesh = wallMeshXY;

            MeshFilter wallMeshFilterXZ = wallXZ.GetComponent<MeshFilter>();
            wallMeshFilterXZ.mesh = wallMeshXZ;

            MeshCollider collXY = wallXY.AddComponent<MeshCollider>();
            collXY.material = pmat;

            MeshCollider collXZ = wallXZ.AddComponent<MeshCollider>();
            collXZ.material = pmat;
        }
    }

    void AddThickness(Mesh mesh, float thickness, bool isOnXY)
    {
        Vector3 targetVec = isOnXY ? Vector3.back : Vector3.up;

        // copy original vertices
        Vector3[] originalVertices = mesh.vertices;
        Vector3[] newVertices = new Vector3[originalVertices.Length * 2];

        // add front, back triangles
        for (int i = 0; i < originalVertices.Length; i++)
        {
            newVertices[i] = originalVertices[i];                    // front vertices
            newVertices[i + originalVertices.Length] = originalVertices[i] + targetVec * thickness; // back vertices
        }

        // copy new triangles
        int[] originalTriangles = mesh.triangles;
        int[] newTriangles = new int[originalTriangles.Length * 2 + originalVertices.Length * 6];

        // add front, back triangles
        for (int i = 0; i < originalTriangles.Length; i++)
        {
            newTriangles[i] = originalTriangles[i]; // front triangles
            newTriangles[i + originalTriangles.Length] = originalTriangles[i] + originalVertices.Length; // back triangles
        }

        // create side faces
        int offset = originalTriangles.Length * 2;
        for (int i = 0; i < originalVertices.Length; i++)
        {
            int next = (i + 1) % originalVertices.Length; //next point

            // create two triangles
            newTriangles[offset++] = i; // front
            newTriangles[offset++] = next + originalVertices.Length; // next back
            newTriangles[offset++] = next; // next front

            newTriangles[offset++] = i; // front
            newTriangles[offset++] = i + originalVertices.Length; // front back
            newTriangles[offset++] = next + originalVertices.Length; // next back
        }

        mesh.vertices = newVertices;
        mesh.triangles = newTriangles;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
}
