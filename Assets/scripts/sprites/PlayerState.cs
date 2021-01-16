using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerState
{
    public static int currHealth;
    public static int maxHealth;
    public static int currBlock;
    public static int currEnergy;
    public static int maxEnergy;

    public static void initialize()
    {
        maxHealth = 50;
        currHealth = maxHealth;

        maxEnergy = 3;
        currEnergy = maxEnergy;

        currBlock = 0;
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
