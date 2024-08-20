using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioHandler : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    [SerializeField] private AudioSource _music;
    [SerializeField] private AudioSource _SFX;
    public void PlaySFX(AudioClip audioClip)
    {
        _SFX.pitch = Random.Range(0.85f, 1.15f);
        _SFX.PlayOneShot(audioClip);
    }
    public void PlayMusic(AudioClip audioClip)
    {
        _music.clip = audioClip;
        _music.Play();   
    }
    public void OnMasterVolumeValueChanged(float percent)
    {
        _audioMixerGroup.audioMixer.SetFloat("MasterVolume", Mathf.Lerp(-80,0,percent));
    }
    public void OnMusicVolumeValueChanged(float percent)
    {
        _audioMixerGroup.audioMixer.SetFloat("MusicVolume", Mathf.Lerp(-80, 0, percent));
    }
    public void OnSFXVolumeValueChanged(float percent)
    {

        _audioMixerGroup.audioMixer.SetFloat("SFXVolume", Mathf.Lerp(-80, 0, percent));
    }
}

