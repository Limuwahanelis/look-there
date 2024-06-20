using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerPusher
{
    public void ResumeCollisonsWithPlayer(Collider2D[] playerCols);
    public void PreventCollisionWithPlayer(Collider2D[] playerCols);
}
