using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;

public class FusionLauncher : MonoBehaviour
{
    private NetworkRunner _runner;
    private NetworkSceneManagerDefault _sceneManager;

    private async void Start()
    {
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;
        _sceneManager = gameObject.GetComponent<NetworkSceneManagerDefault>();
        if (_sceneManager == null)
        {
            _sceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>();
        }
        await _runner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.AutoHostOrClient,
            SessionName = "CarRaceSession",
            Scene = SceneRef.FromIndex(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex),
            SceneManager = _sceneManager
        });
    }
}
