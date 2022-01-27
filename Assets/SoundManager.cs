using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update

    AudioClip[] clips;
    Dictionary<string, AudioSource> sounds;

    void Start()
    {
        clips = Resources.LoadAll<AudioClip>("Sounds/sfx/");
        sounds = new Dictionary<string, AudioSource>();
        foreach (AudioClip clip in clips)
        {
            AudioSource newsound = gameObject.AddComponent<AudioSource>();
            newsound.clip = clip;
            sounds.Add(clip.name, newsound);
        }
    }

    // Update is called once per frame
    void Update()
    {
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
}
