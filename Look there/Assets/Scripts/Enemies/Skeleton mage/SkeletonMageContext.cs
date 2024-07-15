using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMageContext : EnemyContext
{
    public float playerDetectionRange;
    public BoneMissileSpawner boneSpawner;
    public RotatingObjectsSpawner rotatingObjectsSpawner;
    public float boneMissileCooldown;
    public FloatValue boneSpeed;
    public float boneAttackTime;
    public SkeletonMageMovement enemyMovement;
    public float moveFromPlayerDistance;
    public float moveToPlayerDistance;
}
