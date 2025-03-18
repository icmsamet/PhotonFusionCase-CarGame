using UnityEngine;
using _Game._Scripts.Managers;

namespace _Game._Scripts.Race
{
    public class FinishLine : MonoBehaviour
    {
        private GameManager _gameManager => GameManager.Instance;

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