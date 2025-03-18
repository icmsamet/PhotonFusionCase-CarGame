using Fusion;
using UnityEngine;
using Unity.Cinemachine;

namespace _Game._Scripts.Managers
{
    /// <summary>
    /// Manages different camera views in the game, such as player follow, finish line, and showroom.
    /// Uses a singleton pattern for global access.
    /// </summary>
    public class CameraManager : NetworkBehaviour
    {
        #region Instance
        /// <summary>
        /// Singleton instance of CameraManager.
        /// </summary>
        private static CameraManager _ins;
        public static CameraManager Instance
        {
            get
            {
                if (!_ins)
                {
                    _ins = FindObjectOfType<CameraManager>();
                }
                return _ins;
            }
        }
        #endregion

        [Header("**References**")]
        [SerializeField] private CinemachineCamera _finishLineCamera;
        [SerializeField] private CinemachineCamera _carShowRoomCamera;
        [SerializeField] private CinemachineCamera _playerFollowCamera;

        private GameManager _gameManager => GameManager.Instance;

        private void Start()
        {
            _gameManager.onGameEndAction += OnEndAction;
        }

        /// <summary>
        /// Called when the game ends, switching the camera to the finish line view.
        /// </summary>
        private void OnEndAction()
        {
            SetCameraToFinishLine();
        }

        /// <summary>
        /// Activates the finish line camera and deactivates others.
        /// </summary>
        public void SetCameraToFinishLine()
        {
            _finishLineCamera.gameObject.SetActive(true);
            _carShowRoomCamera.gameObject.SetActive(false);
            _playerFollowCamera.gameObject.SetActive(false);
        }

        /// <summary>
        /// Sets the camera to follow the player.
        /// </summary>
        /// <param name="player">The transform of the player.</param>
        public void SetCameraToPlayer(Transform player)
        {
            _finishLineCamera.gameObject.SetActive(false);
            _carShowRoomCamera.gameObject.SetActive(false);
            _playerFollowCamera.gameObject.SetActive(true);
            _playerFollowCamera.Target.TrackingTarget = player;
        }
    }
}