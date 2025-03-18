using UnityEngine;
using _Game._Scripts.Managers;

namespace _Game._Scripts.Race
{
    public class FinishLine : MonoBehaviour
    {
        private GameManager _gameManager => GameManager.Instance;
        private CameraManager _cameraManager => CameraManager.Instance;

        /// <summary>
        /// Called when another collider enters the trigger zone.
        /// Checks if the collider belongs to a player and notifies the GameManager when the finish line is crossed.
        /// </summary>
        /// <param name="other">The collider that entered the trigger.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Player.Player player = other.transform.root.GetComponent<Player.Player>();
                _gameManager.FinishLineCrossed(player.ID);
            }
        }

    }
}