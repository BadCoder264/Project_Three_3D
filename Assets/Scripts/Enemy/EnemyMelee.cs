using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackRadius;
    [SerializeField] private float attackDamageTime;
    [SerializeField] private float attackSoundTime;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private List<AudioClip> audioClips;

    private AudioClip currentClip;

    public void Attack(AudioSource audioSource)
    {
        StartCoroutine(AttackLogic());

        if (audioSource != null && audioClips != null && audioClips.Count > 0)
        {
            StartCoroutine(SoundEffectLogic(audioSource));
        }
    }

    IEnumerator AttackLogic()
    {
        yield return new WaitForSeconds(attackDamageTime);

        Collider[] hits = Physics.OverlapSphere(transform.position, attackRadius, playerLayer);

        foreach (var hit in hits)
        {
            if (hit.tag == "Player")
            {
                hit.GetComponent<PlayerStatistics>()?.Damage(attackDamage);
            }
        }
    }

    IEnumerator SoundEffectLogic(AudioSource audioSource)
    {
        int randomIndex = Random.Range(0, audioClips.Count);
        currentClip = audioClips[randomIndex];

        yield return new WaitForSeconds(attackSoundTime);

        audioSource.clip = currentClip;
        audioSource.Play();
    }
}