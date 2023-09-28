using UnityEngine;

namespace PlayerNS.Components
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSoruce;
        [SerializeField] private AudioClip sound;

        private void Start()
        {
            audioSoruce = GetComponent<AudioSource>();
            PlaySound();
        }

        public void PlaySound()
        {
            audioSoruce.PlayOneShot(sound);
        }
    }
}