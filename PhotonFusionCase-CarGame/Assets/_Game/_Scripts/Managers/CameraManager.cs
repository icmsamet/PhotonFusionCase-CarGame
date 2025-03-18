using Fusion;
using UnityEngine;
using Unity.Cinemachine;

namespace _Game._Scripts.Managers
{
    public class CameraManager : NetworkBehaviour
    {
        #region Instance
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

        private void OnEndAction()
        {
            SetCameraToFinishLine();
        }

        public void SetCameraToFinishLine()
        {
            _finishLineCamera.gameObject.SetActive(true);
            _carShowRoomCamera.gameObject.SetActive(false);
            _playerFollowCamera.gameObject.SetActive(false);
        }

        public void SetCameraToPlayer(Transform player)
        {
            _finishLineCamera.gameObject.SetActive(false);
            _carShowRoomCamera.gameObject.SetActive(false);
            _playerFollowCamera.gameObject.SetActive(true);
            _playerFollowCamera.Target.TrackingTarget = player;
        }
    }
}