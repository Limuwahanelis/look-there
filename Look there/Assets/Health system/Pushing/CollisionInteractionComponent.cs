using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionInteractionComponent : MonoBehaviour, IPusher
{
    [SerializeField]
    protected PlayerHealthSystem.DamageType _pushType;
    [SerializeField]
    protected PlayerHealthSystem.DamageType _damageType;
    [SerializeField]
    protected bool _pushCollidingObject;
    [SerializeField]
    protected bool _damageCollidingObject;
    [SerializeField]
    protected int damage;

    public Vector3 Position { get => transform.position; }

    private void Start()
    {
        
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(_pushCollidingObject)
        {
            IPushable toPush = collision.transform.GetComponentInParent<IPushable>();
            if (toPush != null) toPush.Push(_pushType);
        }
        if(_damageCollidingObject)
        {
            IDamagable toDamage = collision.transform.GetComponentInParent<IDamagable>();
            if (toDamage != null) toDamage.TakeDamage(new DamageInfo(damage, _damageType, transform.position));
        }
    }

    public void ResumeCollisonsWithPlayer(Collider2D[] playerCols)
    {
        foreach (Collider2D col in playerCols)
        {
            Physics2D.IgnoreCollision(col, GetComponent<Collider2D>(), false);
        }

    }

    public void PreventCollisionWithPlayer(Collider2D[] playerCols)
    {
        foreach (Collider2D col in playerCols)
        {
            Physics2D.IgnoreCollision(col, GetComponent<Collider2D>(), true);
        }
    }
}
