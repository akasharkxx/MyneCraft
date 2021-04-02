using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public Material material;
    public BlockType[] blockTypes;

}

[Serializable]
public class BlockType
{
    public string blockName;
    public bool isSolid;

    [Header("Texture Values")]
    public int backFaceTexture;
    public int frontFaceTexture;
    public int topFaceTexture;
    public int bottomFaceTexture;
    public int leftFaceTexture;
    public int rightFaceTexture;

    // Back, Front, Top, Bottom, Left, Right

    public int GetTextureID(int faceIndex)
    {
        switch(faceIndex)
        {
            case 0:
                return backFaceTexture;
            case 1:
                return backFaceTexture;
            case 2:
                return backFaceTexture;
            case 3:
                return backFaceTexture;
            case 4:
                return backFaceTexture;
            case 5:
                return backFaceTexture;
            case 6:
                return backFaceTexture;
            default:
                Debug.Log("Error in GetTexureID, invalid face index.");
                return 0;
        }   
    }
}
