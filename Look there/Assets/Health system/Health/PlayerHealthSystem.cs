using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthSystem : HealthSystem,IPushable
{
    [Flags]
    public enum DamageType
    {
        NONE = 0,
        ENEMY = 2,
        MISSILE = 4,
        TRAPS = 8,
        ALL = 16,
    }
    public Action<Vector3, IPusher> OnPushed;
    [SerializeField] float _invincibilityAfterHitDuration;
    [SerializeField] Collider2D[] _playerCols;
    private bool _canBePushed=true;
    private DamageType _invincibiltyType;
    private DamageType _pushInvincibiltyType;
    private IPusher _playerPusher;
    public PlayerController player;
    public Ringhandle pushHandle;
    public float pushForce=2f;
    private new void Start()
    {
        if (hpBar == null) return;
        hpBar.SetMaxHealth(maxHP);
        hpBar.SetHealth(currentHP);
    }
    public void SetInvincibility(DamageType invincibiltyType)
    {
        _invincibiltyType = invincibiltyType;
    }
    public void SetPushInvincibility(DamageType invincibiltyType)
    {
        _pushInvincibiltyType = invincibiltyType;
    }
    public override void TakeDamage(DamageInfo info)
    {
        if (player.IsAlive)
        {
            if (_invincibiltyType==info.damageType || _invincibiltyType==DamageType.ALL) return;
            currentHP -= info.dmg;
            if(hpBar!=null) hpBar.SetHealth(currentHP);

            if (currentHP < 0) Kill();
            else OnHitEvent?.Invoke(info);
            //player.currentState.OnHit();
            StartCoroutine(InvincibilityCor());
        }

    }

    public override void Kill()
    {
        if (OnDeathEvent == null) Destroy(gameObject);
        else OnDeathEvent.Invoke();
    }
    IEnumerator PushCor(IPusher pusher)
    {
        _canBePushed = false;
        yield return new WaitForSeconds(_invincibilityAfterHitDuration);
        _canBePushed = true;
        if (pusher != null)
        {
            pusher.ResumeCollisonsWithPlayer(_playerCols);
        }
    }
    IEnumerator InvincibilityCor()
    {
        _invincibiltyType=DamageType.ALL;
        _pushInvincibiltyType = DamageType.ALL;
        yield return new WaitForSeconds(_invincibilityAfterHitDuration);
        _invincibiltyType = DamageType.NONE;
        _pushInvincibiltyType = DamageType.NONE;
    }

    public void Push(PlayerHealthSystem.DamageType damageType, IPusher pusher = null)
    {
        if (player.IsAlive)
        {
            if (_pushInvincibiltyType == damageType || _pushInvincibiltyType == DamageType.ALL) return;
            //_playerMovement.PushPlayer(pushHandle.GetVector() * pushForce,pusher);
            OnPushed?.Invoke(Vector3.zero, pusher);
            if (pusher != null) pusher.PreventCollisionWithPlayer(_playerCols);
            StartCoroutine(PushCor(pusher));
        }
    }
    public void Push(Vector3 pushDirection, DamageType damageType)
    {
        if (player.IsAlive)
        {
            if (_pushInvincibiltyType == damageType || _pushInvincibiltyType == DamageType.ALL) return;
            //_playerMovement.PushPlayer(direction, pushHandle.GetVector() * pushForce, null);
            OnPushed?.Invoke(pushDirection, null);
            StartCoroutine(PushCor(null));
        }
    }
    public void Push(Vector3 pushDirection, DamageType damageType, IPusher pusher)
    {
        if (player.IsAlive)
        {
            if (_pushInvincibiltyType == damageType || _pushInvincibiltyType == DamageType.ALL) return;
            //_playerMovement.PushPlayer(direction, pushHandle.GetVector() * pushForce, pusher);
            OnPushed?.Invoke(pushDirection, pusher);
            _playerPusher = pusher;
            StartCoroutine(PushCor(pusher));
        }
    }
    public void IncreaseHealthBarMaxValue()
    {
        hpBar.SetMaxHealth(maxHP);
        hpBar.SetHealth(currentHP);
    }
}
