using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource audioSourceForBackground;
    [SerializeField] private AudioSource audioSourceForSoundEffects;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public void PlayOneShot(AudioClip clip)
    {
        audioSourceForSoundEffects.PlayOneShot(clip);
    }
}