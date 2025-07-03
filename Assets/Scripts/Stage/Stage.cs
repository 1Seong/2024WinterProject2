using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Stage : MonoBehaviour
{
    private StageData data;

    private Transform bottomWall;
    private Transform topWall;
    private Transform[] innerWall; // There can be multiple Inner walls

    private Transform projectionWallParentXY;
    private Transform projectionWallParentXZ;

    private Transform restrictionSide;
    private Transform restrictionTop;

    public GameObject player1;
    public GameObject player2;

    private bool isActing = false;
    private bool restrict = false;

    public List<Movable> defaultMovables;
    public List<Movable> invertMovables;

    public static event Action stageStartEvent;
    public static event Action convertEvent;
    public static event Action convertEventLast;

    private void Awake()
    {
        StageManager.instance.stage = this;

        stageStartEvent += Init;
        stageStartEvent += ObjectReposition;
        stageStartEvent += MakeProjection;

        convertEvent += CallCameraRotate;
        convertEvent += ProjectionSetting;
    }

    private void OnDestroy()
    {
        stageStartEvent -= Init;
        stageStartEvent -= ObjectReposition;
        stageStartEvent -= MakeProjection;

        convertEvent -= CallCameraRotate;
        convertEvent -= ProjectionSetting;
    }

    private void Start()
    {
        stageStartEvent?.Invoke();
    }

    private void Init()
    {
        /*
         * Stage Hierarchy structure
         * - 0. Bottom Wall
         * - 1. Top Wall
         * - 2. Side Wall
         * - 3. Inner Wall
         * - 4. Background Wall
         * - 5. Projection Wall XY
         * - 6. Projection Wall XZ
         * - 7. Restriction Side
         * - 8. Restriction Top
         */
        data = StageManager.instance.currentStageInfo.data;
        GameManager.instance.isSideView = false;

        bottomWall = transform.GetChild(0);
        topWall = transform.GetChild(1);
        innerWall = transform.GetChild(3).GetComponentsInChildren<Transform>();
        projectionWallParentXY = transform.GetChild(5);
        projectionWallParentXZ = transform.GetChild(6);
        restrictionSide = transform.GetChild(7);
        restrictionTop = transform.GetChild(8);
    }

    private void MakeProjection()
    {
        CreateWall(true);
        CreateWall(false);

        if (data.player2Exist)
        {
            CreateWall(true);
            
        }

        ProjectionSetting();
    }

    private void Update()
    {
        // Check convert condition
        CheckConvert();
    }

    private void CheckConvert()
    {
        /*
         * Check convert condition
         */
        // Convert viewpoint when press 'E' and players should be on bottom platform
        if (!data.conversionActive || isActing || restrict) return;

        if (Input.GetKeyDown(KeyCode.E) && !player1.GetComponent<PlayerJump>().isJumping)
        {
            Debug.Log("E key down");
            if (data.player2Exist)
            {
                if (!player2.GetComponent<PlayerJump>().isJumping)
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
        GameManager.instance.isSideView = !GameManager.instance.isSideView;

        convertEvent?.Invoke();
        convertEventLast?.Invoke();
    }

    private void ProjectionSetting()
    {
        if (GameManager.instance.isSideView) // Top view -> Side view
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
    }

    private void CallCameraRotate()
    {
        StartCoroutine(CameraRotate());
    }

    IEnumerator CameraRotate()
    {
        /*
         * Rotate Camera and make bottom and top wall transparent
         */
        bool sideview = GameManager.instance.isSideView;
        float targetRot = sideview ? -90.0f : 90.0f;
        float totalTime = GameManager.instance.cameraRotationTime;

        isActing = true;

        for (float i = 0; i <= totalTime; i += Time.fixedDeltaTime)
        {
            //Camera Rotation
            Camera.main.transform.RotateAround(new Vector3(7, 2, 4), Vector3.right, targetRot / ((totalTime / Time.fixedDeltaTime) + 1));

            yield return new WaitForFixedUpdate(); // Wait for a fixed delta time
        }

        isActing = false;
    }

    void CreateWall(bool onXY)
    {
        /*
         * Project inner walls onto XY, XZ plane
         */
        if (innerWall == null || StageManager.instance.wallPrefab == null) 
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
            wallMesh.Optimize();
            //MeshUtility.Optimize(wallMesh);

            // Create new Object
            GameObject wall = Instantiate(StageManager.instance.wallPrefab);
            wall.tag = "Inner";

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

            Collider coll = wall.AddComponent<MeshCollider>();
            coll.material = StageManager.instance.physicsMat;
        }
    }

    void AddThickness(Mesh mesh, float thickness, bool isOnXY)
    {
        Vector3 targetVec = isOnXY ? Vector3.back : Vector3.up;

        Vector3[] originalVertices = mesh.vertices;
        int[] originalTriangles = mesh.triangles;

        int vertexCount = originalVertices.Length;
        int triangleCount = originalTriangles.Length;

        // 1. 두께 적용된 새로운 정점 생성
        Vector3[] newVertices = new Vector3[vertexCount * 2];
        for (int i = 0; i < vertexCount; i++)
        {
            newVertices[i] = originalVertices[i]; // Front
            newVertices[i + vertexCount] = originalVertices[i] + targetVec * thickness; // Back
        }

        // 2. 앞면과 뒷면 삼각형
        List<int> newTriangles = new List<int>();

        // 앞면: 원래와 동일
        for (int i = 0; i < triangleCount; i++)
            newTriangles.Add(originalTriangles[i]);

        // 뒷면: 정점 인덱스를 뒤집고, 뒤쪽 정점들을 가리키도록 오프셋
        for (int i = 0; i < triangleCount; i += 3)
        {
            newTriangles.Add(originalTriangles[i] + vertexCount);
            newTriangles.Add(originalTriangles[i + 2] + vertexCount);
            newTriangles.Add(originalTriangles[i + 1] + vertexCount);
        }

        // 3. side face 생성 – edge 중복 없이 처리
        HashSet<(int, int)> addedEdges = new HashSet<(int, int)>();

        void AddSideFace(int i0, int i1)
        {
            var edge = (Math.Min(i0, i1), Math.Max(i0, i1));
            if (addedEdges.Contains(edge)) return;
            addedEdges.Add(edge);

            int i0Back = i0 + vertexCount;
            int i1Back = i1 + vertexCount;

            // Triangle 1
            newTriangles.Add(i0);
            newTriangles.Add(i1Back);
            newTriangles.Add(i1);

            // Triangle 2
            newTriangles.Add(i0);
            newTriangles.Add(i0Back);
            newTriangles.Add(i1Back);
        }

        for (int i = 0; i < triangleCount; i += 3)
        {
            int i0 = originalTriangles[i];
            int i1 = originalTriangles[i + 1];
            int i2 = originalTriangles[i + 2];

            AddSideFace(i0, i1);
            AddSideFace(i1, i2);
            AddSideFace(i2, i0);
        }

        // 4. 적용
        mesh.Clear();
        mesh.vertices = newVertices;
        mesh.triangles = newTriangles.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    private void ReposProjection()
    {
        /*
         * Reposition z-axis of projection walls on XY plane
         */
        Transform[] projectionsXY = projectionWallParentXY.GetComponentsInChildren<Transform>();

        if (projectionsXY.Length == 1) return;

        Transform player1pos = player1.transform;
        Transform player2pos = player2.transform;

        int wallNum = (projectionsXY.Length - 1) / 2;

        for (int i = 1; i < wallNum + 1; i++)
        {
            // for player 1
            projectionsXY[i].position = new Vector3(projectionsXY[i].position.x, projectionsXY[i].position.y, player1pos.position.z);

            // for player 2
            projectionsXY[i + wallNum].position = new Vector3(projectionsXY[i+ wallNum].position.x, projectionsXY[i + wallNum].position.y, player2pos.position.z);
          
        }
    }

    private void ObjectReposition()
    {
        //please add item reposition logic
        player1 = Instantiate(GameManager.instance.player1, data.startPos1, Quaternion.identity);
        player1.SetActive(true);
        if (data.player2Exist)
        {
            player2 = Instantiate(GameManager.instance.player2, data.startPos2, Quaternion.identity);
            player2.SetActive(true);
        }
            
        /*
        if (GameManager.instance.isSideView) // Top view -> Side view
        {
            restrictionSide.gameObject.SetActive(true);
            restrictionTop.gameObject.SetActive(false);
        }
        else // Side view -> Top view
        {
            restrictionSide.gameObject.SetActive(false);
            restrictionTop.gameObject.SetActive(true);
        }
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Movable>() != null)
        {
            restrict = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Movable>() != null)
        {
            restrict = false;
        }
    }
}
