using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Fusion;
using Fusion.Sockets;

// Spawns player car prefabs when players join the session and assigns input authority.
public class PlayerSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkPrefabRef playerCarPrefab;
    public float separation = 3.0f;

    // Optional camera follow component in scene (set at runtime when a local car is spawned)
    private CameraFollow cameraFollow;

    private void Awake()
    {
        // Use the newer API to avoid obsolete warning when available
    #if UNITY_2023_1_OR_NEWER
    cameraFollow = UnityEngine.Object.FindFirstObjectByType<CameraFollow>();
    #else
    cameraFollow = FindObjectOfType<CameraFollow>();
    #endif
    }

    // Keep track of spawned players locally on the server
    private HashSet<PlayerRef> _spawnedPlayers = new HashSet<PlayerRef>();
    private bool _gameStarted = false;

    void INetworkRunnerCallbacks.OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        // Only server/host decides when to spawn the players
        if (runner.IsServer == false) return;

        var players = runner.ActivePlayers.ToList();

        // Don't start the match until at least 2 players are connected
        if (!_gameStarted)
        {
            if (players.Count < 2)
            {
                // Wait for more players to join
                return;
            }

            // Spawn all currently connected players at once, separated along X
            for (int i = 0; i < players.Count; i++)
            {
                var p = players[i];
                if (_spawnedPlayers.Contains(p))
                    continue;

                Vector3 spawnPos = new Vector3(i * separation, 0f, 0f);
                runner.Spawn(playerCarPrefab, spawnPos, Quaternion.identity, p);
                _spawnedPlayers.Add(p);
            }

            _gameStarted = true;
        }
        else
        {
            // If the game already started, spawn any late joiner individually
            if (!_spawnedPlayers.Contains(player))
            {
                int index = _spawnedPlayers.Count;
                Vector3 spawnPos = new Vector3(index * separation, 0f, 0f);
                runner.Spawn(playerCarPrefab, spawnPos, Quaternion.identity, player);
                _spawnedPlayers.Add(player);
            }
        }
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }

    void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer == false) return;

        // Remove from spawned set and if players < 2, mark game not started so match is paused
        _spawnedPlayers.Remove(player);
        var players = runner.ActivePlayers.ToList();
        if (_gameStarted && players.Count < 2)
        {
            _gameStarted = false;
            // Note: you may want to notify clients or handle pausing logic here.
        }
    }

    void INetworkRunnerCallbacks.OnSceneLoadDone(NetworkRunner runner) { }

    #region Unused

    void INetworkRunnerCallbacks.OnObjectExitAOI(NetworkRunner                runner, NetworkObject  obj, PlayerRef player) { }
    void INetworkRunnerCallbacks.OnObjectEnterAOI(NetworkRunner               runner, NetworkObject  obj, PlayerRef player) { }
    
    void INetworkRunnerCallbacks.OnInput(NetworkRunner                        runner, NetworkInput   input)                      { }
    void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner                 runner, PlayerRef      player, NetworkInput input) { }
    void INetworkRunnerCallbacks.OnShutdown(NetworkRunner                     runner, ShutdownReason shutdownReason) { }
    void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner            runner)                                                                                        { }
    void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner       runner, NetDisconnectReason reason)                                                            { }
    void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner               runner, NetworkRunnerCallbackArgs.ConnectRequest request,       byte[]                 token)  { }
    void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner                runner, NetAddress                               remoteAddress, NetConnectFailedReason reason) { }
    void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner        runner, SimulationMessagePtr                     message)                         { }
    void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner           runner, List<SessionInfo>                        sessionList)                     { }
    void INetworkRunnerCallbacks.OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object>               data)                            { }
    void INetworkRunnerCallbacks.OnHostMigration(NetworkRunner                runner, HostMigrationToken                       hostMigrationToken)              { }
    void INetworkRunnerCallbacks.OnReliableDataReceived(NetworkRunner         runner, PlayerRef                                player, ReliableKey key, ArraySegment<byte> data) { }
    void INetworkRunnerCallbacks.OnSceneLoadStart(NetworkRunner               runner) { }

    #endregion
}
