using Fusion;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace _Game._Scripts.Managers
{
    /// <summary>
    /// Manages player spawning and network synchronization.
    /// Uses a singleton pattern for global access.
    /// </summary>

    public class SpawnManager : NetworkBehaviour
    {
        #region Instance
        /// <summary>
        /// Singleton instance of SpawnManager.
        /// </summary>
        private static SpawnManager _ins;
        public static SpawnManager Instance
        {
            get
            {
                if (!_ins)
                {
                    _ins = FindObjectOfType<SpawnManager>();
                }
                return _ins;
            }
        }
        #endregion

        [Header("**References**")]
        [SerializeField] private NetworkPrefabRef _playerPrefab;
        [SerializeField] private List<Transform> _spawnPoints;

        /// <summary>
        /// Stores spawned player objects with their IDs.
        /// </summary>
        private Dictionary<int, NetworkObject> _spawnedCharacters = new Dictionary<int, NetworkObject>();

        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        #region Player

        /// <summary>
        /// Spawns a player when they join the game.
        /// Only the server is responsible for spawning players.
        /// </summary>
        /// <param name="runner">Network session manager.</param>
        /// <param name="player">The joining player reference.</param>
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, _spawnPoints[runner.ActivePlayers.Count() - 1].position, Quaternion.identity, player);
                networkPlayerObject.GetComponent<Player.Player>().ID = player.PlayerId;
                _spawnedCharacters.Add(player.PlayerId, networkPlayerObject);
            }
        }

        /// <summary>
        /// Handles player disconnection by removing their network object.
        /// </summary>
        /// <param name="runner">Network session manager.</param>
        /// <param name="player">The leaving player reference.</param>
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            runner.Shutdown();
            if (_spawnedCharacters.TryGetValue(player.PlayerId, out NetworkObject networkObject))
            {
                runner.Despawn(networkObject);
                _spawnedCharacters.Remove(player.PlayerId);
            }
        }

        #endregion

        #region Get

        /// <summary>
        /// Retrieves the local player's network object.
        /// </summary>
        /// <returns>The local player's NetworkObject, or null if not found.</returns>
        public NetworkObject GetLocalNetworkObject()
        {
            foreach (var entry in _spawnedCharacters)
            {
                int id = entry.Key;
                NetworkObject networkObject = entry.Value;
                if (id == Runner.LocalPlayer.PlayerId)
                {
                    return networkObject;
                }
            }
            return null;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Provides access to the dictionary of spawned players.
        /// </summary>
        public Dictionary<int, NetworkObject> SpawnedCharacters => _spawnedCharacters;

        #endregion

    }
}