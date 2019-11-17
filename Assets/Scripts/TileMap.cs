using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TileMap : MonoBehaviour
{
    public int size_x = 100;
    public int size_z = 100;
    public float tileSize = 1f;
    
    public int tileResolution = 16;
    public Texture2D textureAtlas;
    
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private MeshCollider _meshCollider;

    void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshCollider = GetComponent<MeshCollider>();
        InitTileMap();
    }

    public void InitTileMap()
    {
        BuildMap();
        BuildTexture();
    }

    public Vector3[] GetRectVerticesByPoint(Vector3 positionVector)
    {
        Vector2Int tileByPoint = GetTileByPoint(positionVector);
        TileMapGenerator tileMapGenerator = new TileMapGenerator(size_x, size_z, tileSize);
        Mesh sharedMesh = _meshFilter.sharedMesh;
        return tileMapGenerator.GetTrianglesOfATile(tileByPoint, sharedMesh.vertices);
    }
    
    public Vector2Int GetTileByPoint(Vector3 positionVector)
    {
        return new Vector2Int(Mathf.FloorToInt(positionVector.x / tileSize),
            Mathf.FloorToInt(positionVector.z / tileSize));
    }

    public void BuildMap()
    {
        TileMapGenerator tileMapGenerator = new TileMapGenerator(size_x, size_z, tileSize);

        Mesh mesh = tileMapGenerator.GenerateMap();
        _meshFilter.mesh = mesh;
        _meshCollider.sharedMesh = mesh;
    }
    
    private void BuildTexture()
    {
        TileMapTextureGenerator tileMapTextureGenerator = new TileMapTextureGenerator(textureAtlas, tileResolution, size_x, size_z);
        Texture2D texture = tileMapTextureGenerator.GenerateTexture();
        _meshRenderer.sharedMaterial.mainTexture = texture;
        
        Debug.Log("Texture built!");
    }


}
