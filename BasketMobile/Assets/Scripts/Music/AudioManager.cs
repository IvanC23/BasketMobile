using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private List<Sound> _sounds;

    // This Singletone component will be used to manage the audio in the game, it will be used to play music and sound effects
    // and to stop them when needed. It will also be used to lower the volume of the music when the game is paused.
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
        SceneManager.sceneLoaded += OnSceneLoaded;

    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopAllSounds();
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

    public void StopAllSounds()
    {
        foreach (var sound in _sounds)
        {
            if (sound.source.isPlaying)
            {
                sound.source.Stop();
            }
        }
    }

    public void LowerVolume()
    {
        foreach (var sound in _sounds)
        {
            sound.source.volume /= 2;
        }
    }
    public void NormalVolume()
    {
        foreach (var sound in _sounds)
        {
            sound.source.volume *= 2;
        }
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
