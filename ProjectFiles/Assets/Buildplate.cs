using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Buildplate : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    int vertexIndex = 0;
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector2> uvs = new List<Vector2>();

    bool [,,] voxelMap = new bool[VoxelData.baseplateWidth, VoxelData.baseplateHeight, VoxelData.baseplateWidth];

    void Start()
    {
        PoplulateVoxelMap();
        CreateMeshData();
        CreateMesh();
    }

    void PoplulateVoxelMap() {
        for (int y = 0; y < VoxelData.baseplateHeight; y++){
            for (int x = 0; x < VoxelData.baseplateWidth; x++){
                for (int z = 0; z < VoxelData.baseplateWidth; z++){
                    voxelMap[x,y,z] = true;
                }
            }
        }
    }

    void CreateMeshData() {
        for (int y = 0; y < VoxelData.baseplateHeight; y++){
            for (int x = 0; x < VoxelData.baseplateWidth; x++){
                for (int z = 0; z < VoxelData.baseplateWidth; z++){
                    AddVoxelDataToBaseplate(new Vector3(x,y,z));
                }
            }
        }
    }

    bool CheckVoxel(Vector3 pos) {
        int x = Mathf.FloorToInt(pos.x);
        int y = Mathf.FloorToInt(pos.y);
        int z = Mathf.FloorToInt(pos.z);

        if (x < 0 || x > VoxelData.baseplateWidth -1 || y < 0 || y > VoxelData.baseplateHeight - 1 || z < 0 || z > VoxelData.baseplateWidth - 1) {
            return false;
        }

        return voxelMap [x,y,z];
    }

    void AddVoxelDataToBaseplate(Vector3 pos) {
        for (int p  = 0; p < 6; p++) {
            if (!CheckVoxel(pos + VoxelData.faceChecks[p])) {
                vertices.Add (pos + VoxelData.voxelVerts[VoxelData.voxelTris [p, 0]]);
                vertices.Add (pos + VoxelData.voxelVerts[VoxelData.voxelTris [p, 1]]);
                vertices.Add (pos + VoxelData.voxelVerts[VoxelData.voxelTris [p, 2]]);
                vertices.Add (pos + VoxelData.voxelVerts[VoxelData.voxelTris [p, 3]]);
                uvs.Add (VoxelData.voxelUvs [0]);
                uvs.Add (VoxelData.voxelUvs [1]);
                uvs.Add (VoxelData.voxelUvs [2]);
                uvs.Add (VoxelData.voxelUvs [3]);
                triangles.Add (vertexIndex);
                triangles.Add (vertexIndex + 1);
                triangles.Add (vertexIndex + 2);
                triangles.Add (vertexIndex + 2);
                triangles.Add (vertexIndex + 1);
                triangles.Add (vertexIndex + 3);
                vertexIndex += 4;
            }

        }
    }

    void CreateMesh() {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }
}
