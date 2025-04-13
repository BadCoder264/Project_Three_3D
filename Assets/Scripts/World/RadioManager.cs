using UnityEngine;
using System.Collections.Generic;

public class RadioManager : MonoBehaviour, IInteractive
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> musicTracks = new List<AudioClip>();

    private int currentTrackIndex = -1;
    private bool isPlaying = false;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        audioSource.loop = false;
        PlayRandomTrack();
    }

    void Update()
    {
        if (isPlaying && !audioSource.isPlaying)
        {
            PlayRandomTrack();
        }
    }

    private void PlayRandomTrack()
    {
        if (musicTracks.Count == 0) return;

        int newTrackIndex;
        do
        {
            newTrackIndex = Random.Range(0, musicTracks.Count);
        }
        while (musicTracks.Count > 1 && newTrackIndex == currentTrackIndex);

        currentTrackIndex = newTrackIndex;
        audioSource.clip = musicTracks[currentTrackIndex];
        audioSource.Play();
        isPlaying = true;
    }

    public void Interactive(PlayerStatistics playerStatistics, InputListener inputListener, PlayerShooting playerShoot, Transform weaponHandler)
    {
        if (musicTracks.Count == 0) return;

        currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Count;
        audioSource.clip = musicTracks[currentTrackIndex];
        audioSource.Play();
        isPlaying = true;
    }
}