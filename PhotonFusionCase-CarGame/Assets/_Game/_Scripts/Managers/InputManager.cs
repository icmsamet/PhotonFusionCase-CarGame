using _Game._Scripts.Player;
using Fusion;
using System;
using UnityEngine;

namespace _Game._Scripts.Managers
{
    public class InputManager : MonoBehaviour
    {
        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            var data = new PlayerInputData();

            if (Input.GetKey(KeyCode.W))
                data.keyCode = KeyCode.W;

            if (Input.GetKey(KeyCode.S))
                data.keyCode = KeyCode.S;

            if (Input.GetKey(KeyCode.A))
                data.keyCode = KeyCode.A;

            if (Input.GetKey(KeyCode.D))
                data.keyCode = KeyCode.D;

            if (Input.GetKey(KeyCode.Space))
                data.keyCode = KeyCode.Space;

            input.Set(data);
        }
    }
}
