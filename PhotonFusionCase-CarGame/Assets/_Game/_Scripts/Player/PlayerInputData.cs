using Fusion;
using UnityEngine;

namespace _Game._Scripts.Player
{
    /// <summary>
    /// Represents the player's input data for network synchronization.
    /// </summary>
    public struct PlayerInputData : INetworkInput
    {
        /// <summary>
        /// The key pressed by the player.
        /// </summary>
        public KeyCode keyCode;
    }

}