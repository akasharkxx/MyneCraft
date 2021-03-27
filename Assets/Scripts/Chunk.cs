using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    int vertexIndex = 0;
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector2> uvs = new List<Vector2>();

    bool[,,] voxelMap = new bool[VoxelData.ChunkWidth, VoxelData.ChunkHeight, VoxelData.ChunkWidth];

    private void Start()
    {
        PopulateMap();

        CreateMeshData();

        CreateMesh();
    }

    private void PopulateMap()
    {
        for (int y = 0; y < VoxelData.ChunkHeight; y++)
        {
            for (int x = 0; x < VoxelData.ChunkWidth; x++)
            {
                for (int z = 0; z < VoxelData.ChunkWidth; z++)
                {
                    voxelMap[x, y, z] = true;
                }
            }
        }
    }

    private void CreateMeshData()
    {
        for (int y = 0; y < VoxelData.ChunkHeight; y++)
        {
            for (int x = 0; x < VoxelData.ChunkWidth; x++)
            {
                for (int z = 0; z < VoxelData.ChunkWidth; z++)
                {
                    AddVoxelDataToChunk(new Vector3(x, y, z));
                }
            }
        }
    }

    private bool CheckVoxel(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x);
        int y = Mathf.FloorToInt(pos.y);
        int z = Mathf.FloorToInt(pos.z);

        if (x < 0 || x > VoxelData.ChunkWidth - 1 ||
            y < 0 || y > VoxelData.ChunkHeight - 1 || 
            z < 0 || z > VoxelData.ChunkWidth - 1)
        {
            return false;
        }

        return voxelMap[x, y, z];
    }

    private void AddVoxelDataToChunk(Vector3 pos)
    {
        for (int j = 0; j < 6; j++)
        {
            if(!CheckVoxel(pos+ VoxelData.faceChecks[j]))
            {
                vertices.Add(pos + VoxelData.voxelVertex[VoxelData.voxelTris[j, 0]]);
                vertices.Add(pos + VoxelData.voxelVertex[VoxelData.voxelTris[j, 1]]);
                vertices.Add(pos + VoxelData.voxelVertex[VoxelData.voxelTris[j, 2]]);
                vertices.Add(pos + VoxelData.voxelVertex[VoxelData.voxelTris[j, 3]]);

                uvs.Add(VoxelData.voxelUVs[0]);
                uvs.Add(VoxelData.voxelUVs[1]);
                uvs.Add(VoxelData.voxelUVs[2]);
                uvs.Add(VoxelData.voxelUVs[3]);

                triangles.Add(vertexIndex);
                triangles.Add(vertexIndex + 1);
                triangles.Add(vertexIndex + 2);
                triangles.Add(vertexIndex + 2);
                triangles.Add(vertexIndex + 1);
                triangles.Add(vertexIndex + 3);
                vertexIndex += 4;
            }   
        }
    }

    private void CreateMesh()
    {
        Mesh _mesh = new Mesh();
        _mesh.vertices = vertices.ToArray();
        _mesh.triangles = triangles.ToArray();
        _mesh.uv = uvs.ToArray();

        _mesh.RecalculateNormals();

        meshFilter.mesh = _mesh;
    }

    
}
