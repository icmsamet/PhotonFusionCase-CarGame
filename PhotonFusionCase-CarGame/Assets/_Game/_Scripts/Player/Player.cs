using Fusion;
using UnityEngine;
using _Game._Scripts.Managers;

namespace _Game._Scripts.Player
{
    public class Player : NetworkBehaviour
    {
        [Header("**References**")]
        [SerializeField] private PrometeoCarController _carController;

        private int _id = 0;
        private bool _canDrive = false;

        private CameraManager _cameraManager => CameraManager.Instance;

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
            if(value == false)
            {
                _carController.Brakes();
                if (Object.HasInputAuthority)
                {
                    _cameraManager.SetCameraToFinishLine();
                }
            }
        }

        #region Properties

        public bool CanDrive
        {
            get => _canDrive;
        }

        public int ID
        {
            get => _id;
            set => _id = value;
        }

        #endregion
    }
}