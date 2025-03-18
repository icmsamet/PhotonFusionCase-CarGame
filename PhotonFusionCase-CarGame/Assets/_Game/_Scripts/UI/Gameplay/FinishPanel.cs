using TMPro;
using System.Linq;
using UnityEngine;
using _Game._Scripts.UI.Player;
using System.Collections.Generic;

namespace _Game._Scripts.UI.Gameplay
{
    public class FinishPanel : MonoBehaviour
    {
        [Header("**References**")]
        [SerializeField] private TextMeshProUGUI _winnerPlayer_txt;
        [SerializeField] private RectTransform _otherPlayerResultElementParent;
        [SerializeField] private OtherPlayerResultElement _otherPlayerResultElement;

        private List<OtherPlayerResultElement> _spawnedElements = new();

        /// <summary>
        /// Sets the text for the winner player.
        /// </summary>
        /// <param name="text">The name of the winning player.</param>
        public void SetWinnerPlayer(string text)
        {
            _winnerPlayer_txt.text = text;
        }

        /// <summary>
        /// Populates the result list with other players' information if they are not already added.
        /// </summary>
        /// <param name="playerInfoElements">List of player info elements containing nickname and timer.</param>
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
