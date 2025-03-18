using TMPro;
using UnityEngine;

namespace _Game._Scripts.UI.Gameplay
{
    public class OtherPlayerResultElement : MonoBehaviour
    {
        [Header("**References**")]
        [SerializeField] private TextMeshProUGUI _time_txt;
        [SerializeField] private TextMeshProUGUI _nickname_txt;

        private string _nickname = string.Empty;

        /// <summary>
        /// Initializes the player result element with a nickname and a time value.
        /// </summary>
        /// <param name="nickname">The player's nickname.</param>
        /// <param name="time">The player's recorded time.</param>
        public void Initialize(string nickname, float time)
        {
            _nickname = nickname;
            _nickname_txt.text = _nickname;
            _time_txt.text = time.ToString("F2");
        }

        #region Properties

        /// <summary>
        /// Gets the player's nickname.
        /// </summary>
        public string Nickname => _nickname;

        #endregion

    }
}