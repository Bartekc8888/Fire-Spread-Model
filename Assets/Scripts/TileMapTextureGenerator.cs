using System.Collections.Generic;
using UnityEngine;

public class TileMapTextureGenerator
{
    private readonly Texture2D _textureAtlas;
    private readonly int _tileResolution;
    private readonly int _tileSizeX;
    private readonly int _tileSizeZ;

    public TileMapTextureGenerator(Texture2D textureAtlas, int tileResolution, int tileSizeX, int tileSizeZ)
    {
        _textureAtlas = textureAtlas;
        _tileResolution = tileResolution;
        _tileSizeX = tileSizeX;
        _tileSizeZ = tileSizeZ;
    }

    public Texture2D GenerateTexture()
    {
        List<Color[]> extractedTextures = ExtractTexturesFromAtlas();
        return GenerateTexture(extractedTextures);
    }
    
    private List<Color[]> ExtractTexturesFromAtlas()
    {
        int tilesInARow = _textureAtlas.width / _tileResolution;
        int tileRowsCount = _textureAtlas.height / _tileResolution;

        List<Color[]> extractedTextures = new List<Color[]>(tileRowsCount * tilesInARow);
        for (int row = 0; row < tileRowsCount; row++)
        {
            for (int tileInARow = 0; tileInARow < tilesInARow; tileInARow++)
            {
                int x = tileInARow * _tileResolution;
                int y = row * _tileResolution;
                extractedTextures.Add(_textureAtlas.GetPixels(x, y, _tileResolution, _tileResolution));
            }
        }

        return extractedTextures;
    }

    private Texture2D GenerateTexture(IReadOnlyList<Color[]> extractedTextures)
    {
        int textureWidth = _tileResolution * _tileSizeX;
        int textureHeight = _tileResolution * _tileSizeZ;
        Texture2D texture = new Texture2D(textureWidth, textureHeight);
        
        for (int y = 0; y < _tileSizeZ; y++)
        {
            for (int x = 0; x < _tileSizeX; x++)
            {
                Color[] extractedTexture = extractedTextures[Random.Range(0, extractedTextures.Count)];
                texture.SetPixels(x * _tileResolution, y * _tileResolution, _tileResolution, _tileResolution, extractedTexture);
            }
        }

        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Point;
        texture.Apply();
        return texture;
    }
}