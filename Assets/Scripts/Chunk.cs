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

    byte[,,] voxelMap = new byte[VoxelData.ChunkWidth, VoxelData.ChunkHeight, VoxelData.ChunkWidth];

    World world;

    private void Start()
    {
        world = GameObject.Find("World").GetComponent<World>();

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
                    voxelMap[x, y, z] = 2;
                    //if(y < 1)
                    //{
                    //    voxelMap[x, y, z] = 0;
                    //}
                    //else if(y == VoxelData.ChunkHeight - 1)
                    //{
                    //    voxelMap[x, y, z] = 2;
                    //}
                    //else
                    //{
                    //    voxelMap[x, y, z] = 1;
                    //}

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

        return world.blockTypes[voxelMap[x, y, z]].isSolid;
    }

    private void AddVoxelDataToChunk(Vector3 pos)
    {
        for (int j = 0; j < 6; j++)
        {
            if(!CheckVoxel(pos+ VoxelData.faceChecks[j]))
            {
                byte blockID = voxelMap[(int)pos.x, (int)pos.y, (int)pos.z];

                vertices.Add(pos + VoxelData.voxelVertex[VoxelData.voxelTris[j, 0]]);
                vertices.Add(pos + VoxelData.voxelVertex[VoxelData.voxelTris[j, 1]]);
                vertices.Add(pos + VoxelData.voxelVertex[VoxelData.voxelTris[j, 2]]);
                vertices.Add(pos + VoxelData.voxelVertex[VoxelData.voxelTris[j, 3]]);

                AddTexture(world.blockTypes[blockID].GetTextureID(j));

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

    private void AddTexture(int textureID)
    {
        float y = textureID / VoxelData.TextureAtlasSizeInBlocks;
        float x = textureID - (y * VoxelData.TextureAtlasSizeInBlocks);

        x *= VoxelData.NormalizedBlockTextureSize;
        y *= VoxelData.NormalizedBlockTextureSize;

        //If starting from top
        y = 1f - y - VoxelData.NormalizedBlockTextureSize;

        uvs.Add(new Vector2(x, y));
        uvs.Add(new Vector2(x, y + VoxelData.NormalizedBlockTextureSize));
        uvs.Add(new Vector2(x + VoxelData.NormalizedBlockTextureSize, y));
        uvs.Add(new Vector2(x + VoxelData.NormalizedBlockTextureSize, y + VoxelData.NormalizedBlockTextureSize));
    }
}
