using UnityEngine.Audio;
using System;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public VFX[] vfxs;

    void Start()
    {
        this.RegisterListener(EventID.onPlayVFX, (param) => Enable((VFXID)param));
        this.RegisterListener(EventID.onStopVFX, (param) => Disable((VFXID)param));
        this.RegisterListener(EventID.onSpawnVFX, (param) => Spawn((Tuple<VFXID, Vector3, Quaternion>)param));
    }

    public void Spawn(Tuple<VFXID, Vector3, Quaternion> p_spawnInfo)
    {
        VFX f = Array.Find(vfxs, item => item.name == p_spawnInfo.Item1.ToString());
        if (f == null)
        {
            Debug.LogWarning("VFX " + name + " not found!");
            return;
        }
        Instantiate(f.source, p_spawnInfo.Item2, p_spawnInfo.Item3);
    }

    public void Enable(VFXID vfx)
    {
        VFX f = Array.Find(vfxs, item => item.name == vfx.ToString());
        if (f == null)
        {
            Debug.LogWarning("VFX " + name + " not found!");
            return;
        }
        if (!f.source.activeSelf)
		{
            f.source.SetActive(true);
		}
    }

    public void Disable (VFXID vfx)
    {
        VFX f = Array.Find(vfxs, item => item.name == vfx.ToString());
        if (f == null)
        {
            Debug.LogWarning("VFX " + name + " not found!");
            return;
        }
        if (f.source.activeSelf)
		{
            f.source.SetActive(false);
		}
    }
}
