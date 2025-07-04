using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public List<Movable> movables;

    public static event Action stageStartEvent;
    public static event Action convertEvent;
    public static event Action convertEventLast;

    private void Awake()
    {
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
        StageManager.instance.stage = this;

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
        //if (!data.conversionActive || isActing || restrict) return;

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

            MeshCollider coll = wall.AddComponent<MeshCollider>();
            coll.material = StageManager.instance.physicsMat;
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

        // Add front and back vertices
        for (int i = 0; i < originalVertices.Length; i++)
        {
            newVertices[i] = originalVertices[i]; // Front vertices
            newVertices[i + originalVertices.Length] = originalVertices[i] + targetVec * thickness; // Back vertices
        }

        // Copy original triangles for front and back faces
        int[] originalTriangles = mesh.triangles;
        int[] newTriangles = new int[originalTriangles.Length * 2 + originalVertices.Length * 6];

        // Front face triangles (unchanged)
        for (int i = 0; i < originalTriangles.Length; i++)
        {
            newTriangles[i] = originalTriangles[i];
        }

        // Back face triangles (reversed to maintain correct normals)
        for (int i = 0; i < originalTriangles.Length; i += 3)
        {
            newTriangles[originalTriangles.Length + i] = originalTriangles[i] + originalVertices.Length;
            newTriangles[originalTriangles.Length + i + 1] = originalTriangles[i + 2] + originalVertices.Length;
            newTriangles[originalTriangles.Length + i + 2] = originalTriangles[i + 1] + originalVertices.Length;
        }

        // Add side faces
        int offset = originalTriangles.Length * 2;
        for (int i = 0; i < originalVertices.Length; i++)
        {
            int next = (i + 1) % originalVertices.Length;

            if (i < next)
            {
                // Triangle 1
                newTriangles[offset++] = i;
                newTriangles[offset++] = i + originalVertices.Length;
                newTriangles[offset++] = next;

                // Triangle 2
                newTriangles[offset++] = next;
                newTriangles[offset++] = i + originalVertices.Length;
                newTriangles[offset++] = next + originalVertices.Length;
            }
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

        if (projectionsXY.Length == 1) return;

        Transform player1pos = player1.transform;

        projectionsXY[1].position = new Vector3(projectionsXY[1].position.x, projectionsXY[1].position.y, player1pos.position.z);

        if (data.player2Exist)
        {
            Transform player2pos = player2.transform;
            projectionsXY[2].position = new Vector3(projectionsXY[1].position.x, projectionsXY[1].position.y, player2pos.position.z);
        }
    }

    private void ObjectReposition()
    {
        //please add item reposition logic
        player1 = Instantiate(GameManager.instance.player1, data.startPos1, Quaternion.identity);
        
        if (data.player2Exist)
            player2 = Instantiate(GameManager.instance.player2, data.startPos2, Quaternion.identity);

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
