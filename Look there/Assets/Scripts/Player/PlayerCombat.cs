using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public enum AttackModifiers
    {

        NONE,UP_ARROW,DOWN_ARROW,
    }
    public enum AttackType
    {
        NORMAL,JUMPING,AIR_SLAM_LOOP,AIR_SLAM_LAND
    }

#if UNITY_EDITOR
    [SerializeField] bool _debug;
#endif
    public float SlamSpeed;
    public ComboList PlayerCombos => _comboList;
    public ComboList PlayerAirCombos => _airComboList;
    public LayerMask enemyLayer;
    [SerializeField] float attackRange;
    [SerializeField] int attackDamage;
    public Sprite playerHitSprite;
    [SerializeField] PlayerMovement _playerMovement;
    [SerializeField] PlayerController _playerController;
    [SerializeField] AnimationManager _animMan;
    [SerializeField] ComboList _comboList;
    [SerializeField] ComboList _airComboList;

    [Header("Attacks positions")]

    [SerializeField] Transform _attackPos;
    [SerializeField] Transform _jumpAttackPos;
    [SerializeField] Transform _airSlamLoopAttackPos;
    [SerializeField] Transform _airSlamLandingAttackPos;
    [Header("Attacks sizes")]
    [SerializeField] Vector2 _jumpAttackSize;
    [SerializeField] Vector2 _airSlamLoopAttackSize;
    [SerializeField] Vector2 _airSlamLandingAttackSize;

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
            case AttackType.AIR_SLAM_LOOP: hitEnemies = Physics2D.OverlapBoxAll(_airSlamLoopAttackPos.position, _airSlamLoopAttackSize, enemyLayer).ToList(); break;
            case AttackType.AIR_SLAM_LAND: hitEnemies = Physics2D.OverlapBoxAll(_airSlamLandingAttackPos.position, _airSlamLandingAttackSize, enemyLayer).ToList(); break;
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
        if (_debug)
        {
            if (_attackPos != null) Gizmos.DrawWireSphere(_attackPos.position, attackRange);
            if (_jumpAttackPos != null) Gizmos.DrawWireCube(_jumpAttackPos.position, _jumpAttackSize);
            if (_airSlamLoopAttackPos != null) Gizmos.DrawWireCube(_airSlamLoopAttackPos.position, _airSlamLoopAttackSize);
            if (_airSlamLandingAttackPos != null) Gizmos.DrawWireCube(_airSlamLandingAttackPos.position, _airSlamLandingAttackSize);
        }
    }
}
