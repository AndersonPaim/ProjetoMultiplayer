using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Audio : MonoBehaviour
{
    public void Play()
    {
        AudioSource audio = GetComponent<AudioSource>();
        StartCoroutine(StopAudio(audio.clip.length));
    }

    private IEnumerator StopAudio(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}