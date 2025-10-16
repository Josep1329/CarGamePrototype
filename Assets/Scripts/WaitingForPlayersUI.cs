using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;

// Simple runtime UI that displays "Waiting for players..." when the local player has no spawned object yet.
[DisallowMultipleComponent]
public class WaitingForPlayersUI : MonoBehaviour
{
    private GameObject _canvasGO;
    private GameObject _panelGO;
    private TextMeshProUGUI _text;

    private NetworkRunner _runner;

    private void Awake()
    {
    #if UNITY_2023_1_OR_NEWER
    _runner = UnityEngine.Object.FindFirstObjectByType<NetworkRunner>();
    #else
    _runner = FindObjectOfType<NetworkRunner>();
    #endif

        // Create Canvas
        _canvasGO = new GameObject("WaitingCanvas");
        var canvas = _canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        _canvasGO.AddComponent<CanvasScaler>();
        _canvasGO.AddComponent<GraphicRaycaster>();

        // Create Panel
        _panelGO = new GameObject("WaitingPanel");
        _panelGO.transform.SetParent(_canvasGO.transform, false);
        var image = _panelGO.AddComponent<Image>();
        image.color = new Color(0f, 0f, 0f, 0.6f);
        var rect = _panelGO.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(400, 100);
        rect.anchoredPosition = Vector2.zero;

    // Create Text (TextMeshPro)
    var textGO = new GameObject("WaitingText");
    textGO.transform.SetParent(_panelGO.transform, false);
    _text = textGO.AddComponent<TextMeshProUGUI>();
    _text.alignment = TextAlignmentOptions.Center;
    _text.fontSize = 36;
    _text.text = "Waiting for players...";
    _text.color = Color.white;
    // Default wrapping behavior (controlled by RectTransform) will apply
    var textRect = textGO.GetComponent<RectTransform>();
    textRect.anchorMin = new Vector2(0, 0);
    textRect.anchorMax = new Vector2(1, 1);
    textRect.sizeDelta = Vector2.zero;
    }

    private void Update()
    {
        if (_runner == null)
        {
            #if UNITY_2023_1_OR_NEWER
            _runner = UnityEngine.Object.FindFirstObjectByType<NetworkRunner>();
            #else
            _runner = FindObjectOfType<NetworkRunner>();
            #endif
            if (_runner == null) return;
        }

        // If runner is running and local player has a spawned object, hide UI
        if (_runner.IsRunning)
        {
            var localPlayer = _runner.LocalPlayer;
            var playerObj = _runner.GetPlayerObject(localPlayer);
            bool hasObject = playerObj != null;
            _panelGO.SetActive(!hasObject);
        }
        else
        {
            // If runner not running, show waiting UI
            _panelGO.SetActive(true);
        }
    }
}
