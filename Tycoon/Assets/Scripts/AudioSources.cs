using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSources : MonoBehaviour
{
    public AudioSource EightBitSoundtrack;
    public AudioSource Pop;
    public AudioSource PlantingSeeds;
    public AudioSource PlantRemoving;
    public AudioSource Purchase;
    public AudioSource Coin;

    private void Awake()
    {
        EightBitSoundtrack = GetComponentsInChildren<AudioSource>()[0];
        Pop = GetComponentsInChildren<AudioSource>()[1];
        PlantingSeeds = GetComponentsInChildren<AudioSource>()[2];
        PlantRemoving = GetComponentsInChildren<AudioSource>()[3];
        Purchase = GetComponentsInChildren<AudioSource>()[4];
        Coin = GetComponentsInChildren<AudioSource>()[5];
    }


    /// <summary>
    /// Use AudioSource from AudioSources class, like "EightBitSoundtrack".
    /// </summary>
    public void PlaySound(AudioSource audioSource)
    {
        //audioSource = new AudioSource();
        audioSource.Play();
    }
}

