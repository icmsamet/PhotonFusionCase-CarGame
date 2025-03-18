using TMPro;
using UnityEngine;

namespace _Game._Scripts.UI.Player
{
    public class PlayerInfoElement : MonoBehaviour
    {
        [Header("**References**")]
        [SerializeField] private TextMeshProUGUI _timer_txt;
        [SerializeField] private TextMeshProUGUI _nickname_txt;

        private int _id = 0;
        private float _timer = 0f;
        private bool _isCounting = false;
        private string _nickname = string.Empty;

        /// <summary>
        /// Initializes the player with an ID, resets the timer, and sets the default nickname.
        /// </summary>
        /// <param name="id">The unique identifier for the player.</param>
        public void Initialize(int id)
        {
            _id = id;
            _timer = 0f;
            _timer_txt.text = _timer.ToString("F2");
            _nickname = "Player " + _id;
            _nickname_txt.text = _nickname;
            _nickname_txt.color = Color.white;
        }

        /// <summary>
        /// Updates the timer while counting is active.
        /// </summary>
        private void Update()
        {
            if (!_isCounting) return;

            _timer += Time.deltaTime;
            _timer_txt.text = _timer.ToString("F2");
        }

        #region Set

        /// <summary>
        /// Sets the color of the player's nickname text.
        /// </summary>
        /// <param name="color">The color to apply to the nickname text.</param>
        public void SetNicknameColor(Color color)
        {
            _nickname_txt.color = color;
        }

        #endregion

        #region Counting

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void StartCounting()
        {
            _isCounting = true;
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void StopCounting()
        {
            _isCounting = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current timer value.
        /// </summary>
        public float Timer => _timer;

        /// <summary>
        /// Gets the player's nickname.
        /// </summary>
        public string Nickname => _nickname;

        #endregion
    }
}