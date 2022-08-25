using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum EventID
{
    None = 0,
    onKeyCollected,
    onHPPotCollected,
    onHPChanged,
    onHPMaxChanged,
    onSolChange,
    onSlowTime,
    onSlowTimeCoolDown,
    onSave,
    onLose,
    onWin,
    onDash,
    onDashChargeCooldown,

    //sound
    onPlaySound,
    onStopSound,
    
    //vfx
    onPlayVFX,
    onStopVFX,
    onSpawnVFX,

    //UI
    onToggleUI
}
