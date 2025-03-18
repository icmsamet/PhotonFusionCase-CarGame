using Fusion;
using UnityEngine;
using UnityEngine.Events;

namespace _Game._Scripts.Managers
{
    public class GameManager : NetworkBehaviour
    {
        #region Instance
        private static GameManager _ins;
        public static GameManager Instance
        {
            get
            {
                if (!_ins)
                {
                    _ins = FindObjectOfType<GameManager>();
                }
                return _ins;
            }
        }
        #endregion

        public UnityAction onGameStartAction = null;

        private SpawnManager _spawnManager => SpawnManager.Instance;

        private void Start()
        {
            onGameStartAction += OnGameStart;
        }

        private void OnGameStart()
        {
            RPC_OnGameStart();
        }

        [Rpc]
        private void RPC_OnGameStart()
        {
            foreach (var item in _spawnManager.SpawnedCharacters)
            {
                item.Value.GetComponent<Player.Player>().SetCanDrive(true);
            }
        }
    }
}
