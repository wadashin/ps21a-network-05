using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// âπÇÃä«óù
/// </summary>
public class Audiomanager : MonoBehaviour
{
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip _sound;


    public void AudioPlay(AudioClip audioClip, float volume)
    {
        _audio.PlayOneShot(audioClip, volume);
    }
}
