using UnityEngine;

public class MapTexture : MonoBehaviour
{
    MeshFilter cubeMesh;
    Mesh mesh;
    void Start()
    {
        cubeMesh = GetComponent<MeshFilter>();
        mesh = cubeMesh.mesh;
        Vector2[] uvMap = mesh.uv;

        //ORDER: front, top, back, bottom, left, right
        //front
        uvMap[0] = new Vector2(0.250f, 0.333f);
        uvMap[1] = new Vector2(0.500f, 0.333f);
        uvMap[2] = new Vector2(0.500f, 0.25f);
        uvMap[3] = new Vector2(0.500f, 0.50f);
        //top
        uvMap[4] = new Vector2(0.25f, 0.333f);
        uvMap[5] = new Vector2(0.666f, 0.333f);
        uvMap[8] = new Vector2(0.334f, 0);
        uvMap[9] = new Vector2(0.666f, 0);
        //back
        uvMap[6] = new Vector2(1, 0);
        uvMap[7] = new Vector2(0.667f, 0);
        uvMap[10] = new Vector2(1, 0.333f);
        uvMap[11] = new Vector2(0.667f, 0.333f);
        //bottom
        uvMap[12] = new Vector2(0, 0.334f);
        uvMap[13] = new Vector2(0, 0.666f);
        uvMap[14] = new Vector2(0.333f, 0.666f);
        uvMap[15] = new Vector2(0.333f, 0.334f);
        //left
        uvMap[16] = new Vector2(0.334f, 0.334f);
        uvMap[17] = new Vector2(0.334f, 0.666f);
        uvMap[18] = new Vector2(0.666f, 0.666f);
        uvMap[19] = new Vector2(0.666f, 0.334f);
        //right
        uvMap[20] = new Vector2(0.667f, 0.334f);
        uvMap[21] = new Vector2(0.667f, 0.666f);
        uvMap[22] = new Vector2(1, 0.666f);
        uvMap[23] = new Vector2(1, 0.334f);

        mesh.uv = uvMap;
    }
}
