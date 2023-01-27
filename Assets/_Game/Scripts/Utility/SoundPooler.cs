using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPooler : MonoBehaviour
{
    Queue<AudioSource> pool = new Queue<AudioSource>();

    public void PlayAt(AudioClip clip, Vector3 position, float volume, float pitch)
    {
        AudioSource src = Pop();
        src.clip = clip;
        src.volume = volume;
        src.pitch = pitch;
        src.gameObject.transform.position = position;
        StartCoroutine(EnqueueAfterSoundPlayed(src));
    }
    AudioSource Pop()
    {
        if (pool.Count == 0)
        {
            CreateGo();
        }
        return pool.Dequeue();
    }
    void CreateGo()
    {
        GameObject go = Instantiate(new GameObject(), Vector3.zero, Quaternion.identity);
        AudioSource src = go.AddComponent<AudioSource>();
        pool.Enqueue(src);
    }
    IEnumerator EnqueueAfterSoundPlayed(AudioSource src)
    {
        src.Play();
        yield return new WaitUntil(() => !src.isPlaying);
        pool.Enqueue(src);
    }
}
