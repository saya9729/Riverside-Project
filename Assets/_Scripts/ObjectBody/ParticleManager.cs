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

    void PlayParticle(VFXID vfxName)
    {
        _vfxManager.Enable(vfxName);
    }

    void StopParticle(VFXID vfxName)
    {
        _vfxManager.Disable(vfxName);
    }
}
