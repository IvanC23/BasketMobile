using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    // Sound class created for convenience in managing the audio clips in the game
    // The AudioManager class will use a list of Sound, which will be easily modifiable in the Unity Editor
    public string name;
    public AudioClip clip;
    public AudioMixerGroup outputAudioMixerGroup;

    [Range(0f,1f)]
    public float volume;
    [Range(0.1f,3f)]
    public float pitch;

    public bool loop;
    [HideInInspector]
    public AudioSource source;

}
