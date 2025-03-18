using Fusion;
using Unity.Cinemachine;
using UnityEngine;

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
        [SerializeField] private CinemachineCamera _carShowRoomCamera;
        [SerializeField] private CinemachineCamera _playerFollowCamera;

        private GameManager _gameManager => GameManager.Instance;

        private void Start()
        {
            _gameManager.onGameEndAction += OnGameEnd;
        }

        #region Action

        private void OnGameEnd()
        {
            RPC_OnGameEnd();
        }

        [Rpc]
        private void RPC_OnGameEnd()
        {
            _carShowRoomCamera.gameObject.SetActive(true);
            _playerFollowCamera.gameObject.SetActive(false);
        }

        #endregion

        #region Set

        public void SetCameraToPlayer(Transform player)
        {
            _carShowRoomCamera.gameObject.SetActive(false);
            _playerFollowCamera.gameObject.SetActive(true);
            _playerFollowCamera.Target.TrackingTarget = player;
        }

        #endregion
    }
}