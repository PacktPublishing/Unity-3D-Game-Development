using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSounds : MonoBehaviour
{
    public AudioSource AnimSound;

    public List<AudioClip> soundPool = new List<AudioClip>();

    void PlaySound()
    {
        AnimSound.clip = soundPool[Random.Range(0, soundPool.Count)];


        AnimSound.pitch = Random.Range(-0.3f, 0.3f);

        AnimSound.Play();
    }
}
