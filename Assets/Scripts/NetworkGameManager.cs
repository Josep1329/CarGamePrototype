using UnityEngine;
using Fusion;
using System.Linq;

public class NetworkGameManager : SimulationBehaviour
{
    [Header("Prefabs")]
    public GameObject playerCarPrefab;

    // Spawn logic moved to PlayerSpawner (INetworkRunnerCallbacks) to avoid duplicate spawns

    private Vector3 GetSpawnPosition()
    {
        // Separar los jugadores en el eje X según su índice en ActivePlayers
        int playerIndex = 0;
        var players = Runner.ActivePlayers.ToList();
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == Object.InputAuthority)
            {
                playerIndex = i;
                break;
            }
        }
        float separation = 3.0f; // distancia entre jugadores
        return new Vector3(playerIndex * separation, 0, 0);
    }
}
