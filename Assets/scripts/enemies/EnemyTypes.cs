using System.Collections.Generic;
public static class EnemyTypes
{
    public static Enemy getBasicEnemy()
    {
        Enemy basicEnemy = new Enemy();
        basicEnemy.maxHealth = 15;
        basicEnemy.enemyTurns.Add(new EnemyTurn(10, 0));
        basicEnemy.enemyTurns.Add(new EnemyTurn(0, 10));
        basicEnemy.enemyTurns.Add(new EnemyTurn(6, 6));
        basicEnemy.randomAttackOrder = true;

        basicEnemy.initialize();
        return basicEnemy;
    }

    public static Enemy getBoss()
    {
        Enemy bossEnemy = new Enemy();
        bossEnemy.maxHealth = 33;
        for (int i = 0; i < 3; i++)
        {
            EnemyTurn turn = new EnemyTurn(3, 0);
            turn.attackMultiplier = 2 + i;
            bossEnemy.enemyTurns.Add(turn);
        }

        bossEnemy.initialize();
        return bossEnemy;
    }
}