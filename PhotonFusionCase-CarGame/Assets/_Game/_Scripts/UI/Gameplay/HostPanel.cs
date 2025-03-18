using UnityEngine;
using UnityEngine.UI;
using _Game._Scripts.Managers;

namespace _Game._Scripts.UI.Gameplay
{
    public class HostPanel : MonoBehaviour
    {
        [Header("**References**")]
        [SerializeField] private Button _hostStart_btn;

        private GameManager _gameManager => GameManager.Instance;

        private void Start()
        {
            _hostStart_btn.onClick.AddListener(HostStartButton);
        }
        private void HostStartButton()
        {
            _gameManager.onGameStartAction?.Invoke();
        }
    }
}