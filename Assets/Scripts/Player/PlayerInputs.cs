using System;
using UnityEngine;

namespace PlayerNS
{
    public class PlayerInputs : MonoBehaviour
    {
        private Inputs inputs;

        private void Awake()
        {
            inputs = new Inputs();
        }
        
        private void OnEnable()
        {
            inputs.Enable();
        }

        private void OnDisable()
        {
            inputs.Disable();
        }

        public bool IsSwitchPressed()
        {
            return inputs.Player.Switch.triggered;
        }
    }

}