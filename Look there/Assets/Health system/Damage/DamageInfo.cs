using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInfo
{
    public int dmg;
    public PlayerHealthSystem.DamageType damageType;
    public Vector3 dmgPosition;

    public DamageInfo(int dmg, PlayerHealthSystem.DamageType damageType,Vector3 dmgPosition) 
    {
        this.dmg = dmg;
        this.damageType = damageType;
        this.dmgPosition = dmgPosition;
    }
}
