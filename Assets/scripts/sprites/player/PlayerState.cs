public class PlayerState
{
    public int currHealth;
    public int maxHealth;
    public int currBlock;
    public int currEnergy;
    public int maxEnergy;
    public int extraDrawMax;
    public int currExtraDraw;
    public int extraDrawCostHealth;
    public int vulnerableCount;

    public void initialize()
    {
        maxHealth = 50;
        currHealth = maxHealth;

        maxEnergy = 3;
        currEnergy = maxEnergy;

        extraDrawMax = 5;
        currExtraDraw = 0;

        extraDrawCostHealth = 3;
        vulnerableCount = 0;

        currBlock = 0;
    }

    public void startFight()
    {
        currEnergy = maxEnergy;
        currBlock = 0;
        currExtraDraw = 0;
    }

    public void takeHit(int damage)
    {
        if (currBlock >= damage)
        {
            currBlock -= damage;
        }
        else
        {
            currHealth -= damage - currBlock;
            currBlock = 0;
        }
    }

    public void onCardPlayed(Card card)
    {
        currEnergy -= card.energyCost;
        currBlock += card.defend;
    }

    public bool canExtraDraw()
    {
        return currHealth > extraDrawCostHealth && currExtraDraw < extraDrawMax;
    }

    public void onExtraDraw()
    {
        currHealth -= extraDrawCostHealth;
        currExtraDraw++;
    }

    public void endTurn()
    {
        currEnergy = maxEnergy;
        currExtraDraw = 0;
        currBlock = 0;
    }
}
