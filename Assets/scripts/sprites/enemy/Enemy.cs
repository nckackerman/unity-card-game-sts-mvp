public class Enemy
{
    public EnemyData data;
    public EnemyActions enemyActions;

    public Enemy(EnemyData data, EnemyActions enemyActions)
    {
        this.data = data;
        this.enemyActions = enemyActions;
    }
}