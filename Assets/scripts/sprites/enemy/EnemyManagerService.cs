using UnityEngine;
public class EnemyManagerService
{

    private EnemyUiManager enemyUiManager;
    public EnemyManagerService(EnemyUiManager enemyUiManager)
    {
        this.enemyUiManager = enemyUiManager;
    }

    public Enemy getEnemyForFight(int fightCount)
    {
        if (fightCount == 1)
        {
            return EnemyTypes.getFirstEnemy();
        }
        else if (fightCount == 2)
        {
            enemyUiManager.scaleEnemy(new Vector3(25, 25, 1));
            return EnemyTypes.getSecondEnemy();
        }
        return EnemyTypes.getBoss();
    }
}