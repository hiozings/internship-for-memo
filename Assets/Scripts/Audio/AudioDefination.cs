using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDefination : MonoBehaviour
{
    public PlayAudioEventSO PlayAudioEvent;

    public AudioClip Clip;

    public void PlayAudioClip()
    {
        PlayAudioEvent.RaiseEvent(Clip);
    }
}
