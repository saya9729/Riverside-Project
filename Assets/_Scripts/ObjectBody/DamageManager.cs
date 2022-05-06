using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    //attached to damage dealing instances e.g bullet, sword, dagger, . . . that has collider 
    [SerializeField] private float dmg;

    public float GetDamage()
    {
        return dmg;
    }

    public void SetDamage(float newDmg)
    {
        dmg = newDmg;
    }
}
