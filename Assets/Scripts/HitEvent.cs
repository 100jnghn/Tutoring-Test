using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEvent : MonoBehaviour
{
    public Enemy enemy;

    public void playerHit()
    {
        enemy.attackAction();
    }
}