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
            enemyGameObject.getEnemyTurn(0);
        }
        showEnemies();
    }

    public void showEnemies()
    {
        foreach (EnemyGameObject enemyGameObject in currEnemies)
        {
            enemyGameObject.initialize();
            Card newEnemyTurn = enemyGameObject.getEnemyTurn(0);
            enemyGameObject.updateEnemyIntent(newEnemyTurn);
        }
    }

    public void hideEnemy()
    {
        foreach (EnemyGameObject enemyGameObject in currEnemies)
        {
            enemyGameObject.gameObject.SetActive(false);
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
            enemyGameObject.updateEnemyIntent(newEnemyTurn);
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
        currEnemies[0].updateEnemyIntent(currEnemies[0].getModifiedEnemyTurn());
    }

    public void damageAllEnemy(int damage)
    {
        foreach (EnemyGameObject enemyGameObject in currEnemies)
        {
            enemyGameObject.takeHit(damage);
        }
    }

    public void updateEnemyUi()
    {
        foreach (EnemyGameObject enemyGameObject in currEnemies)
        {
            enemyGameObject.updateEnemyUi();
        }
    }
}