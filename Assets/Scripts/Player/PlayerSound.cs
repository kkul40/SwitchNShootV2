using System;
using UnityEngine;

namespace PlayerNS
{
    public class PlayerSound : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSoruce;
        [SerializeField] private AudioClip deadSoundEffect;

        private void Start()
        {
            audioSoruce = GetComponent<AudioSource>();
        }

        public void PlaySound()
        {
            audioSoruce.PlayOneShot(deadSoundEffect);
        }
    }
}