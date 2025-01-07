using System;
using System.Collections;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [Header("Stage Info")]
    public int startPosX1; // Start position of player 1
    public int startPosZ1;
    public int startPosX2; // Start position of player 2
    public int startPosZ2;

    public float invertLineZ; // Z line used for player inversion

    public GameObject wallPrefab;
    public PhysicsMaterial pmat; // Physics material (No friction)

    private Transform bottomWall;
    private Transform topWall;
    private Transform[] innerWall; // There can be multiple Inner walls

    private void Start()
    {
        /*
         * Start
         */
        /*
         * Stage Hierarchy structure
         * - 0. Bottom Wall
         * - 1. Top Wall
         * - 2. Left Wall
         * - 3. Right Wall
         * - 4. Inner Wall
         * - 5. Background Wall
         */
        bottomWall = transform.GetChild(0).GetChild(0);
        topWall = transform.GetChild(1).GetChild(0);
        innerWall = transform.GetChild(4).GetComponentsInChildren<Transform>();

        CreateWall();
    }

    public void CallCameraRotate()
    {
        /*
         * Start Camera Rotation Coroutine
         */
        StartCoroutine(CameraRotate());
    }

    IEnumerator CameraRotate()
    {
        /*
         * Rotate Camera and make bottom and top wall transparent
         */
        bool topview = GameManager.instance.isTopView;
        float targetRot = topview ? -90.0f : 90.0f;
        float totalTime = GameManager.instance.cameraRotationTime;

        Material mat1 = bottomWall.GetComponent<MeshRenderer>().material;
        Material mat2 = topWall.GetComponent<MeshRenderer>().material;

        if (!topview) // Side view -> Top view
        {
            bottomWall.gameObject.SetActive(true);
            topWall.gameObject.SetActive(true);
        }
        for (float i = 0; i <= totalTime; i += Time.fixedDeltaTime)
        {
            //Camera Rotation
            Camera.main.transform.RotateAround(new Vector3(0, 2, 2), Vector3.right, targetRot / ((totalTime / Time.fixedDeltaTime) + 1));

            //Wall Transparency Control
            Color color = mat1.color;
            float amount = Mathf.Lerp(0f, 1f, i / totalTime);
            if (topview)
                amount = 1f - amount;

            color.a = amount;
            mat1.color = color;
            mat2.color = color;

            yield return new WaitForFixedUpdate(); // Wait for a fixed delta time
        }
        if (topview) // Top view -> Side view
        {
            bottomWall.gameObject.SetActive(false);
            topWall.gameObject.SetActive(false);
        }
    }

    void CreateWall()
    {
        /*
         * Project inner walls onto XY, XZ plane
         */
        if (innerWall == null || wallPrefab == null) 
            return;

        for(int i = 1; i != innerWall.Length; ++i) // iterate for every inner walls
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

            // Add thickness to mesh
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
        /*
         * Add thickness to the mesh
         */
        Vector3 targetVec = isOnXY ? Vector3.back : Vector3.up;

        // Copy original vertices
        Vector3[] originalVertices = mesh.vertices;
        Vector3[] newVertices = new Vector3[originalVertices.Length * 2];

        // Add front, back triangles
        for (int i = 0; i < originalVertices.Length; i++)
        {
            newVertices[i] = originalVertices[i];                    // Front vertices
            newVertices[i + originalVertices.Length] = originalVertices[i] + targetVec * thickness; // Back vertices
        }

        // Copy new triangles
        int[] originalTriangles = mesh.triangles;
        int[] newTriangles = new int[originalTriangles.Length * 2 + originalVertices.Length * 6];

        // Add front, back triangles
        for (int i = 0; i < originalTriangles.Length; i++)
        {
            newTriangles[i] = originalTriangles[i]; // Front triangles
            newTriangles[i + originalTriangles.Length] = originalTriangles[i] + originalVertices.Length; // Back triangles
        }

        // Create side faces
        int offset = originalTriangles.Length * 2;
        for (int i = 0; i < originalVertices.Length; i++)
        {
            int next = (i + 1) % originalVertices.Length; // Next point

            // Create two triangles
            newTriangles[offset++] = i; // Front
            newTriangles[offset++] = next + originalVertices.Length; // Next back
            newTriangles[offset++] = next; // Next front

            newTriangles[offset++] = i; // Front
            newTriangles[offset++] = i + originalVertices.Length; // Back
            newTriangles[offset++] = next + originalVertices.Length; // Next back
        }

        // Apply to mesh
        mesh.vertices = newVertices;
        mesh.triangles = newTriangles;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
}
