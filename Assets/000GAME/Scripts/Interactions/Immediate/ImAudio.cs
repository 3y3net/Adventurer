using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ImAudio : ReactionBase
{

    public AudioSource audioSource;     // The AudioSource to play the clip.
    public AudioClip audioClip;         // The AudioClip to be played.
    public float delay;                 // How long after React is called before the clip plays.


    protected override void ImmediateReaction()
    {
        // Set the AudioSource's clip to the given one and play with the given delay.
        audioSource.clip = audioClip;
        audioSource.PlayDelayed(delay);
    }
}
