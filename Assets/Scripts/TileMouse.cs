using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(Renderer))]
public class TileMouse : MonoBehaviour
{
    public TileMap tileMap;

    private Collider _collider;
    private MeshFilter _meshFilter;
    
    private Camera _mainCamera;
    private Renderer _renderer;
    private bool _isTileMapNull;

    void Start()
    {
        _isTileMapNull = tileMap == null;
        if (_isTileMapNull)
        {
            Debug.Log("Tile mouse - Tile map is null");
        }
        else
        {
            _collider = tileMap.GetComponent<Collider>();
        }

        _mainCamera = Camera.main;
        _meshFilter = GetComponent<MeshFilter>();
        _renderer = GetComponent<Renderer>();
        _renderer.enabled = false;
    }
    
    void Update()
    {
        if (_isTileMapNull)
        {
            return;
        }
        
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo = new RaycastHit();
        
        bool isTileHitWithRay = _collider.Raycast(ray, out hitInfo, Mathf.Infinity);
        if (isTileHitWithRay)
        {
            Vector3 hitPointPositionVector = tileMap.gameObject.transform.InverseTransformPoint(hitInfo.point);
            Vector3[] rectVertices = tileMap.GetRectVerticesByPoint(hitPointPositionVector);
            if (rectVertices.Length < 4)
            {
                return;
            }

            Vector3 startPosition = tileMap.gameObject.transform.TransformPoint(rectVertices[0]);
            for (int i = 0; i < rectVertices.Length; i++)
            {
                rectVertices[i] = tileMap.gameObject.transform.TransformPoint(rectVertices[i].x, rectVertices[i].y + 0.2f, rectVertices[i].z);
                rectVertices[i] = new Vector3(rectVertices[i].x - startPosition.x,
                    rectVertices[i].y + 0.2f - startPosition.y, rectVertices[i].z - startPosition.z);
            }

            transform.position = startPosition;
            _meshFilter.mesh.vertices = rectVertices;
            
            _renderer.enabled = true;
        }
        else
        {
            _renderer.enabled = false;
        }
    }
}
