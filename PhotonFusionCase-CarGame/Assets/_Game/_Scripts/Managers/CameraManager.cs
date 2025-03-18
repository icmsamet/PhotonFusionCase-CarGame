using Unity.Cinemachine;
using UnityEngine;

namespace _Game._Scripts.Managers
{
    public class CameraManager : MonoBehaviour
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

        public void SetCameraToPlayer(Transform player)
        {
            _carShowRoomCamera.gameObject.SetActive(false);
            _playerFollowCamera.gameObject.SetActive(true);
            _playerFollowCamera.Target.TrackingTarget = player;
        }
    }
}