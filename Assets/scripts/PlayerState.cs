using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerState
{
    public static int currHealth;
    public static int maxHealth;
    public static int currBlock;
    public static int currEnergy;

    public static void initialize()
    {
        maxHealth = 50;
        currHealth = maxHealth;
        currBlock = 0;
        currEnergy = 3;
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
        }
    }
}
