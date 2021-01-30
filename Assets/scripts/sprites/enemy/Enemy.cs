using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy
{
    public int currHealth;
    public int maxHealth;
    public int currBlock;
    public List<Card> baseEnemyTurns = new List<Card>();
    public bool randomAttackOrder = false;
    public Sprite sprite;
    public RuntimeAnimatorController animatorController;
    public Card currEnemyTurn;
    public Action onShuffleAction;
    List<Card> enemyModifiers = new List<Card>();

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

    public Card getEnemyTurn(int turnCount)
    {
        if (randomAttackOrder)
        {
            int index = UnityEngine.Random.Range(0, baseEnemyTurns.Count);
            currEnemyTurn = baseEnemyTurns[index];
        }
        else
        {
            currEnemyTurn = baseEnemyTurns[turnCount % baseEnemyTurns.Count];
        }
        enemyModifiers = new List<Card>();
        enemyModifiers.Add(currEnemyTurn);
        return currEnemyTurn;
    }

    public Card getModifiedEnemyTurn(int turnCount)
    {
        Card modifiedEnemyTurn = new Card();
        foreach (Card card in enemyModifiers)
        {
            modifiedEnemyTurn.stackCardAffects(card);
        }

        return modifiedEnemyTurn;
    }

    public void onShuffle()
    {
        if (onShuffleAction != null)
        {
            onShuffleAction();
        }
    }

    public void onEnemyCardDrawn(Card card)
    {
        enemyModifiers.Add(card);
    }
}
