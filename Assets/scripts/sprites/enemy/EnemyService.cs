using System.Linq;
using UnityEngine;

public class EnemyService
{

    public EnemyTurnService enemyTurnService;
    public StatusService statusService;

    public EnemyService(EnemyTurnService enemyTurnService, StatusService statusService)
    {
        this.enemyTurnService = enemyTurnService;
        this.statusService = statusService;
    }

    public void initializeEnemy(EnemyGameObject enemyGameObject)
    {
        enemyGameObject.setPosition();
        enemyGameObject.enemy.data.initialize();
        enemyTurnService.updateEnemyTurn(enemyGameObject.enemy, 0);
    }

    public void onEnemyCardDrawn(EnemyGameObject enemyGameObject, Card card)
    {
        enemyGameObject.enemy.data.enemyTurnData.enemyModifiers.Add(card);
        enemyGameObject.enemy.data.enemyTurnData.currEnemyTurn = enemyTurnService.getModifiedEnemyTurn(enemyGameObject);
    }

    public void onCardPlayed(EnemyGameObject enemyGameObject, Card card)
    {
        takeHit(enemyGameObject, card.data.attack, card.data.attackMultiplier);
        statusService.addStatuses(enemyGameObject.statusesObject, card.data.statuses, enemyGameObject.enemy.data);
    }

    public void takeHit(EnemyGameObject enemyGameObject, int damage, int attackMultiplier)
    {
        if (damage <= 0)
        {
            return;
        }
        for (int i = 0; i < attackMultiplier; i++)
        {
            int modifiedDamage = damage + GameData.getInstance().playerGameObject.playerData.nextAttackBonusDamage;
            if (StatusUtils.getAppliedStatusCount(StatusTypes.StatusEnum.vulnerable, enemyGameObject.statusesObject.activeStatuses) > 0)
            {
                modifiedDamage = (int)(modifiedDamage * enemyGameObject.enemy.data.vulnerableMultiplier);
            }
            if (enemyGameObject.enemy.data.healthBarData.currBlock >= modifiedDamage)
            {
                enemyGameObject.enemy.data.healthBarData.currBlock -= modifiedDamage;
            }
            else
            {
                enemyGameObject.enemy.data.healthBarData.currHealth -= modifiedDamage - enemyGameObject.enemy.data.healthBarData.currBlock;
                enemyGameObject.enemy.data.healthBarData.currBlock = 0;
            }
        }
    }
}