using UnityEngine;

namespace PlayerNS
{
    public class PlayerShield : MonoBehaviour
    {
        [SerializeField] private bool hasShield;
        
        public bool HasShield
        {
            get { return hasShield; }
            set { hasShield = value; }
        }
    }
}