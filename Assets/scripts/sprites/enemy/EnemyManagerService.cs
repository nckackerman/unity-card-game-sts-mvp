using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyManagerService
{

    private GameObject enemyObjectPrefab;
    private GameObject enemyContainer;
    public List<EnemyGameObject> currEnemies = new List<EnemyGameObject>();

    public EnemyManagerService(
        GameObject enemyObjectPrefab,
        GameObject enemyContainer
    )
    {
        this.enemyObjectPrefab = enemyObjectPrefab;
        this.enemyContainer = enemyContainer;
    }

    private static EnemyManagerService enemyManagerService;

    public static EnemyManagerService getInstance()
    {
        if (enemyManagerService == null)
        {
            throw new Exception("Error, attempted to retrieve a null enemyManagerService");
        }
        return enemyManagerService;
    }

    public static void setInstance(EnemyManagerService instance)
    {
        enemyManagerService = instance;
    }

    public void initializeEnemiesForFight(int fightCount)
    {
        foreach (Transform child in enemyContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        if (fightCount == 1)
        {
            currEnemies.Add(EnemyTypes.getFirstEnemy());
        }
        else if (fightCount == 2)
        {
            currEnemies.Add(EnemyTypes.getSecondEnemy());
        }
        else
        {
            currEnemies.Add(EnemyTypes.getBoss());
        }

        foreach (EnemyGameObject enemyGameObject in currEnemies)
        {
            enemyGameObject.transform.SetParent(enemyContainer.transform, false);
            enemyGameObject.initialize();
        }
    }

    public void enemyTurn(int turnCount)
    {
        foreach (EnemyGameObject enemyGameObject in currEnemies)
        {
            Card enemyTurn = enemyGameObject.getModifiedEnemyTurn();

            for (int i = 0; i < enemyTurn.attackMultiplier; i++)
            {
                FightManagerService.getInstance().onEnemyCardPlayed(enemyTurn);
            }
            enemyGameObject.updateBlock(enemyTurn.defend);

            if (enemyTurn.cardToAddToPlayersDecks != null)
            {
                foreach (Card cardToAdd in enemyTurn.cardToAddToPlayersDecks)
                {
                    FightManagerService.getInstance().addCardToDeck(cardToAdd);
                }
            }
            Card newEnemyTurn = enemyGameObject.getEnemyTurn(turnCount);
        }
    }

    public void onEnemyDefeat(EnemyGameObject enemy)
    {
        currEnemies.Remove(enemy);
        if (currEnemies.Count == 0)
        {
            FightManagerService.getInstance().onEnemyDefeat();
        }
    }

    public void onEnemyCardDrawn(Card card)
    {
        currEnemies[0].onEnemyCardDrawn(card);
    }

    public void damageAllEnemy(int damage)
    {
        foreach (EnemyGameObject enemyGameObject in currEnemies)
        {
            enemyGameObject.takeHit(damage);
        }
    }
}