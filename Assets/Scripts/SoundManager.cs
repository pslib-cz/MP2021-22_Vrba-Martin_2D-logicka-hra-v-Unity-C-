using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    Dictionary<string, AudioSource> sounds;
    Dictionary<string, AudioSource> music;
    public static bool DoneLoading = false;
    public bool Muted;
    void Start()
    {
        Muted = false;

        AudioClip[] clips;
        //sound effects
        clips = Resources.LoadAll<AudioClip>("Sounds/sfx/");
        sounds = new Dictionary<string, AudioSource>();
        foreach (AudioClip clip in clips)
        {
            AudioSource newsound = gameObject.AddComponent<AudioSource>();
            newsound.clip = clip;
            sounds.Add(clip.name, newsound);
        }

        //music
        clips = Resources.LoadAll<AudioClip>("Sounds/music/");
        music = new Dictionary<string, AudioSource>();
        foreach (AudioClip clip in clips)
        {
            AudioSource newsound = gameObject.AddComponent<AudioSource>();
            newsound.clip = clip;
            newsound.loop = true;
            music.Add(clip.name, newsound);
        }

        DoneLoading = true;
    }


    void Update()
    {
        //(un)mute on M press
        if (Input.GetKeyDown(KeyCode.M))
        {
            Muted = !Muted;
            if (Muted)
            {
                AudioListener.volume = 0;
            }
            else
            {
                AudioListener.volume = 1;
            }
        }
    }

    public void Play(string name)
    {
        if (!sounds.ContainsKey(name))
        {
            Debug.LogWarning("There is no sound called \"" + name + "\"");
            return;
        }
        else
        {
        sounds[name].Play();
        }
    }
    
    public void PlayMusic(string name)
    {
        void StopAllMusic()
        {
            foreach (AudioSource audiosource in music.Values)
            {
                audiosource.Stop();
            }
        }

        if (!music.ContainsKey(name))
        {
            Debug.LogWarning("There is no music called \"" + name + "\"");
            return;
        }
        else
        {
            StopAllMusic();
            music[name].Play();
        }
    }
}
