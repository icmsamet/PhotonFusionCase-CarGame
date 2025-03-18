using Fusion;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace _Game._Scripts.Managers
{
    public class SpawnManager : NetworkBehaviour
    {
        #region Instance
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

        private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

        private void Start()
        {
            Application.targetFrameRate = 60;
        }
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, _spawnPoints[runner.ActivePlayers.Count()-1].position, Quaternion.identity, player);
                _spawnedCharacters.Add(player, networkPlayerObject);
            }
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            runner.Shutdown();
            if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
            {
                runner.Despawn(networkObject);
                _spawnedCharacters.Remove(player);
            }
        }

        #region Get

        public NetworkObject GetLocalNetworkObject()
        {
            foreach (var entry in _spawnedCharacters)
            {
                PlayerRef player = entry.Key;
                NetworkObject networkObject = entry.Value;
                if (player == Runner.LocalPlayer)
                {
                    return networkObject;
                }
            }
            return null;
        }


        #endregion

        #region Properties

        public Dictionary<PlayerRef, NetworkObject> SpawnedCharacters => _spawnedCharacters;

        #endregion
    }
}