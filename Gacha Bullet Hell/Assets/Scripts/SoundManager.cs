using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
//using System;

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;
    private AudioMixerGroup mastermix;

    private AudioMixer masterMixActuall;

    private bool fadingsound;
    private AudioSource currentlyFadingSound;
    private float ogvolume;

    public void Awake()
    {
        //mastermix = Resources.Load<AudioMixerGroup>("MainMixer");
        //masterMixActuall = Resources.Load<AudioMixer>("MainMixer");

        //masterMixActuall.SetFloat("MasterVolume", TitleManager.globalVolume);


        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = mastermix;
        }


    }

    public void FixedUpdate()
    {
        if (fadingsound == true)
        {
            if (currentlyFadingSound.volume > 0)
            {
                currentlyFadingSound.volume -= 0.005f;
            }
            else
            {
                fadingsound = false;
                currentlyFadingSound.Pause();
                currentlyFadingSound.volume = ogvolume;
                currentlyFadingSound = null;
            }
        }
    }

    public float getSoundLength(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                return s.clip.length;
            }
        }

        return 0;
    }

    public void FadeOutSong(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                currentlyFadingSound = s.source;
                ogvolume = s.volume;

                fadingsound = true;
            }
        }
    }

    public AudioSource getSourceByName(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                return s.source;
            }
        }

        return null;
    }



    public void Play(string name)
    {
        //Sound s = Array.Find(sounds, sound => sound.name == name);

        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                s.source.Play();
                return;
            }
        }

    }

    public void PlayRandPitch(string name, float pitch, float vol)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                s.source.volume = vol;
                s.source.pitch = pitch * Random.Range(0.95f, 1.05f);
                s.source.Play();
                return;
            }
        }
    }

    public void Pause(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                s.source.Pause();
                return;
            }
        }
    }
}