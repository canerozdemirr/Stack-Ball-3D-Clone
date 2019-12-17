using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private AudioSource _audioSource;
    public bool _soundPlay = true;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundEffect(AudioClip clip, float volume)
    {
        if(_soundPlay)
            _audioSource.PlayOneShot(clip, volume);
    }

    public void SoundOnOff()
    {
        _soundPlay = !_soundPlay;
    }
}
