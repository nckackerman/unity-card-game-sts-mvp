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

    public void initializeEnemy(Enemy enemy)
    {
        enemy.data.initialize();
        enemyTurnService.updateEnemyTurn(enemy, 0);
    }

    public void onEnemyCardDrawn(Enemy enemy, Card card)
    {
        enemy.data.enemyTurnData.enemyModifiers.Add(card);
        enemy.data.enemyTurnData.currEnemyTurn = enemyTurnService.getModifiedEnemyTurn(enemy);
    }

    public void onCardPlayed(Enemy enemy, Card card)
    {
        takeHit(enemy, card.data.attack, card.data.attackMultiplier);
        statusService.addStatus(enemy.data.statuses, card.data.statuses);
    }

    public void takeHit(Enemy enemy, int damage, int attackMultiplier)
    {
        if (damage <= 0)
        {
            return;
        }
        for (int i = 0; i < attackMultiplier; i++)
        {
            int modifiedDamage = damage + GameData.getInstance().playerGameObject.playerData.nextAttackBonusDamage;
            if (enemy.data.statuses.Where(status => status.data.name == "Vulnerable").Count() > 0)
            {
                modifiedDamage = (int)(modifiedDamage * enemy.data.vulnerableMultiplier);
            }
            if (enemy.data.healthBarData.currBlock >= modifiedDamage)
            {
                enemy.data.healthBarData.currBlock -= modifiedDamage;
            }
            else
            {
                enemy.data.healthBarData.currHealth -= modifiedDamage - enemy.data.healthBarData.currBlock;
                enemy.data.healthBarData.currBlock = 0;
            }
        }
    }
}