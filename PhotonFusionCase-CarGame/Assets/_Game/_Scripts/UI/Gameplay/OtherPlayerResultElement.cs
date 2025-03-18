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

        public void Initialize(string nickname, float time)
        {
            _nickname = nickname;
            _nickname_txt.text = _nickname;
            _time_txt.text = time.ToString("F2");
        }

        #region Properties

        public string Nickname => _nickname;

        #endregion
    }
}