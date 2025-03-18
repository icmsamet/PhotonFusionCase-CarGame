using Fusion;
using UnityEngine;
using _Game._Scripts.UI.Player;
using System.Collections.Generic;

namespace _Game._Scripts.Managers
{
    public class UIManager : MonoBehaviour
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

        private Dictionary<PlayerRef, PlayerInfoElement> _spawnedElements = new Dictionary<PlayerRef, PlayerInfoElement>();

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
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (_spawnedElements.TryGetValue(player, out PlayerInfoElement element))
            {
                Destroy(element);
                _spawnedElements.Remove(player);
            }
        }

        private void SpawnPlayerElement(NetworkRunner runner, PlayerRef player)
        {
            if (!_spawnedElements.ContainsKey(player))
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

                _spawnedElements.Add(player, element);
            }
        }
    }
}