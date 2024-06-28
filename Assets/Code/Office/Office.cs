using System;
using System.Collections;
using UnityEngine;

public class Office : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip[] _songs;

    private int _songIndex;

    private void Start()
    {
        PlayRandomSong();
    }

    public void EndDay()
    {
        throw new Exception("To be implemented");
    }

    private void PlayRandomSong()
    {
        _songIndex = UnityEngine.Random.Range(0, _songs.Length);
        PlayNextSong();
    }

    private void PlayNextSong()
    {
        _songIndex = (_songIndex + 1) % 3;
        AudioClip song = _songs[_songIndex];
        _audioSource.clip = song;
        _audioSource.Play();
        StartCoroutine(WaitAndPlaySong());
    }

    private IEnumerator WaitAndPlaySong()
    {
        yield return new WaitUntil(() => !_audioSource.isPlaying);
        yield return new WaitForSeconds(1f);
        PlayNextSong();
    }
}
