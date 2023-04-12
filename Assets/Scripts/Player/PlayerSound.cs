using UnityEngine;

namespace PlayerNS
{
    public class PlayerSound : MonoBehaviour
    {
        [SerializeField] private AudioClip deadSoundEffect;


        public void PlaySound()
        {
            SoundManager.Instance.PlayOneShot(deadSoundEffect);
        }
    }
}