using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum EventID
{
    None = 0,
    onDodgePress,
    onKeyCollected,
    onHPPotCollected,
    onHPChanged,
    onEnergyChange,
    onSolChange,
    onSave,
}