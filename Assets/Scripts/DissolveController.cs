using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveController : MonoBehaviour
{
    [SerializeField] private float dissolveRate = 0.0125f;
    [SerializeField] private float refreshRate = 0.025f;
    [SerializeField] private SkinnedMeshRenderer m_SkinnedMeshRenderer;

    private Material[] materials;

    void Start()
    {
        if (m_SkinnedMeshRenderer != null)
        {
            materials = m_SkinnedMeshRenderer.materials;
        }
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            StartCoroutine(DissolveCo());
        }
    }

    IEnumerator DissolveCo()
    {
        if (materials.Length > 0)
        {
            float counter = 0;

            while (materials[0].GetFloat("_DissolveAmmount") < 1)
            {
                counter += dissolveRate;

                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i].SetFloat("_DissolveAmmount", counter);
                }

                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}