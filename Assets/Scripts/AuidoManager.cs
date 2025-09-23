using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuidoManager : MonoBehaviour
{
    public static AuidoManager Instance;

    [Header("Audio Sources")]
    public AudioSource music;
    public AudioSource soundEffects;

    [Header("Audio Clips")]
    public List<AudioClip> musicTracks;
    public List<AudioClip> soundEffectClips;

    //public AudioClip mainMenuTrack;

    //public AudioClip tigerHunting;
    //public AudioClip tigerProwling;
    //public AudioClip tigerStalking;
    //public AudioClip tigerEvacuating;
    //public AudioClip tigerChase;

    private Vector3 tigerLocation;

    
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMusic()
    {

    }

    public void StopMusic()
    {

    }

    public void ChangeMusic()
    {
        //Fade in and out between tracks
    }

    public void PlaySound()
    {
        //Teleport auido source to tiger location, then play sound
    }
}
