using _Game._Scripts.Managers;
using _Game._Scripts.UI.Player;
using Fusion;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace _Game._Scripts.UI.Gameplay
{
    public class FinishPanel : MonoBehaviour
    {
        [Header("**References**")]
        [SerializeField] private TextMeshProUGUI _winnerPlayer_txt;
        [SerializeField] private RectTransform _otherPlayerResultElementParent;
        [SerializeField] private OtherPlayerResultElement _otherPlayerResultElement;

        private List<OtherPlayerResultElement> _spawnedElements = new();

        public void SetWinnerPlayer(string text)
        {
            _winnerPlayer_txt.text = text;
        }

        public void SetOtherPlayersResult(List<PlayerInfoElement> playerInfoElements)
        {
            foreach (var item in playerInfoElements)
            {
                if (_spawnedElements.Any(x => x.Nickname == item.Nickname)) return;
                OtherPlayerResultElement resultElement = Instantiate(_otherPlayerResultElement, _otherPlayerResultElementParent);
                resultElement.Initialize(item.Nickname, item.Timer);
                _spawnedElements.Add(resultElement);
            }
        }

    }
}
