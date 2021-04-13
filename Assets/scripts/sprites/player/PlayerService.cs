using System.Collections.Generic;
using System;
public class PlayerService
{

    public PlayerData playerData;
    public SceneUiManager sceneUiManager;
    public StatusService statusService;
    public PlayerGameObject playerGameObject;
    public PlayerService(SceneUiManager sceneUiManager, StatusService statusService, PlayerGameObject playerGameObject)
    {
        this.sceneUiManager = sceneUiManager;
        this.statusService = statusService;
        this.playerGameObject = playerGameObject;
        this.playerData = GameData.getInstance().playerGameObject.playerData;
    }

    public void initialize()
    {
        playerData.initialize();
        playerGameObject.setPosition();
    }

    public void startFight()
    {
        playerGameObject.statusesObject.removeActiveStatuses();

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


    public void heal(int healAmount)
    {
        playerData.healthBarData.currHealth += healAmount;
        if (playerData.healthBarData.currHealth > playerData.healthBarData.maxHealth)
        {
            playerData.healthBarData.currHealth = playerData.healthBarData.maxHealth;
        }
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
        if (card.data.playerCardData.statuses.Count > 0)
        {
            statusService.addStatuses(playerGameObject.statusesObject, card.data.playerCardData.statuses, null);
        }

        if (card.data.attack > 0)
        {
            GameData.getInstance().playerGameObject.attackAnimation();
        }

        if (card.data.defend > 0)
        {
            GameData.getInstance().playerGameObject.blockAnimation();
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
        if (playerData.blockToLoseEachTurn == -1)
        {
            playerData.healthBarData.currBlock = 0;
        }
        else
        {
            playerData.healthBarData.currBlock = Math.Max(0, playerData.healthBarData.currBlock - playerData.blockToLoseEachTurn);
        }

        statusService.onTurnOver(playerGameObject.statusesObject);
    }
}