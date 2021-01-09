using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyState
{
    public static int currHealth;
    public static int maxHealth;
    public static int currBlock;
    public static int attackIntent;
    public static int blockIntent;

    public static void initialize(int scaleFactor)
    {
        currBlock = 0;
        maxHealth = 10 * scaleFactor;
        currHealth = maxHealth;
        attackIntent = 5 + scaleFactor;
        blockIntent = 5 + scaleFactor;
    }

    public static void takeHit(int damage)
    {
        if (currBlock >= damage)
        {
            currBlock -= damage;
        }
        else
        {
            currHealth -= damage - currBlock;
            currBlock = 0;
        }
    }
}
