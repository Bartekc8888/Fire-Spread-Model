using Data;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TileMap : MonoBehaviour
{
    public float tileSize = 1f;
    
    public int tileResolution = 16;
    public Texture2D textureAtlas;
    
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private MeshCollider _meshCollider;

    private TileMapData _tileMapData;
    private int _sizeX;
    private int _sizeY;

    public void InitTileMap(int sizeX, int sizeY, TileMapData tileMapData)
    {
        GrabComponents();
        
        _sizeX = sizeX;
        _sizeY = sizeY;
        _tileMapData = tileMapData;
        
        BuildMap();
        BuildTexture();
    }
    
    void GrabComponents()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshCollider = GetComponent<MeshCollider>();
    }

    public Vector3[] GetRectVerticesByPoint(Vector3 positionVector)
    {
        Vector2Int tileByPoint = GetTileByPoint(positionVector);
        TileMapGenerator tileMapGenerator = new TileMapGenerator(_sizeX, _sizeY, tileSize);
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
        TileMapGenerator tileMapGenerator = new TileMapGenerator(_sizeX, _sizeY, tileSize);

        Mesh mesh = tileMapGenerator.GenerateMap();
        _meshFilter.mesh = mesh;
        _meshCollider.sharedMesh = mesh;
    }
    
    private void BuildTexture()
    {
        TileMapTextureGenerator tileMapTextureGenerator = new TileMapTextureGenerator(textureAtlas, tileResolution, _sizeX, _sizeY);
        Texture2D texture = tileMapTextureGenerator.GenerateTexture(_tileMapData);
        _meshRenderer.sharedMaterial.mainTexture = texture;
        
        Debug.Log("Texture built!");
    }


}
