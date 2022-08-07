using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiParticleAnim : MonoBehaviour
{
    public GameObject particle;

    void PlayParticle()
    {
        if (particle == null)
        {
            Debug.LogWarning("VFX " + name + " not found!");
            return;
        }
        if (!particle.activeSelf)
		{
            particle.SetActive(true);
		}
    }

    void StopParticle()
    {
        if (particle == null)
        {
            Debug.LogWarning("VFX " + name + " not found!");
            return;
        }
        if (particle.activeSelf)
		{
            particle.SetActive(false);
		}
    }
}
