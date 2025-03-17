using Fusion;
using UnityEngine;

namespace _Game._Scripts.Player
{
    public struct PlayerInputData : INetworkInput
    {
        public KeyCode keyCode;
    }
}