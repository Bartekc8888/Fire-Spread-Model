using System;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.UI;

public class TileTypesPanel : MonoBehaviour
{
    public GameObject TerrainTypesPanel;
    public GameObject buttonPrefab;

    public TileMap TileMap;

    private List<GameObject> _terrainButtons = new List<GameObject>();

    private GameObject _currentlySelectedButton;
    public TerrainType CurrentlySelectedTerrainType { get; private set; }

    private GameObject _buttonHighlight;
    
    void Start()
    {
        GenerateButtonsForTileTypes();
    }

    public bool IsButtonSelected()
    {
        return _currentlySelectedButton != null;
    }

    private void GenerateButtonsForTileTypes()
    {
        TerrainType[] terrainTypesValues = (TerrainType[])Enum.GetValues(typeof(TerrainType));
        
        TileMapTextureGenerator tileMapTextureGenerator = TileMap.GetTileMapTextureGenerator();
        List<Color[]> texturesFromAtlas = tileMapTextureGenerator.ExtractTexturesFromAtlas();

        RectTransform parentRectTransform = TerrainTypesPanel.transform.GetComponent<RectTransform>();
        foreach (TerrainType terrainTypesValue in terrainTypesValues)
        {
            Texture2D texture2D = GetTextureForTerrainType(tileMapTextureGenerator, terrainTypesValue, texturesFromAtlas);
            GameObject button = CreateButton(terrainTypesValue, texture2D);
            _terrainButtons.Add(button);

            RectTransform buttonRectTransform = button.transform.GetComponent<RectTransform>();
            float rectWidth = buttonRectTransform.rect.width / terrainTypesValues.Length;
            int buttonOffset = Array.IndexOf(terrainTypesValues, terrainTypesValue);
            button.transform.localPosition = new Vector3(rectWidth * 2 + (rectWidth * 8 * buttonOffset),
                (-parentRectTransform.rect.height / 2), 0);
        }

        SetPanelWidth(terrainTypesValues, parentRectTransform);
    }

    private void SetPanelWidth(TerrainType[] terrainTypesValues, RectTransform parentRectTransform)
    {
        GameObject terrainButton = _terrainButtons[0];
        RectTransform buttonRect = terrainButton.transform.GetComponent<RectTransform>();
        Rect rect = buttonRect.rect;
        float maxWidth = (rect.width * 2 * terrainTypesValues.Length);
        Rect parentRect = parentRectTransform.rect;
        parentRectTransform.sizeDelta = new Vector2(maxWidth, parentRect.height);
    }

    private GameObject CreateButton(TerrainType terrainTypesValue, Texture2D texture2D)
    {
        GameObject button = Instantiate(buttonPrefab, TerrainTypesPanel.transform, false);
        button.GetComponent<Button>().onClick.AddListener(() => HandleButtonClick(button, terrainTypesValue));

        Image component = button.transform.GetChild(0).GetComponent<Image>();
        component.sprite = Sprite.Create(texture2D,
            new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
        return button;
    }

    private Texture2D GetTextureForTerrainType(TileMapTextureGenerator tileMapTextureGenerator,
        TerrainType terrainTypesValue, List<Color[]> texturesFromAtlas)
    {
        int terrainIndex = tileMapTextureGenerator.MapTerrainToTexture(terrainTypesValue);
        Color[] texturePixels = texturesFromAtlas[terrainIndex];

        Texture2D texture2D = new Texture2D(TileMap.tileResolution, TileMap.tileResolution);
        texture2D.SetPixels(texturePixels);
        texture2D.Apply();
        return texture2D;
    }

    private void HandleButtonClick(GameObject clickedButton, TerrainType terrainTypesValue)
    {
        if (clickedButton != _currentlySelectedButton)
        {
            Button button = clickedButton.GetComponent<Button>();
            button.Select();
            _currentlySelectedButton = clickedButton;
            CurrentlySelectedTerrainType = terrainTypesValue;
            AddBackgroundHighlightToButton(clickedButton);
//            SetPanelColor(new Color(0.6f, 0.2f, 0f, 0.4f));
            SetPanelColor(new Color(1f, 1f, 1f, 0.8f));
            Debug.Log("Button clicked");
        }
        else
        {
            _currentlySelectedButton = null;
            RemoveHighlight();
            SetPanelColor(new Color(1f, 1f, 1f, 0.4f));
            Debug.Log("Button unselected");
        }
    }

    private void AddBackgroundHighlightToButton(GameObject parentButton)
    {
        RectTransform rectTransform = parentButton.transform.GetComponent<RectTransform>();
        Rect rect = rectTransform.rect;

        GameObject imageGameObject = new GameObject();
        imageGameObject.transform.SetParent(parentButton.transform);

        Image image = imageGameObject.AddComponent<Image>();
        image.color = new Color(0.2f, 0.0f, 1.0f, 0.3f);

        Vector3 pos = new Vector3(rect.size.x / 2, 0, -1);
        imageGameObject.transform.localPosition = pos;
        imageGameObject.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(rect.size.x / 1.5f, rect.size.y / 1.5f);

        RemoveHighlight();
        _buttonHighlight = imageGameObject;
    }

    private void SetPanelColor(Color color)
    {
        Image component = TerrainTypesPanel.GetComponent<Image>();
        component.color = color;
    }

    private void RemoveHighlight()
    {
        if (_buttonHighlight != null)
        {
            Destroy(_buttonHighlight);
            _buttonHighlight = null;
        }
    }
}
