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

        /// <summary>
        /// Initializes the camera for the player if they have input authority.
        /// </summary>
        private void Start()
        {
            if (Object.HasInputAuthority)
            {
                _cameraManager.SetCameraToPlayer(transform);
            }
        }

        /// <summary>
        /// Processes network updates and handles player input if driving is allowed.
        /// </summary>
        public override void FixedUpdateNetwork()
        {
            if (!_canDrive) return;

            if (GetInput(out PlayerInputData data))
            {
                _carController.Move(data.keyCode);
            }
        }

        #region Set

        /// <summary>
        /// Sets the driving permission for the car.
        /// </summary>
        /// <param name="value">Boolean value to enable or disable driving.</param>
        public void SetCanDrive(bool value)
        {
            RPC_SetCanDrive(value);
        }

        /// <summary>
        /// Remote procedure call to update the driving state across the network.
        /// If disabled, applies brakes and adjusts the camera if the player has input authority.
        /// </summary>
        /// <param name="value">Boolean value to enable or disable driving.</param>
        [Rpc]
        private void RPC_SetCanDrive(bool value)
        {
            _canDrive = value;
            if (value == false)
            {
                _carController.Brakes();
                if (Object.HasInputAuthority)
                {
                    _cameraManager.SetCameraToFinishLine();
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current driving state of the car.
        /// </summary>
        public bool CanDrive
        {
            get => _canDrive;
        }

        /// <summary>
        /// Gets or sets the unique ID of the car.
        /// </summary>
        public int ID
        {
            get => _id;
            set => _id = value;
        }

        #endregion
    }
}