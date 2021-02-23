using System.Collections.Generic;
using UnityEngine;
using System;

public class CardTypes
{

    private StatusTypes statusTypes;

    public enum CardEnum
    {
        smack,
        defend,
        haymaker,
        turtle,
        search,
        swordAndShield,
        firstStrike,
        sweep,
        followUp,
        doubleSmack,
        threaten,
        fortify,
    }

    public List<CardEnum> obtainableCards;

    public void initialize(StatusTypes statusTypes)
    {
        this.statusTypes = statusTypes;
        obtainableCards = new List<CardTypes.CardEnum>((CardTypes.CardEnum[])Enum.GetValues(typeof(CardTypes.CardEnum)));
        obtainableCards.Remove(CardEnum.smack);
        obtainableCards.Remove(CardEnum.defend);
    }

    public Card getCardFromEnum(CardEnum cardEnum)
    {
        Card card = new Card(cardEnum);
        CardData data = new CardData();
        CardActions actions = new CardActions();
        if (cardEnum == CardEnum.smack)
        {
            data.attack = 5;
            data.playerCardData.needsTarget = true;
            data.playerCardData.memoryCount = 1;
            data.playerCardData.energyCost = 1;
            actions.getDescriptionAction = (EnemyData enemyData, PlayerData playerData) =>
            {
                return "Deal " + actions.getDamageAmount(data.attack, enemyData, playerData) + " damage.";
            };
            data.description = actions.getModifiedDescription(null, null, data);

            data.name = "smack";
        }
        else if (cardEnum == CardEnum.defend)
        {
            data.defend = 5;
            data.playerCardData.memoryCount = 1;
            data.playerCardData.energyCost = 1;
            actions.getDescriptionAction = (EnemyData enemyData, PlayerData playerData) =>
            {
                return "Gain " + actions.getBlockAmount(data.defend, playerData) + " block.";

            };
            data.description = actions.getModifiedDescription(null, null, data);

            data.name = "defend";
        }
        else if (cardEnum == CardEnum.haymaker)
        {
            data.attack = 13;
            data.playerCardData.energyCost = 1;
            data.playerCardData.needsTarget = true;
            data.name = "haymaker";

            actions.getDescriptionAction = (EnemyData enemyData, PlayerData playerData) =>
            {
                return "Deal " + actions.getDamageAmount(data.attack, enemyData, playerData) + " damage.";

            };
            data.description = actions.getModifiedDescription(null, null, data);

        }
        else if (cardEnum == CardEnum.turtle)
        {
            data.defend = 13;
            data.name = "turtle";
            actions.getDescriptionAction = (EnemyData enemyData, PlayerData playerData) =>
            {
                return "Gain " + actions.getBlockAmount(data.defend, playerData) + " block.";

            };
            data.description = actions.getModifiedDescription(null, null, data);
        }
        else if (cardEnum == CardEnum.search)
        {
            data.cardsToDraw = 2;
            data.description = "Draw " + data.cardsToDraw + " cards.";
            data.name = "search";
        }
        else if (cardEnum == CardEnum.swordAndShield)
        {
            data.attack = 7;
            data.defend = 3;
            data.playerCardData.energyCost = 1;
            data.playerCardData.needsTarget = true;

            actions.getDescriptionAction = (EnemyData enemyData, PlayerData playerData) =>
            {
                return "Gain " + actions.getBlockAmount(data.defend, playerData) + " block" +
                "\n" + "and deal " + actions.getDamageAmount(data.attack, enemyData, playerData) + " damage.";

            };
            data.description = actions.getModifiedDescription(null, null, data);

            data.name = "swordNshield";
        }
        else if (cardEnum == CardEnum.firstStrike)
        {
            data.attack = 14;
            data.playerCardData.energyCost = 1;
            data.playerCardData.needsTarget = true;
            data.playerCardData.firstCardPlayed = true;
            actions.getDescriptionAction = (EnemyData enemyData, PlayerData playerData) =>
            {
                return "Must be played first." + "\n" + "Deal " + actions.getDamageAmount(data.attack, enemyData, playerData) + " damage.";

            };
            data.description = actions.getModifiedDescription(null, null, data);

            data.name = "first strike";
        }
        else if (cardEnum == CardEnum.sweep)
        {
            data.attack = 7;
            data.playerCardData.energyCost = 1;
            data.playerCardData.hitsAll = true;
            actions.getDescriptionAction = (EnemyData enemyData, PlayerData playerData) =>
            {
                return "Deal " + actions.getDamageAmount(data.attack, null, null) + " damage to all enemies.";

            };
            data.description = actions.getModifiedDescription(null, null, data);

            data.name = "sweep";
        }
        else if (cardEnum == CardEnum.followUp)
        {
            data.attack = 9;
            data.cardsToDraw = 1;
            data.playerCardData.energyCost = 1;
            data.playerCardData.needsTarget = true;
            actions.getDescriptionAction = (EnemyData enemyData, PlayerData playerData) =>
            {
                return "Deal " + actions.getDamageAmount(data.attack, enemyData, playerData) + " damage and draw " + data.cardsToDraw + " card.";

            };
            data.description = actions.getModifiedDescription(null, null, data);

            data.name = "follow up";
        }
        else if (cardEnum == CardEnum.doubleSmack)
        {
            data.attack = 4;
            data.attackMultiplier = 2;
            data.playerCardData.energyCost = 1;
            data.playerCardData.needsTarget = true;
            actions.getDescriptionAction = (EnemyData enemyData, PlayerData playerData) =>
            {
                return "Deal " + actions.getDamageAmount(data.attack, enemyData, playerData) + " damage " + data.attackMultiplier + " times.";

            };
            data.description = actions.getModifiedDescription(null, null, data);

            data.name = "double smack";
        }
        else if (cardEnum == CardEnum.threaten)
        {
            data.exhaust = true;
            data.playerCardData.hitsAll = true;
            data.playerCardData.energyCost = 1;
            Status vulnerable = statusTypes.getStatusFromEnum(StatusTypes.StatusEnum.vulnerable);
            data.statuses.Add(vulnerable);

            Status weak = statusTypes.getStatusFromEnum(StatusTypes.StatusEnum.weak);
            data.statuses.Add(weak);

            actions.getDescriptionAction = (EnemyData enemyData, PlayerData playerData) =>
            {
                return "Apply " + vulnerable.data.statusCount + " vulnerable and " + weak.data.statusCount + " weak. Exhaust.";

            };
            data.description = actions.getModifiedDescription(null, null, data);

            data.name = "threaten";
        }
        else if (cardEnum == CardEnum.fortify)
        {
            int defendAmount = 4;
            data.playerCardData.energyCost = 1;
            actions.getDescriptionAction = (EnemyData enemyData, PlayerData playerData) =>
            {
                return "Gain " + defendAmount + " block at the start of each turn. Removed.";

            };
            data.description = actions.getModifiedDescription(null, null, data);
            CardPower cardPower = new CardPower();
            cardPower.onTurnOverAction = (PlayerData playerState) =>
            {
                playerState.healthBarData.currBlock += defendAmount;
            };
            data.playerCardData.cardPower = cardPower;

            data.name = "fortify";
        }
        else
        {
            throw new System.Exception("invalid status enum provided: " + cardEnum);
        }
        card.data = data;
        card.actions = actions;
        return card;

    }
}
