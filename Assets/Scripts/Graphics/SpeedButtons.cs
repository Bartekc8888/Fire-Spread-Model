using Simulation;
using UnityEngine;
using UnityEngine.UI;

public class SpeedButtons : MonoBehaviour
{
    public SimulationInstance simulationInstance;
    public GameObject stopButton;
    public GameObject startButton;
    public GameObject fasterButton;
    public GameObject fastestButton;

    private readonly Color _normalColor = Color.white; 
    private readonly Color _highlightColor = new Color(0.3686275f, 0.8235294f, 0.427451f); 
    
    void Start()
    {
        stopButton.GetComponent<Button>().onClick.AddListener(() => HandleButtonClick(stopButton, 0));
        startButton.GetComponent<Button>().onClick.AddListener(() => HandleButtonClick(startButton, 30000));
        fasterButton.GetComponent<Button>().onClick.AddListener(() => HandleButtonClick(fasterButton, 120000));
        fastestButton.GetComponent<Button>().onClick.AddListener(() => HandleButtonClick(fastestButton, 360000));

        simulationInstance.SetSimulationSpeed(1);
        HighlightButton(startButton);
    }

    private void HandleButtonClick(GameObject clickedButton, int gameSpeed)
    {
        ClearHighlight();
        
        simulationInstance.SetSimulationSpeed(gameSpeed);
        HighlightButton(clickedButton);
    }

    private void ClearHighlight()
    {
        stopButton.GetComponent<Button>().GetComponent<Image>().color = _normalColor;
        startButton.GetComponent<Button>().GetComponent<Image>().color = _normalColor;
        fasterButton.GetComponent<Button>().GetComponent<Image>().color = _normalColor;
        fastestButton.GetComponent<Button>().GetComponent<Image>().color = _normalColor;
    }

    private void HighlightButton(GameObject button)
    {
        button.GetComponent<Button>().GetComponent<Image>().color = _highlightColor;
    }
}