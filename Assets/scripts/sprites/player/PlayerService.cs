using System.Collections.Generic;
public class PlayerService
{

    public PlayerData playerData;
    public SceneUiManager sceneUiManager;
    public PlayerService(PlayerData playerData, SceneUiManager sceneUiManager)
    {
        this.playerData = playerData;
        this.sceneUiManager = sceneUiManager;
    }

    public void initialize()
    {
        playerData.initialize();
    }

    public void startFight()
    {
        playerData.powers = new List<CardPower>();
        playerData.currEnergy = playerData.maxEnergy;
        playerData.healthBarData.currBlock = 0;
        playerData.currExtraDraw = 0;
    }

    public void takeHit(int damage)
    {
        if (playerData.healthBarData.currBlock >= damage)
        {
            playerData.healthBarData.currBlock -= damage;
        }
        else
        {
            playerData.healthBarData.currHealth -= damage - playerData.healthBarData.currBlock;
            playerData.healthBarData.currBlock = 0;
        }

        if (playerData.healthBarData.currHealth <= 0)
        {
            sceneUiManager.showGameOver();
        }
    }

    public void addPlayerBlock(int block)
    {
        playerData.healthBarData.currBlock += block;
    }

    public bool isCardPlayable(Card card)
    {
        if (playerData.currEnergy < card.data.playerCardData.energyCost)
        {
            return false;
        }
        return true;
    }

    //Shouldnt be called directly (normally);
    public void onCardPlayed(Card card)
    {
        playerData.currEnergy -= card.data.playerCardData.energyCost;
        playerData.healthBarData.currBlock += card.data.defend;
        playerData.memories += card.data.playerCardData.memoryCount;
        if (card.data.playerCardData.cardPower != null)
        {
            playerData.powers.Add(card.data.playerCardData.cardPower);
        }
    }

    public bool canExtraDraw()
    {
        return playerData.memories > 0 && playerData.currExtraDraw < playerData.extraDrawMax;
    }

    public void onExtraDraw()
    {
        playerData.memories -= playerData.extraDrawCostMemories;
        playerData.currExtraDraw++;
    }

    //Shouldnt be called directly (normally);
    public void endTurn()
    {
        playerData.currEnergy = playerData.maxEnergy;
        playerData.currExtraDraw = 0;
        playerData.healthBarData.currBlock = 0;

        foreach (CardPower power in playerData.powers)
        {
            power.onTurnOver(playerData);
        }
    }
}