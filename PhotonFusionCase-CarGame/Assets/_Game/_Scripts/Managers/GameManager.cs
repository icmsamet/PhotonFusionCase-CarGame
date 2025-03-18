using Fusion;
using System.Linq;
using UnityEngine.Events;
using _Game._Scripts.UI.Player;

namespace _Game._Scripts.Managers
{
    public class GameManager : NetworkBehaviour
    {
        #region Instance
        private static GameManager _ins;
        public static GameManager Instance
        {
            get
            {
                if (!_ins)
                {
                    _ins = FindObjectOfType<GameManager>();
                }
                return _ins;
            }
        }
        #endregion

        public UnityAction onGameEndAction = null;
        public UnityAction onGameStartAction = null;

        private int _crossedLinePlayerCount = 0;

        private UIManager _uiManager => UIManager.Instance;
        private SpawnManager _spawnManager => SpawnManager.Instance;

        private void Start()
        {
            onGameEndAction += OnGameEnd;
            onGameStartAction += OnGameStart;
        }

        #region Main

        public void FinishLineCrossed(int id)
        {
            RPC_FinishLineCrossed(id);
        }

        [Rpc]
        private void RPC_FinishLineCrossed(int id)
        {
            if (_uiManager.Elements.TryGetValue(id, out PlayerInfoElement element))
            {
                element.StopCounting();
                if (_spawnManager.SpawnedCharacters.TryGetValue(id, out NetworkObject network))
                {
                    network.GetComponent<Player.Player>().SetCanDrive(false);
                }
                _crossedLinePlayerCount++;
                if(_crossedLinePlayerCount == Runner.ActivePlayers.Count())
                {
                    onGameEndAction?.Invoke();
                }
            }
        }

        #endregion

        #region Actions

        private void OnGameEnd()
        {
            RPC_OnGameEnd();
        }

        [Rpc]
        private void RPC_OnGameEnd()
        {
            foreach (var item in _spawnManager.SpawnedCharacters)
            {
                item.Value.GetComponent<Player.Player>().SetCanDrive(false);
            }
        }

        private void OnGameStart()
        {
            RPC_OnGameStart();
        }

        [Rpc]
        private void RPC_OnGameStart()
        {
            foreach (var item in _spawnManager.SpawnedCharacters)
            {
                item.Value.GetComponent<Player.Player>().SetCanDrive(true);
            }
        }

        #endregion
    }
}