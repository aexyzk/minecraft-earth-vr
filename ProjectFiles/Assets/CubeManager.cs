using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    public GameObject cube;
    public int width;
    public int height;
    public Material material;
    public BlockType[] blockTypes;
    public Transform playerPos;

    void Start()
    {
        SpawnBaseplate();
        playerPos.position = new Vector3(width / 2, playerPos.position.y, width / 2);
    }

    void SpawnBaseplate() {
        for(int y = 0; y < height; y++) {
            for(int x = 0; x < width; x++) {
                for(int z = 0; z < width; z++) {
                    SpawnCube(Mathf.FloorToInt(x),Mathf.FloorToInt(y),Mathf.FloorToInt(z), true);
                }
            }
        }
    }

    public void SpawnCube(int x, int y, int z, bool shouldMoveDownSome)
    {
        GameObject newCube;

        if (shouldMoveDownSome)
        {
            newCube = Instantiate(cube, new Vector3(x, y - height, z), Quaternion.identity);
        }
        else
        {
            newCube = Instantiate(cube, new Vector3(x, y, z), Quaternion.identity);
        }

        newCube.transform.SetParent(this.transform.parent);

        Renderer cubeRenderer = newCube.GetComponent<Renderer>();
        if (cubeRenderer != null)
        {
            Texture2D tex = null;
            SpriteToTexture2D(blockTypes[1].texture, ref tex);
            cubeRenderer.material.mainTexture = tex;
        }
        else
        {
            Debug.LogError("Cube does not have a Renderer component!");
        }
    }
    void SpriteToTexture2D(Sprite spriteToConvert, ref Texture2D tex) {
        tex = new Texture2D((int)spriteToConvert.rect.width, (int)spriteToConvert.rect.height);
        tex.filterMode = FilterMode.Point;
        tex.SetPixels(spriteToConvert.texture.GetPixels(
            (int)spriteToConvert.textureRect.x,
            (int)spriteToConvert.textureRect.y,
            (int)spriteToConvert.textureRect.width,
            (int)spriteToConvert.textureRect.height));
        tex.Apply();
    }
}

[System.Serializable]
public class BlockType {
    public string blockName;
    public Sprite texture;
    public int soundType;
    public bool breakable;
}