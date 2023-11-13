using System;
using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource BackgroundAudioSource;
    [SerializeField] private AudioSource BackgroundAudioSource2;
    [SerializeField] private AudioSource audioSourceForSoundEffects;

    private AudioSource susturulan;
    private AudioSource actirilan;

    private float maxVolume = 1f;
    public float MusicChangeSpeed = 1;

    private bool isBackgroundMusicInProssec;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        susturulan = BackgroundAudioSource;
        actirilan = BackgroundAudioSource2;
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        audioSourceForSoundEffects.PlayOneShot(clip);
    }

    public void ChangeBackgroundMusic(AudioClip audioClip)
    {
        if (isBackgroundMusicInProssec)
        {
            StopAllCoroutines();
        }
        
        isBackgroundMusicInProssec = true;
        susturulan.mute = false;
        StartCoroutine(LowerTheVolume(susturulan));
        actirilan.clip = audioClip;
        actirilan.mute = false;
        StartCoroutine(UpperTheVolume(actirilan));
        actirilan.Play();
        
        (susturulan, actirilan) = (actirilan, susturulan);
    }

    IEnumerator LowerTheVolume(AudioSource source)
    {
        float velocity = 0;
        while (source.volume > 0.01f)
        {
            source.volume = Mathf.SmoothDamp(source.volume, 0, ref velocity, MusicChangeSpeed);
            yield return new WaitForFixedUpdate();
        }

        source.mute = true;
        source.volume = 0;
        source.Stop();
    }
    
    IEnumerator UpperTheVolume(AudioSource source)
    {
        float velocity = 0;
        while (source.volume < maxVolume - 0.01f)
        {
            source.volume = Mathf.SmoothDamp(source.volume, maxVolume, ref velocity, MusicChangeSpeed);
            yield return new WaitForFixedUpdate();
        }
        source.mute = false;
        source.volume = maxVolume;
        
        isBackgroundMusicInProssec = false;
    }
}