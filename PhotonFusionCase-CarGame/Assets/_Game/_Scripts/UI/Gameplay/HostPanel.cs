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

        /// <summary>
        /// Subscribes the HostStartButton method to the button's click event.
        /// </summary>
        private void Start()
        {
            _hostStart_btn.onClick.AddListener(HostStartButton);
        }

        /// <summary>
        /// Invokes the game start action in the GameManager when the host start button is clicked.
        /// </summary>
        private void HostStartButton()
        {
            _gameManager.onGameStartAction?.Invoke();
        }
    }
}