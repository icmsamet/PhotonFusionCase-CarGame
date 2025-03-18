using Fusion;
using UnityEngine;

namespace _Game._Scripts.Player
{
    public class Player : NetworkBehaviour
    {
        [Header("**References**")]
        [SerializeField] private PrometeoCarController _carController;

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out PlayerInputData data))
            {
                _carController.Move(data.keyCode);
            }
        }
    }
}