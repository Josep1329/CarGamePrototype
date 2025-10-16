using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Simple main menu created at runtime: session name input + Host / Join buttons
[DisallowMultipleComponent]
public class MainMenuUI : MonoBehaviour
{
    private GameObject _canvasGO;
    private TMP_InputField _sessionInput;
    private Button _hostButton;
    private Button _joinButton;

    private FusionLauncher _launcher;

    private void Awake()
    {
    #if UNITY_2023_1_OR_NEWER
    _launcher = UnityEngine.Object.FindFirstObjectByType<FusionLauncher>();
    #else
    _launcher = FindObjectOfType<FusionLauncher>();
    #endif

        // Build minimal UI
    _canvasGO = new GameObject("MainMenuCanvas", typeof(RectTransform));
        var canvas = _canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        _canvasGO.AddComponent<CanvasScaler>();
        _canvasGO.AddComponent<GraphicRaycaster>();

    var panelGO = new GameObject("MenuPanel", typeof(RectTransform));
    panelGO.transform.SetParent(_canvasGO.transform, false);
        var image = panelGO.AddComponent<Image>();
        image.color = new Color(0f, 0f, 0f, 0.6f);
        var rect = panelGO.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(500, 200);
        rect.anchoredPosition = Vector2.zero;

        // Session input
    var inputGO = new GameObject("SessionInput", typeof(RectTransform));
    inputGO.transform.SetParent(panelGO.transform, false);
        _sessionInput = inputGO.AddComponent<TMP_InputField>();
    var textGO = new GameObject("Text", typeof(RectTransform));
    textGO.transform.SetParent(inputGO.transform, false);
        var tmp = textGO.AddComponent<TextMeshProUGUI>();
        tmp.text = "";
        tmp.fontSize = 24;
        _sessionInput.textComponent = tmp;
        var inputRect = inputGO.GetComponent<RectTransform>();
        inputRect.anchorMin = new Vector2(0.1f, 0.6f);
        inputRect.anchorMax = new Vector2(0.9f, 0.85f);
        inputRect.sizeDelta = Vector2.zero;
        _sessionInput.text = "CarRaceSession";

        // Host button
    var hostGO = new GameObject("HostButton", typeof(RectTransform));
    hostGO.transform.SetParent(panelGO.transform, false);
        _hostButton = hostGO.AddComponent<Button>();
    var hostTextGO = new GameObject("HostText", typeof(RectTransform));
    hostTextGO.transform.SetParent(hostGO.transform, false);
        var hostText = hostTextGO.AddComponent<TextMeshProUGUI>();
        hostText.text = "Host";
        hostText.alignment = TextAlignmentOptions.Center;
        var hostRect = hostGO.GetComponent<RectTransform>();
        hostRect.anchorMin = new Vector2(0.1f, 0.1f);
        hostRect.anchorMax = new Vector2(0.45f, 0.5f);
        hostRect.sizeDelta = Vector2.zero;

        // Join button
    var joinGO = new GameObject("JoinButton", typeof(RectTransform));
    joinGO.transform.SetParent(panelGO.transform, false);
        _joinButton = joinGO.AddComponent<Button>();
    var joinTextGO = new GameObject("JoinText", typeof(RectTransform));
    joinTextGO.transform.SetParent(joinGO.transform, false);
        var joinText = joinTextGO.AddComponent<TextMeshProUGUI>();
        joinText.text = "Join";
        joinText.alignment = TextAlignmentOptions.Center;
        var joinRect = joinGO.GetComponent<RectTransform>();
        joinRect.anchorMin = new Vector2(0.55f, 0.1f);
        joinRect.anchorMax = new Vector2(0.9f, 0.5f);
        joinRect.sizeDelta = Vector2.zero;

        _hostButton.onClick.AddListener(OnHostClicked);
        _joinButton.onClick.AddListener(OnJoinClicked);
    }

    private void Update()
    {
        if (_launcher == null)
        {
            #if UNITY_2023_1_OR_NEWER
            _launcher = UnityEngine.Object.FindFirstObjectByType<FusionLauncher>();
            #else
            _launcher = FindObjectOfType<FusionLauncher>();
            #endif
            return;
        }

        // Hide menu when runner is running
        if (_launcher.Runner != null && _launcher.Runner.IsRunning)
        {
            _canvasGO.SetActive(false);
        }
        else
        {
            _canvasGO.SetActive(true);
        }
    }

    private void OnHostClicked()
    {
        if (_launcher == null) return;
        _launcher.StartHost(_sessionInput.text);
    }

    private void OnJoinClicked()
    {
        if (_launcher == null) return;
        _launcher.StartClient(_sessionInput.text);
    }
}
