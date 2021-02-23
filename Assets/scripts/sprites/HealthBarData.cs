public class HealthBarData
{
    public int maxHealth;
    public int currHealth;
    public int currBlock;

    public HealthBarData(int maxHealth)
    {
        this.maxHealth = maxHealth;
        this.currHealth = maxHealth;
        this.currBlock = 0;
    }

    public HealthBarData(int maxHealth, int currHealth, int currBlock)
    {
        this.maxHealth = maxHealth;
        this.currHealth = currHealth;
        this.currBlock = currBlock;
    }
}