using _Game._Scripts.Managers;
using Fusion;
using UnityEngine;

namespace _Game._Scripts.Player
{
    public class Player : NetworkBehaviour
    {
        [Header("**References**")]
        [SerializeField] private PrometeoCarController _carController;

        private GameManager _gameManager => GameManager.Instance;
        private CameraManager _cameraManager => CameraManager.Instance;

        private bool _canDrive = false;

        private void Start()
        {
            if (Object.HasInputAuthority)
            {
                _cameraManager.SetCameraToPlayer(transform);
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (!_canDrive) return;

            if (GetInput(out PlayerInputData data))
            {
                _carController.Move(data.keyCode);
            }
        }

        public void SetCanDrive(bool value)
        {
            RPC_SetCanDrive(value);
        }

        [Rpc]
        private void RPC_SetCanDrive(bool value)
        {
            _canDrive = value;
        }

        #region Properties

        public bool CanDrive
        {
            get => _canDrive;
            set => _canDrive = value;
        }

        #endregion
    }
}