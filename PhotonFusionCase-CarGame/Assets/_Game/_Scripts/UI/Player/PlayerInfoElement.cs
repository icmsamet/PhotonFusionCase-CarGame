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

        public void Initialize(int id)
        {
            _id = id;
            _timer = 0f;
            _timer_txt.text = _timer.ToString("F2");
            _nickname = "Player " + _id;
            _nickname_txt.text = _nickname;
            _nickname_txt.color = Color.white;
        }

        private void Update()
        {
            if (!_isCounting) return;

            _timer += Time.deltaTime;
            _timer_txt.text = _timer.ToString("F2");
        }

        #region Set

        public void SetNicknameColor(Color color)
        {
            _nickname_txt.color = color;
        }

        #endregion

        #region Counting

        public void StartCounting()
        {
            _isCounting = true;
        }

        public void StopCounting()
        {
            _isCounting = false;
        }

        #endregion

        #region Properties

        public float Timer => _timer;

        public string Nickname => _nickname;

        #endregion
    }
}