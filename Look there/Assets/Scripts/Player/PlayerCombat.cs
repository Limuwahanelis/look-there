using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public enum AttackModifiers
    {

        NONE,UP_ARROW
    }
    public enum AttackType
    {
        NORMAL,JUMPING
    }
    public ComboList PlayerCombos => _comboList;
    public ComboList PlayerAirCombos => _airComboList;
    [SerializeField] PlayerMovement _playerMovement;
    [SerializeField] PlayerController _playerController;
    [SerializeField] AnimationManager _animMan;
    [SerializeField] ComboList _comboList;
    [SerializeField] ComboList _airComboList;
    //private Collider2D

    [SerializeField] Transform _attackPos;
    [SerializeField] Transform _jumpAttackPos;
    [SerializeField] Vector2 _jumpAttackSize;
    public LayerMask enemyLayer;
    public float attackRange;
    public int attackDamage;
    public Sprite playerHitSprite;
    private Coroutine airAttackCor;
    private Coroutine playerMovAirAttackCor;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StopAttack()
    {
        StopAllCoroutines();
    }
    public void StopAirAttack()
    {
        StopCoroutine(airAttackCor);
        StopCoroutine(playerMovAirAttackCor);
        //_player.playerMovement.SetGravityScale(2);
    }
    public void ChangeSpriteToPushed()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = playerHitSprite;
    }
    public IEnumerator AttackCor(AttackType attackType)
    {
        List<Collider2D> hitEnemies = new List<Collider2D>() ;
        switch (attackType)
        {
            case AttackType.NORMAL: hitEnemies = Physics2D.OverlapCircleAll(_attackPos.position, attackRange, enemyLayer).ToList(); break;
            case AttackType.JUMPING: hitEnemies = Physics2D.OverlapBoxAll(_attackPos.position, _jumpAttackSize, enemyLayer).ToList(); break;
        }

        
        int index = 0;
        for (; index < hitEnemies.Count; index++)
        {
            IDamagable tmp = hitEnemies[index].GetComponentInParent<IDamagable>();
            if (tmp != null) tmp.TakeDamage(new DamageInfo(attackDamage,PlayerHealthSystem.DamageType.ENEMY,transform.position));
        }
        yield return null;
        while (true)
        {
            Collider2D[] colliders = null;
            switch (attackType)
            {
                case AttackType.NORMAL: colliders = Physics2D.OverlapCircleAll(_attackPos.position, attackRange, enemyLayer); break;
                case AttackType.JUMPING: colliders = Physics2D.OverlapBoxAll(_attackPos.position, _jumpAttackSize, enemyLayer); break;
            }
            for (int i = 0; i < colliders.Length; i++)
            {
                if (!hitEnemies.Contains(colliders[i]))
                {
                    hitEnemies.Add(colliders[i]);
                    IDamagable tmp = colliders[i].GetComponentInParent<IDamagable>();
                    if (tmp != null) tmp.TakeDamage(new DamageInfo(attackDamage, PlayerHealthSystem.DamageType.ENEMY, transform.position));
                }
            }
            yield return null;
        }
    }
    public IEnumerator AirAttackCor()
    {
        float airAttackTime = 0f;
        List<Collider2D> hitEnemies = new List<Collider2D>(Physics2D.OverlapCircleAll(_attackPos.position, attackRange, enemyLayer));
        int index = 0;
        for (; index < hitEnemies.Count; index++)
        {
            IDamagable tmp = hitEnemies[index].GetComponentInParent<IDamagable>();
            if (tmp != null) tmp.TakeDamage(new DamageInfo(attackDamage, PlayerHealthSystem.DamageType.ENEMY, transform.position));
        }
        yield return null;
        while (airAttackTime <= _animMan.GetAnimationLength("Air attack"))
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_attackPos.position, attackRange, enemyLayer);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (!hitEnemies.Contains(colliders[i]))
                {
                    hitEnemies.Add(colliders[i]);
                    IDamagable tmp = colliders[i].GetComponentInParent<IDamagable>();
                    if (tmp != null) tmp.TakeDamage(new DamageInfo(attackDamage, PlayerHealthSystem.DamageType.ENEMY, transform.position));
                }
            }
            airAttackTime += Time.deltaTime;
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if (_attackPos != null) Gizmos.DrawWireSphere(_attackPos.position, attackRange);
        if (_jumpAttackPos != null) Gizmos.DrawWireCube(_jumpAttackPos.position, _jumpAttackSize);
    }
}
