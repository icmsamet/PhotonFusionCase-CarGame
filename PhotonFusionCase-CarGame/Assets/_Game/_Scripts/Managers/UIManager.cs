using Fusion;
using UnityEngine;
using UnityEngine.UI;
using _Game._Scripts.UI.Player;
using System.Collections.Generic;
using _Game._Scripts.UI.Gameplay;

namespace _Game._Scripts.Managers
{
    public class UIManager : NetworkBehaviour
    {
        #region Instance
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

        private Dictionary<int, PlayerInfoElement> _elements = new Dictionary<int, PlayerInfoElement>();

        private GameManager _gameManager => GameManager.Instance;

        private void Start()
        {
            _gameManager.onGameEndAction += OnGameEnd;
            _gameManager.onGameStartAction += OnGameStart;
        }

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

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (_elements.TryGetValue(player.PlayerId, out PlayerInfoElement element))
            {
                Destroy(element);
                _elements.Remove(player.PlayerId);
            }
        }

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

        #region Actions

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

        public FinishPanel FinishPanel => _finishPanel;

        public Dictionary<int, PlayerInfoElement> Elements => _elements;

        #endregion
    }
}