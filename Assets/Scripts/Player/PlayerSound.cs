using Unity.VisualScripting;
using UnityEngine;

namespace PlayerNS
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerSound : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSoruce;
        [SerializeField] private AudioClip deadSoundEffect;
        [SerializeField] private AudioClip engineSoundEffect;

        private void Start()
        {
            audioSoruce = GetComponent<AudioSource>();
            audioSoruce.PlayOneShot(engineSoundEffect);
            audioSoruce.loop = true;
        }

        public void PlaySound()
        {
            audioSoruce.loop = false;
            audioSoruce.PlayOneShot(deadSoundEffect);
        }
    }
}