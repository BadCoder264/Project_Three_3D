using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClips;

    private bool isPlaying;
    private AudioClip currentClip;

    public void MoveTowardsPlayer(Vector3 playerPosition, AudioSource audioSource, EnemyStatistics enemyStatistics)
    {
        if (enemyStatistics.currentHealth <= 0)
            return;

        if (enemyStatistics.navMeshAgent != null)
        {
            enemyStatistics.navMeshAgent.SetDestination(playerPosition);
        }

        HandleMovementSounds(audioSource);
    }

    private void HandleMovementSounds(AudioSource audioSource)
    {
        if (audioSource == null || audioClips == null || audioClips.Count == 0)
            return;

        if (!isPlaying)
        {
            PlayRandomSound(audioSource);
        }
        else if (!audioSource.isPlaying)
        {
            isPlaying = false;
            PlayRandomSound(audioSource);
        }
    }

    private void PlayRandomSound(AudioSource audioSource)
    {
        int randomIndex = Random.Range(0, audioClips.Count);
        currentClip = audioClips[randomIndex];

        audioSource.clip = currentClip;
        audioSource.Play();
        isPlaying = true;
    }
}