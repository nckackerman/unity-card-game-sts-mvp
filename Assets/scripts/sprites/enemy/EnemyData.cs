using System.Collections.Generic;

public class EnemyData
{
    public HealthBarData healthBarData = new HealthBarData(0, 0, 0);
    public EnemyTurnData enemyTurnData = new EnemyTurnData();
    public double weakMultiplier = 0.75;
    public double vulnerableMultiplier = 1.5;
    public List<Status> statuses = new List<Status>();

    public EnemyData(HealthBarData healthBarData, EnemyTurnData enemyTurnData)
    {
        this.healthBarData = healthBarData;
        this.enemyTurnData = enemyTurnData;
    }
    public void initialize()
    {
        healthBarData.currBlock = 0;
        healthBarData.currHealth = healthBarData.maxHealth;
    }
}
