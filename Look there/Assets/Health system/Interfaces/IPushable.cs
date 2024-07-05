using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPushable
{
    void Push(PlayerHealthSystem.DamageType damageType, IPusher pusher = null);
    void Push(Vector3 pushDirection, PlayerHealthSystem.DamageType damageType);

    void Push(Vector3 pushDirection, PlayerHealthSystem.DamageType damageType,IPusher pusher);
}
