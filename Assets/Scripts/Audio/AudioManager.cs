using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public PlayAudioEventSO FXEvent;
    public PlayAudioEventSO BGMEvent;

    public AudioSource BGMSource;
    public AudioSource FXSource;

    private void OnEnable()
    {
        FXEvent.OnEventRaised += OnFXEvent;
        
    }

    private void OnDisable()
    {
        FXEvent.OnEventRaised -= OnFXEvent;
    }

    private void OnFXEvent(AudioClip clip)
    {
        FXSource.clip = clip;
        FXSource.Play();
    }
}
