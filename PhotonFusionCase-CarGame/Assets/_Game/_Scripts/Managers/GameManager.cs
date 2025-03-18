using Fusion;
using System.Linq;
using UnityEngine.Events;
using _Game._Scripts.UI.Player;
using System.Collections.Generic;
using _Game._Scripts.UI.Gameplay;

namespace _Game._Scripts.Managers
{
    /// <summary>
    /// Manages the game flow, including start and end events, player progress tracking,
    /// and handling the finish line logic.
    /// Uses a singleton pattern for global access.
    /// </summary>

    public class GameManager : NetworkBehaviour
    {
        #region Instance
        /// <summary>
        /// Singleton instance of GameManager.
        /// </summary>
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

        /// <summary>
        /// Event triggered when the game ends.
        /// </summary>
        public UnityAction onGameEndAction = null;

        /// <summary>
        /// Event triggered when the game starts.
        /// </summary>
        public UnityAction onGameStartAction = null;

        private int _crossedLinePlayerCount = 0;
        private FinishPanel _finishPanel;

        private UIManager _uiManager => UIManager.Instance;
        private SpawnManager _spawnManager => SpawnManager.Instance;

        private void Start()
        {
            _finishPanel = _uiManager.FinishPanel;

            // Subscribe to game start and end actions
            onGameEndAction += OnGameEnd;
            onGameStartAction += OnGameStart;
        }

        #region Main

        /// <summary>
        /// Called when a player crosses the finish line.
        /// </summary>
        /// <param name="id">Player ID</param>
        public void FinishLineCrossed(int id)
        {
            RPC_FinishLineCrossed(id);
        }

        /// <summary>
        /// Handles player finish line crossing and checks if all players are finished.
        /// </summary>
        /// <param name="id">Player ID</param>
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
                if (_crossedLinePlayerCount == Runner.ActivePlayers.Count())
                {
                    onGameEndAction?.Invoke();
                }
            }
        }

        #endregion

        #region Actions

        /// <summary>
        /// Called when the game ends, disabling player movement and updating the results.
        /// </summary>
        private void OnGameEnd()
        {
            RPC_OnGameEnd();
        }

        /// <summary>
        /// Handles game end logic, stopping all players and displaying results.
        /// </summary>
        [Rpc]
        private void RPC_OnGameEnd()
        {
            foreach (var item in _spawnManager.SpawnedCharacters)
            {
                item.Value.GetComponent<Player.Player>().SetCanDrive(false);
            }

            List<PlayerInfoElement> sortedList = _uiManager.Elements.Values.OrderBy(e => e.Timer).ToList();
            _finishPanel.SetWinnerPlayer($"{sortedList[0].Nickname} \n {sortedList[0].Timer.ToString("F2")}");
            sortedList.RemoveAt(0);
            _finishPanel.SetOtherPlayersResult(sortedList);
        }

        /// <summary>
        /// Called when the game starts, allowing players to move.
        /// </summary>
        private void OnGameStart()
        {
            RPC_OnGameStart();
        }

        /// <summary>
        /// Handles game start logic, enabling player movement.
        /// </summary>
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