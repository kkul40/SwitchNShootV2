using UnityEngine;

namespace PlayerNS.Game.Manager
{
    public class UiInput
    {
        private Inputs inputs;

        public UiInput()
        {
            inputs = new Inputs();
            inputs.Enable();
        }

        public bool IsRestartPressed()
        {
            return inputs.UI.RestartButton.IsPressed();
        }
    }
}