using System.Collections.Generic;
public class PlayerData
{
    public HealthBarData healthBarData = new HealthBarData(0, 0, 0);
    public int currEnergy;
    public int maxEnergy;
    public int extraDrawMax;
    public int currExtraDraw;
    public int strength = 0;
    public int dexterity = 0;
    public int extraDrawCostMemories;
    public int vulnerableCount;
    public int memories;
    public double vulnerableMultiplier = 1.5;
    public double weakMultiplier = 0.75;
    public int blockToLoseEachTurn = -1;
    public int nextAttackBonusDamage = 0;
    public List<Status> statuses = new List<Status>();

    public void initialize()
    {
        healthBarData.maxHealth = 50;
        healthBarData.currHealth = healthBarData.maxHealth;

        memories = 3;
        maxEnergy = 3;
        currEnergy = maxEnergy;

        extraDrawMax = 3;
        currExtraDraw = 0;

        extraDrawCostMemories = 1;
        vulnerableCount = 0;

        healthBarData.currBlock = 0;
    }
}
