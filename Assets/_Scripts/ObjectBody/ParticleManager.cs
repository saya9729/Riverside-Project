using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    private VFXManager _vfxManager;

    private void Start()
    {
        _vfxManager = FindObjectOfType<VFXManager>();
    }

    void PlayParticle(string particle)
    {
        _vfxManager.Enable(particle);
    }

    void StopParticle(string particle)
    {
        _vfxManager.Disable(particle);
    }
}
