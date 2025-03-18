using Fusion;
using UnityEngine;
using _Game._Scripts.UI.Player;
using System.Collections.Generic;
using _Game._Scripts.UI.Gameplay;

namespace _Game._Scripts.Managers
{
    public class UIManager : NetworkBehaviour
    {
        #region Instance
        /// <summary>
        /// Singleton instance of UIManager.
        /// </summary>
        private static UIManager _ins;
        public static UIManager Instance
        {
            get
            {
                if (!_ins)
                {
                    _ins = FindObjectOfType<UIManager>();
                }
                return _ins;
            }
        }
        #endregion

        [Header("**References**")]
        [SerializeField] private RectTransform _elementParent;
        [SerializeField] private PlayerInfoElement _elementPrefab;
        [SerializeField] private GameObject _hostPanel;
        [SerializeField] private GameObject _clientPanel;
        [SerializeField] private FinishPanel _finishPanel;
        [SerializeField] private GameObject _gameplayPanel;

        /// <summary>
        /// Stores player UI elements with their IDs.
        /// </summary>
        private Dictionary<int, PlayerInfoElement> _elements = new Dictionary<int, PlayerInfoElement>();

        private GameManager _gameManager => GameManager.Instance;

        private void Start()
        {
            _gameManager.onGameEndAction += OnGameEnd;
            _gameManager.onGameStartAction += OnGameStart;
        }

        #region Player

        /// <summary>
        /// Handles UI updates when a player joins.
        /// Spawns a player info element for each player.
        /// Updates host and client panel visibility.
        /// </summary>
        /// <param name="runner">Network session manager.</param>
        /// <param name="player">The joining player reference.</param>
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            foreach (PlayerRef _player in runner.ActivePlayers)
            {
                if (_player != runner.LocalPlayer)
                {
                    SpawnPlayerElement(runner, _player);
                }
            }

            SpawnPlayerElement(runner, runner.LocalPlayer);

            if (runner.IsServer)
            {
                _hostPanel.SetActive(true);
                _clientPanel.SetActive(false);
            }
            else
            {
                _hostPanel.SetActive(false);
                _clientPanel.SetActive(true);
            }
        }

        /// <summary>
        /// Handles UI cleanup when a player leaves.
        /// Removes their player info element.
        /// </summary>
        /// <param name="runner">Network session manager.</param>
        /// <param name="player">The leaving player reference.</param>
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (_elements.TryGetValue(player.PlayerId, out PlayerInfoElement element))
            {
                Destroy(element);
                _elements.Remove(player.PlayerId);
            }
        }

        /// <summary>
        /// Spawns a player info UI element.
        /// </summary>
        /// <param name="runner">Network session manager.</param>
        /// <param name="player">The player reference.</param>
        private void SpawnPlayerElement(NetworkRunner runner, PlayerRef player)
        {
            if (!_elements.ContainsKey(player.PlayerId))
            {
                PlayerInfoElement element = Instantiate(_elementPrefab, _elementParent);
                element.Initialize(player.PlayerId);

                if (player == runner.LocalPlayer)
                {
                    element.SetNicknameColor(Color.cyan);
                }
                else
                {
                    element.SetNicknameColor(Color.white);
                }

                _elements.Add(player.PlayerId, element);
            }
        }

        #endregion

        #region Actions

        /// <summary>
        /// Triggers when the game ends.
        /// Hides gameplay UI and shows the finish panel.
        /// </summary>
        private void OnGameEnd()
        {
            RPC_OnGameEnd();
        }

        [Rpc]
        private void RPC_OnGameEnd()
        {
            _gameplayPanel.SetActive(false);
            _finishPanel.gameObject.SetActive(true);
            foreach (var item in _elements)
            {
                item.Value.StopCounting();
            }
        }

        /// <summary>
        /// Triggers when the game starts.
        /// Hides pre-game UI and shows the gameplay panel.
        /// </summary>
        private void OnGameStart()
        {
            RPC_OnGameStart();
        }

        [Rpc]
        private void RPC_OnGameStart()
        {
            _hostPanel.SetActive(false);
            _clientPanel.SetActive(false);
            _gameplayPanel.SetActive(true);
            foreach (var item in _elements)
            {
                item.Value.StartCounting();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the FinishPanel UI reference.
        /// </summary>
        public FinishPanel FinishPanel => _finishPanel;

        /// <summary>
        /// Gets the dictionary of player UI elements.
        /// </summary>
        public Dictionary<int, PlayerInfoElement> Elements => _elements;

        #endregion

    }
}