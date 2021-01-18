public class PlayerState
{
    public int currHealth;
    public int maxHealth;
    public int currBlock;
    public int currEnergy;
    public int maxEnergy;

    public void initialize()
    {
        maxHealth = 50;
        currHealth = maxHealth;

        maxEnergy = 3;
        currEnergy = maxEnergy;

        currBlock = 0;
    }

    public void startFight()
    {
        currEnergy = maxEnergy;
        currBlock = 0;
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
}
