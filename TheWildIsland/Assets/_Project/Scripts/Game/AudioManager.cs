using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager sInstance;

    [SerializeField] private GameObject _audioSourceObj;

    private void Awake()
    {
        if(sInstance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            sInstance = this;
        }
    }

    public void PlayAudio(AudioClip clip, Transform pos, float volume)
    {
        GameObject audio = Instantiate(_audioSourceObj, pos);
        AudioSource source = audio.GetComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        audio.GetComponent<Audio>().Play();
    }

    public void PlayLoopAudio(AudioSource source, AudioClip clip, float volume)
    {
        source.clip = clip;
        source.volume = volume;
    }
}
