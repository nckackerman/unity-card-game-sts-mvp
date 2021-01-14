using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class Enemy
{
    public int currHealth;
    public int maxHealth;
    public int currBlock;
    public List<EnemyTurn> enemyTurns = new List<EnemyTurn>();
    public bool randomAttackOrder = false;
    public Sprite sprite;
    public RuntimeAnimatorController animatorController;

    public void initialize()
    {
        currBlock = 0;
        currHealth = maxHealth;
    }

    public void takeHit(int damage)
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

    public EnemyTurn getEnemyTurn(int turnCount)
    {
        if (randomAttackOrder)
        {
            int index = Random.Range(0, enemyTurns.Count);
            return enemyTurns[index];
        }
        return enemyTurns[turnCount % enemyTurns.Count];
    }
}
