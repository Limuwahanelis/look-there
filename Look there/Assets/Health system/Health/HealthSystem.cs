using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour,IDamagable
{
    public bool isInvincible;
    public HealthBar hpBar;
    public int maxHP;
    public int currentHP;
    public Action<DamageInfo> OnHitEvent;
    public Action OnDeathEvent;
    // Start is called before the first frame update
    protected void Start()
    {
        hpBar.SetMaxHealth(maxHP);
        currentHP = maxHP;
        hpBar.SetHealth(currentHP);
    }
    public virtual void TakeDamage(DamageInfo info)
    {
        currentHP -= info.dmg;
        hpBar.SetHealth(currentHP);
        OnHitEvent?.Invoke(info);
        if (currentHP <= 0) Kill();
    }

    public virtual void Kill()
    {
        if (OnDeathEvent == null)
        {
            Destroy(gameObject);
            Destroy(hpBar.gameObject);
        }
        else OnDeathEvent.Invoke();
    }
}
