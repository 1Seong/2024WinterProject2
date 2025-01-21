using System;
using System.Collections;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [Header("Stage Info")]
    public int startPosX1; // Start position of player 1
    public int startPosZ1;

    public bool player2Exist; // Does player 2 exist in this stage?

    public int startPosX2; // Start position of player 2
    public int startPosZ2;

    public float invertLineZ; // Z line used for player inversion

    public GameObject wallPrefab;
    public PhysicsMaterial pmat; // Physics material (No friction)

    private Transform bottomWall;
    private Transform topWall;
    private Transform[] innerWall; // There can be multiple Inner walls

    private Transform projectionWallParentXY;
    private Transform projectionWallParentXZ;

    private Transform stage; // Stage의 Transform 참조

    public GameObject frogItemPrefab; // 개구리 아이템 프리팹
    public GameObject swapItemPrefab; // 스왑 아이템 프리팹

    private void OnEnable()
    {
        /*
         * Start
         *
         *
         * Stage Hierarchy structure
         * - 0. Bottom Wall
         * - 1. Top Wall
         * - 2. Left Wall
         * - 3. Right Wall
         * - 4. Inner Wall
         * - 5. Background Wall
         * - 6. Projection Wall XY
         * - 7. Projection Wall XZ
         */
        bottomWall = transform.GetChild(0).GetChild(0);
        topWall = transform.GetChild(1).GetChild(0);
        innerWall = transform.GetChild(4).GetComponentsInChildren<Transform>();
        projectionWallParentXY = transform.GetChild(6);
        projectionWallParentXZ = transform.GetChild(7);
        
        // GameManager에서 현재 스테이지 Transform 가져오기
        stage = GameManager.instance.currentStage.transform;

        // 플레이어 상태 초기화
        GameManager.instance.player1.ResetPlayer();
        if (GameManager.instance.player2 != null)
        {
            GameManager.instance.player2.ResetPlayer();
        }

        // 아이템 배치
        if (frogItemPrefab != null)
        {
            Vector3 spawnPosition = new Vector3(1, 0, 1); // 위치 설정
            Instantiate(frogItemPrefab, spawnPosition, Quaternion.identity, transform);
        }

        // 스왑 아이템 배치
        if (swapItemPrefab != null)
        {
            Vector3 swapItemPosition = new Vector3(0, 0, 4); // 위치 설정
            Instantiate(swapItemPrefab, swapItemPosition, Quaternion.identity, transform);
        }

        CreateWall(true);
        CreateWall(false);

        if (player2Exist)
        {
            CreateWall(true);
            CreateWall(false);
        }

        if(GameManager.instance.isTopView)
            projectionWallParentXY.gameObject.SetActive(false);
        else
            projectionWallParentXZ.gameObject.SetActive(false);
    }

    private void Update()
    {
        /*
         * Update
         */
        // Check convert condition
        CheckConvert();
    }

    private void CheckConvert()
    {
        /*
         * Check convert condition
         */
        // Convert viewpoint when press 'E' and players should be on bottom platform
        Player player1 = GameManager.instance.player1;
        Player player2 = null;

        if(player2Exist)
            player2 = GameManager.instance.player2;

        if (Input.GetKeyDown(KeyCode.E) && !player1.isJumping && player1.onBottom)
        {
            if (player2Exist)
            {
                if (!player2.isJumping && player2.onBottom)
                    ConvertView();
            }
            else
                ConvertView();
        }
    }

    private void ConvertView()
    {
        /*
         * Convert Viewpoint
         */
        bool topview = GameManager.instance.isTopView;

        // Camera setting
        StartCoroutine(CameraRotate());

        // Projection wall setting
        if (GameManager.instance.isTopView) // Top view -> Side view
        {
            projectionWallParentXY.gameObject.SetActive(true);
            projectionWallParentXZ.gameObject.SetActive(false);
            ReposProjection();
        }
        else // Side view -> Top view
        {
            projectionWallParentXY.gameObject.SetActive(false);
            projectionWallParentXZ.gameObject.SetActive(true);
        }

        // Player physics setting
        GameManager.instance.player1.ConversionPhysicsSetting();
        if(player2Exist)
            GameManager.instance.player2.ConversionPhysicsSetting();

        GameManager.instance.isTopView = !topview;
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

    void CreateWall(bool onXY)
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
            Vector3[] projectedVertices = new Vector3[vertices.Length];

            for (int j = 0; j != vertices.Length; ++j)
            {
                Vector3 worldVertex = innerWall[i].TransformPoint(vertices[j]);
                projectedVertices[j] = onXY ? new Vector3(worldVertex.x, worldVertex.y, 0)  : new Vector3(worldVertex.x, 0, worldVertex.z);
            }

            // Create new mesh
            Mesh wallMesh = new Mesh();
    
            wallMesh.vertices = projectedVertices;
            wallMesh.triangles = targetMesh.triangles;
            wallMesh.RecalculateBounds();
            wallMesh.RecalculateNormals();

            // Add thickness to mesh
            AddThickness(wallMesh, 0.2f, onXY);

            // Create new Object
            GameObject wall = Instantiate(wallPrefab);

            if(onXY)
                wall.transform.SetParent(projectionWallParentXY);
            else
                wall.transform.SetParent(projectionWallParentXZ);

            wall.transform.position = Vector3.zero;
            wall.transform.rotation = Quaternion.identity;

            // Apply mesh to object
            MeshFilter wallMeshFilterXY = wall.GetComponent<MeshFilter>();
            wallMeshFilterXY.mesh = wallMesh;

            MeshRenderer mesher = wall.GetComponent<MeshRenderer>();
            mesher.enabled = false;

            MeshCollider coll = wall.AddComponent<MeshCollider>();
            coll.material = pmat;
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

    private void ReposProjection()
    {
        /*
         * Reposition z-axis of projection walls on XY plane
         */
        Transform[] projectionsXY = projectionWallParentXY.GetComponentsInChildren<Transform>();
        Transform player1pos = GameManager.instance.player1.transform;

        projectionsXY[1].position = new Vector3(projectionsXY[1].position.x, projectionsXY[1].position.y, player1pos.position.z);

        if (player2Exist)
        {
            Transform player2pos = GameManager.instance.player2.transform;
            projectionsXY[2].position = new Vector3(projectionsXY[1].position.x, projectionsXY[1].position.y, player2pos.position.z);
        }
    }
}
