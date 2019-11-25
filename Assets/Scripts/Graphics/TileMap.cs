using System;
using Data;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TileMap : MonoBehaviour
{
    public TileGuiInfo tileGuiInfo;
    public TileTypesPanel tileTypesPanel;
    public float tileSize = 1f;
    
    public int tileResolution = 16;
    public Texture2D textureAtlas;

    private Camera _mainCamera;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private MeshCollider _meshCollider;

    private TileMapData _tileMapData;
    private int _sizeX;
    private int _sizeY;
    
    void OnMouseDown()
    {
        TileData tileUnderMouse = GetTileUnderMouse();
        if (tileTypesPanel.IsButtonSelected())
        {
            TerrainType currentlySelectedTerrainType = tileTypesPanel.CurrentlySelectedTerrainType;
            tileUnderMouse.TerrainData.Type = currentlySelectedTerrainType;
            tileUnderMouse.TerrainData.MaterialProperties = MaterialPropertiesFactory.GetProperties(currentlySelectedTerrainType);
            
            UpdateTexture(tileUnderMouse);
        }
        else
        {
            tileGuiInfo.HandleMouseButtonClick(tileUnderMouse);
        }
    }

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
        _mainCamera = Camera.main;
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

        Mesh mesh = tileMapGenerator.GenerateMap(_tileMapData);
        _meshFilter.mesh = mesh;
        _meshCollider.sharedMesh = mesh;
    }
    
    private void BuildTexture()
    {
        TileMapTextureGenerator tileMapTextureGenerator = GetTileMapTextureGenerator();
        Texture2D texture = tileMapTextureGenerator.GenerateTexture(_tileMapData);
        _meshRenderer.sharedMaterial.mainTexture = texture;
        
        Debug.Log("Texture built!");
    }

    private void UpdateTexture(TileData tileUnderMouse)
    {
        TileMapTextureGenerator tileMapTextureGenerator = GetTileMapTextureGenerator();
        tileMapTextureGenerator.UpdateTexture(_meshRenderer.sharedMaterial.mainTexture as Texture2D, tileUnderMouse);
    }

    public TileMapTextureGenerator GetTileMapTextureGenerator()
    {
        TileMapTextureGenerator tileMapTextureGenerator =
            new TileMapTextureGenerator(textureAtlas, tileResolution, _sizeX, _sizeY);
        return tileMapTextureGenerator;
    }

    private TileData GetTileUnderMouse()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo = new RaycastHit();
        
        bool isTileHitWithRay = _meshCollider.Raycast(ray, out hitInfo, Mathf.Infinity);
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && isTileHitWithRay)
        {
            Vector3 hitPointPositionVector = gameObject.transform.InverseTransformPoint(hitInfo.point);
            Vector2Int tileByPoint = GetTileByPoint(hitPointPositionVector);
            return _tileMapData.GetTileData(tileByPoint.x, tileByPoint.y);
        }

        return null;
    }
}
