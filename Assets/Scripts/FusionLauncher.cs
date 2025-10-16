using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;

public class FusionLauncher : MonoBehaviour
{
    private NetworkRunner _runner;
    private NetworkSceneManagerDefault _sceneManager;

    public NetworkRunner Runner => _runner;

    private void Awake()
    {
        // Prepare runner component but don't auto-start the game. Start is triggered by UI.
        _runner = gameObject.GetComponent<NetworkRunner>();
        if (_runner == null)
            _runner = gameObject.AddComponent<NetworkRunner>();

        _runner.ProvideInput = true;

        _sceneManager = gameObject.GetComponent<NetworkSceneManagerDefault>();
        if (_sceneManager == null)
            _sceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>();
    }

    public async void StartHost(string sessionName = "CarRaceSession")
    {
        if (_runner.IsRunning) return;

        await _runner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Host,
            SessionName = sessionName,
            Scene = SceneRef.FromIndex(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex),
            SceneManager = _sceneManager
        });
    }

    public async void StartClient(string sessionName = "CarRaceSession")
    {
        if (_runner.IsRunning) return;

        await _runner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Client,
            SessionName = sessionName,
            Scene = SceneRef.FromIndex(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex),
            SceneManager = _sceneManager
        });
    }
}
