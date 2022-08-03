using UnityEngine.Audio;
using System;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public VFX[] vfxs;

    void Start()
    {
        this.RegisterListener(EventID.onPlayVFX, (param) => Enable((string)param));
        this.RegisterListener(EventID.onStopVFX, (param) => Disable((string)param));
        this.RegisterListener(EventID.onSpawnVFX, (param) => Spawn((Tuple<string, Vector3, Quaternion>)param));
    }

    public void Spawn(Tuple<string, Vector3, Quaternion> spawnInfo)
    {
        VFX f = Array.Find(vfxs, item => item.name == spawnInfo.Item1);
        if (f == null)
        {
            Debug.LogWarning("VFX " + name + " not found!");
            return;
        }
        Instantiate(f.source, spawnInfo.Item2, spawnInfo.Item3);
    }

    public void Enable(string vfx)
    {
        VFX f = Array.Find(vfxs, item => item.name == vfx);
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

    public void Disable (string vfx)
    {
        VFX f = Array.Find(vfxs, item => item.name == vfx);
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
