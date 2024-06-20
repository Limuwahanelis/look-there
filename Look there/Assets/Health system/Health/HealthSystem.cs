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
    public Action OnHitEvent;
    public Action OnDeathEvent;
    // Start is called before the first frame update
    protected void Start()
    {
        hpBar.SetMaxHealth(maxHP);
        currentHP = maxHP;
        hpBar.SetHealth(currentHP);
    }
    public virtual void TakeDamage(int dmg, PlayerHealthSystem.DamageType damageType)
    {
        currentHP -= dmg;
        hpBar.SetHealth(currentHP);
        OnHitEvent?.Invoke();
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
