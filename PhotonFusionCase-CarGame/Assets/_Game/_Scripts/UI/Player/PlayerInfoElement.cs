using TMPro;
using UnityEngine;

namespace _Game._Scripts.UI.Player
{
    public class PlayerInfoElement : MonoBehaviour
    {
        [Header("**References**")]
        [SerializeField] private TextMeshProUGUI _timer_txt;
        [SerializeField] private TextMeshProUGUI _nickname_txt;

        private bool _isCounting = false;
        private float _timer = 0f;

        public void Initialize(string nickname)
        {
            _timer = 0f;
            _timer_txt.text = _timer.ToString("F2");
            _nickname_txt.text = nickname;
            _nickname_txt.color = Color.white;
        }

        public void SetNicknameColor(Color color)
        {
            _nickname_txt.color = color;
        }

        private void Update()
        {
            if (!_isCounting) return;

            _timer += Time.deltaTime;
            _timer_txt.text = _timer.ToString("F2");
        }

        public void StartCounting()
        {
            _isCounting = true;
        }

        public void StopCounting()
        {
            _isCounting = false;
        }
    }
}