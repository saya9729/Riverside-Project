using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public GameObject particle;

    void PlayParticle()
    {
        if (particle)
        {
            if (!particle.activeSelf) particle.SetActive(true);
        }
        else Debug.LogWarning("Particle not assigned to anim event!");
    }

    void StopParticle()
    {
        if (particle)
        {
            if (particle.activeSelf) particle.SetActive(false);
        }
        else Debug.LogWarning("Particle not assigned to anim event!");
    }
}
