using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer :MonoBehaviour
{
    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        _source.playOnAwake = false;
        _source.loop = false;
    }

    public void Play(AudioClip clip)
    {
        _source.PlayOneShot(clip);
    }

    private void Reset()
    {
        _source = GetComponent<AudioSource>();
        _source.playOnAwake = false;
        _source.loop = false;
    }
}