using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public ComboList PlayerCombos => _comboList;

    [SerializeField] PlayerMovement _playerMovement;
    [SerializeField] PlayerController _playerController;
    [SerializeField] AnimationManager _animMan;
    [SerializeField] ComboList _comboList;
    //private Collider2D

    [SerializeField] Transform _attackPos;
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
    public IEnumerator AttackCor()
    {

        List<Collider2D> hitEnemies = new List<Collider2D>(Physics2D.OverlapCircleAll(_attackPos.position, attackRange, enemyLayer));
        int index = 0;
        for (; index < hitEnemies.Count; index++)
        {
            IDamagable tmp = hitEnemies[index].GetComponentInParent<IDamagable>();
            if (tmp != null) tmp.TakeDamage(attackDamage,PlayerHealthSystem.DamageType.ENEMY);
        }
        yield return null;
        while (true)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_attackPos.position, attackRange, enemyLayer);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (!hitEnemies.Contains(colliders[i]))
                {
                    hitEnemies.Add(colliders[i]);
                    IDamagable tmp = colliders[i].GetComponentInParent<IDamagable>();
                    if (tmp != null) tmp.TakeDamage(attackDamage, PlayerHealthSystem.DamageType.ENEMY);
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
            if (tmp != null) tmp.TakeDamage(attackDamage, PlayerHealthSystem.DamageType.ENEMY);
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
                    if (tmp != null) tmp.TakeDamage(attackDamage, PlayerHealthSystem.DamageType.ENEMY);
                }
            }
            airAttackTime += Time.deltaTime;
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_attackPos.position, attackRange);
    }
}
