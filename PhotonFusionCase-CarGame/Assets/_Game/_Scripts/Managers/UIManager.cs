using Fusion;
using UnityEngine;
using UnityEngine.UI;
using _Game._Scripts.UI.Player;
using System.Collections.Generic;

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
        [SerializeField] private GameObject _gameplayPanel;
        [SerializeField] private Button _hostStart_btn;

        private GameManager _gameManager => GameManager.Instance;

        private Dictionary<PlayerRef, PlayerInfoElement> _elements = new Dictionary<PlayerRef, PlayerInfoElement>();

        private void Start()
        {
            _hostStart_btn.onClick.AddListener(HostStartButton);
            //
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
            if (_elements.TryGetValue(player, out PlayerInfoElement element))
            {
                Destroy(element);
                _elements.Remove(player);
            }
        }

        private void SpawnPlayerElement(NetworkRunner runner, PlayerRef player)
        {
            if (!_elements.ContainsKey(player))
            {
                PlayerInfoElement element = Instantiate(_elementPrefab, _elementParent);
                element.Initialize(player.PlayerId.ToString());

                if (player == runner.LocalPlayer)
                {
                    element.SetNicknameColor(Color.cyan);
                }
                else
                {
                    element.SetNicknameColor(Color.white);
                }

                _elements.Add(player, element);
            }
        }

        #region Button Actions

        private void HostStartButton()
        {
            _gameManager.onGameStartAction?.Invoke();
        }

        #endregion

        #region Actions

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
        }

        #endregion
    }
}