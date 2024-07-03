using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using static PlayerCombat;

public class SkeletonCombat : MonoBehaviour
{
    public enum AttackType
    {
        SLASH,STAB
    }
    public ComboList SkeletonCombos => _comboList;
    public float[] AttacksDelays=>_attacksDelays;

#if UNITY_EDITOR
    [SerializeField] bool _debug;
#endif

    [SerializeField] LayerMask _hitLayer;

    [Header("Attacks")]
    [SerializeField] ComboList _comboList;

    [Header("Attacks delays")]
    [SerializeField] float[] _attacksDelays;

    [Header("Attacks positions")]
    [SerializeField] Transform _slashAttackPos;
    [SerializeField] Transform _stabAttackPos;

    [Header("Attacks sizes")]
    [SerializeField] Vector2 _slashAttackSize;
    [SerializeField] Vector2 _stabAttackSize;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator AttackCor(AttackType attackType)
    {
        List<Collider2D> hitEnemies = new List<Collider2D>();
        switch (attackType)
        {
            case AttackType.SLASH: hitEnemies = Physics2D.OverlapBoxAll(_slashAttackPos.position, _slashAttackSize, 0,_hitLayer).ToList(); break;
            case AttackType.STAB: hitEnemies = Physics2D.OverlapBoxAll(_slashAttackPos.position, _stabAttackSize, 0,_hitLayer).ToList(); break;
        }


        int index = 0;
        for (; index < hitEnemies.Count; index++)
        {
            IDamagable tmp = hitEnemies[index].GetComponentInParent<IDamagable>();
            if (tmp != null) tmp.TakeDamage(new DamageInfo(_comboList.comboList[((int)attackType)].Damage, PlayerHealthSystem.DamageType.ENEMY, transform.position));
        }
        yield return null;
        while (true)
        {
            Collider2D[] colliders = null;
            switch (attackType)
            {
                case AttackType.SLASH: colliders = Physics2D.OverlapBoxAll(_slashAttackPos.position, _slashAttackSize,0, _hitLayer); break;
                case AttackType.STAB: colliders = Physics2D.OverlapBoxAll(_slashAttackPos.position, _stabAttackSize,0,_hitLayer); break;
            }
            for (int i = 0; i < colliders.Length; i++)
            {
                if (!hitEnemies.Contains(colliders[i]))
                {
                    hitEnemies.Add(colliders[i]);
                    IDamagable tmp = colliders[i].GetComponentInParent<IDamagable>();
                    if (tmp != null) tmp.TakeDamage(new DamageInfo(_comboList.comboList[((int)attackType)].Damage, PlayerHealthSystem.DamageType.ENEMY, transform.position));
                }
            }
            yield return null;
        }
    }
    private void OnDrawGizmos()
    {
        if (_debug)
        {
            if (_slashAttackPos != null) Gizmos.DrawWireCube(_slashAttackPos.position, _slashAttackSize);
            if (_stabAttackPos != null) Gizmos.DrawWireCube(_stabAttackPos.position, _stabAttackSize);
        }
    }
}
