using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<Sound> sounds;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.loop = sound.loop;

            if (sound.playOnStart)
                sound.source.Play();
        }
    }

    public void PlayClip (string name)
    {
        Sound sound = sounds.Find(x => x.name == name);
        sound.source.Play();
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    public bool playOnStart;

    [HideInInspector]
    public AudioSource source;

    [Range(0f, 1f)]
    public float volume;
    public bool loop;
}
