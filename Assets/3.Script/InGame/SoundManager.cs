using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : BehaviourSingleton<SoundManager>
{
    protected override bool IsDontDestroy() => true;

    [SerializeField] AudioSource masterSource;
    [SerializeField] AudioSource effectSource;
    [SerializeField] AudioSource characterSource;

    public float volume_Master;
    public float volume_Effect;

    void Start()
    {
        volume_Master = 0f;
        volume_Effect = 0f;
    }

    public void SetVolume(float master, float effect)
    {
        this.volume_Master = master;
        this.volume_Effect = effect;

        masterSource.volume = master;
        effectSource.volume = effect;
    }

    public void SetMasterAudio(AudioClip clip)
    {
        masterSource.clip = clip;
        masterSource.Play();
    }

    public void SetEffectAudio(AudioClip clip)
    {
        effectSource.clip = clip;
        effectSource.Play();
    }
    
    public void SetCharacterAudio(AudioClip clip)
    {
        characterSource.clip = clip;
        characterSource.Play();
    }      
}