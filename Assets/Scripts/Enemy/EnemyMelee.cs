using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour, IEnemyAttack
{
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private List<AudioClip> audioClips;

    private AudioClip currentClip;

    public void Attack(AudioSource audioSource)
    {
        StartCoroutine(AttackLogic());

        if (audioSource != null && audioClips != null && audioClips.Count > 0)
        {
            PlayRandomSound(audioSource);
        }
        else if (audioSource != null)
        {
            PlayRandomSound(audioSource);
        }
    }

    private void PlayRandomSound(AudioSource audioSource)
    {
        int randomIndex = Random.Range(0, audioClips.Count);
        currentClip = audioClips[randomIndex];

        audioSource.clip = currentClip;
        audioSource.Play();
    }

    IEnumerator AttackLogic()
    {
        yield return new WaitForSeconds(0.85f);

        Collider[] hits = Physics.OverlapSphere(transform.position, attackRadius, playerLayer);

        foreach (var hit in hits)
        {
            if (hit.tag == "Player")
            {
                hit.GetComponent<PlayerStatistics>()?.Damage(attackDamage);
            }
        }
    }
}