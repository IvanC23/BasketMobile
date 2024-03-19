using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private List<Sound> _sounds;
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            InitializeSounds();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    private void InitializeSounds()
    {
        foreach (var sound in _sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.outputAudioMixerGroup = sound.outputAudioMixerGroup;
        }
    }

    public void PlayMusicByName(string name)
    {
        var sound = GetSoundByName(name);
        PlayMusic(sound);
    }

    public void StopMusicByName(string name)
    {
        var sound = GetSoundByName(name);
        sound.source.Stop();
    }

    private void PlayMusic(Sound sound)
    {
        sound.source.Play();
    }

    private Sound GetSoundByName(string name)
    {
        return _sounds.Find(sound => sound.name == name);
    }
}
