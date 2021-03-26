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
        terrorize,
        fortify,
        deflect,
        sword,
        shield,
        shatter,
        hiddenDaggers,
        dagger,
        reload,
        enemy_basic,
        enemy_elite
    }

    public List<CardEnum> obtainableCards;

    public void initialize(StatusTypes statusTypes)
    {
        this.statusTypes = statusTypes;
        obtainableCards = new List<CardTypes.CardEnum>((CardTypes.CardEnum[])Enum.GetValues(typeof(CardTypes.CardEnum)));

        obtainableCards.Remove(CardEnum.smack);
        obtainableCards.Remove(CardEnum.defend);
        obtainableCards.Remove(CardEnum.enemy_basic);
        obtainableCards.Remove(CardEnum.enemy_elite);
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
            actions.getDescriptionAction = () =>
            {
                return "Deal " + actions.getDamageAmount(card) + " damage.";
            };
            data.description = actions.getModifiedDescription(data);

            data.name = "smack";
        }
        else if (cardEnum == CardEnum.defend)
        {
            data.defend = 5;
            data.playerCardData.memoryCount = 1;
            data.playerCardData.energyCost = 1;
            actions.getDescriptionAction = () =>
            {
                return "Gain " + actions.getBlockAmount(card) + " block.";

            };
            data.description = actions.getModifiedDescription(data);

            data.name = "defend";
        }
        else if (cardEnum == CardEnum.haymaker)
        {
            data.attack = 13;
            data.playerCardData.energyCost = 1;
            data.playerCardData.needsTarget = true;
            data.name = "haymaker";

            actions.getDescriptionAction = () =>
            {
                return "Deal " + actions.getDamageAmount(card) + " damage.";

            };
            data.description = actions.getModifiedDescription(data);

        }
        else if (cardEnum == CardEnum.turtle)
        {
            data.defend = 13;
            data.playerCardData.energyCost = 2;
            data.name = "turtle";
            actions.getDescriptionAction = () =>
            {
                return "Gain " + actions.getBlockAmount(card) + " block.";

            };
            data.description = actions.getModifiedDescription(data);
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
            data.defend = 4;
            data.playerCardData.energyCost = 1;
            data.playerCardData.needsTarget = true;

            actions.getDescriptionAction = () =>
            {
                return "Gain " + actions.getBlockAmount(card) + " block" +
                "\n" + "and deal " + actions.getDamageAmount(card) + " damage.";

            };
            data.description = actions.getModifiedDescription(data);

            data.name = "swordNshield";
        }
        else if (cardEnum == CardEnum.firstStrike)
        {
            data.attack = 14;
            data.playerCardData.energyCost = 1;
            data.playerCardData.needsTarget = true;
            data.playerCardData.firstCardPlayed = true;
            actions.getDescriptionAction = () =>
            {
                return "Must be played first." + "\n" + "Deal " + actions.getDamageAmount(card) + " damage.";

            };
            data.description = actions.getModifiedDescription(data);

            data.name = "first strike";
        }
        else if (cardEnum == CardEnum.sweep)
        {
            data.attack = 7;
            data.playerCardData.energyCost = 1;
            data.playerCardData.hitsAll = true;
            actions.getDescriptionAction = () =>
            {
                return "Deal " + actions.getDamageAmount(card) + " damage to all enemies.";

            };
            data.description = actions.getModifiedDescription(data);

            data.name = "sweep";
        }
        else if (cardEnum == CardEnum.followUp)
        {
            data.attack = 9;
            data.cardsToDraw = 1;
            data.playerCardData.energyCost = 1;
            data.playerCardData.needsTarget = true;
            actions.getDescriptionAction = () =>
            {
                return "Deal " + actions.getDamageAmount(card) + " damage and draw " + data.cardsToDraw + " card.";

            };
            data.description = actions.getModifiedDescription(data);

            data.name = "follow up";
        }
        else if (cardEnum == CardEnum.deflect)
        {
            data.cardsToDraw = 1;
            data.defend = 8;
            data.playerCardData.energyCost = 1;
            actions.getDescriptionAction = () =>
            {
                return "Gain " + actions.getBlockAmount(card) + " block and draw " + data.cardsToDraw + " card.";

            };
            data.description = actions.getModifiedDescription(data);

            data.name = "deflect";
        }
        else if (cardEnum == CardEnum.doubleSmack)
        {
            data.attack = 4;
            data.attackMultiplier = 2;
            data.playerCardData.energyCost = 1;
            data.playerCardData.needsTarget = true;
            actions.getDescriptionAction = () =>
            {
                return "Deal " + actions.getDamageAmount(card) + " damage " + data.attackMultiplier + " times.";

            };
            data.description = actions.getModifiedDescription(data);

            data.name = "double smack";
        }
        else if (cardEnum == CardEnum.threaten)
        {
            data.playerCardData.needsTarget = true;
            data.playerCardData.energyCost = 1;
            Status vulnerable = statusTypes.getStatusFromEnum(StatusTypes.StatusEnum.vulnerable);
            data.statuses.Add(vulnerable);

            Status weak = statusTypes.getStatusFromEnum(StatusTypes.StatusEnum.weak);
            data.statuses.Add(weak);

            actions.getDescriptionAction = () =>
            {
                return "Apply " + vulnerable.data.statusCount + " vulnerable and " + weak.data.statusCount + " weak.";

            };
            data.description = actions.getModifiedDescription(data);

            data.name = "threaten";
        }
        else if (cardEnum == CardEnum.terrorize)
        {
            data.playerCardData.hitsAll = true;
            data.exhaust = true;
            data.playerCardData.energyCost = 2;
            Status vulnerable = statusTypes.getStatusFromEnum(StatusTypes.StatusEnum.vulnerable);
            vulnerable.data.statusCount = 2;
            data.statuses.Add(vulnerable);

            Status weak = statusTypes.getStatusFromEnum(StatusTypes.StatusEnum.weak);
            weak.data.statusCount = 2;
            data.statuses.Add(weak);

            actions.getDescriptionAction = () =>
            {
                return "Apply " + vulnerable.data.statusCount + " vulnerable and " + weak.data.statusCount + " weak to all enemies. Exhaust.";

            };
            data.description = actions.getModifiedDescription(data);

            data.name = "terrorize";
        }
        else if (cardEnum == CardEnum.fortify)
        {
            Status armor = statusTypes.getStatusFromEnum(StatusTypes.StatusEnum.armor);
            armor.data.statusDeltaPerTurn = 0;
            armor.data.statusCount = 4;
            data.playerCardData.statuses.Add(armor);

            data.exhaust = true;
            data.playerCardData.energyCost = 1;
            actions.getDescriptionAction = () =>
            {
                return armor.actions.getModifiedDescription(armor.data, null, null) + "\n" + "Removed.";

            };
            data.description = actions.getModifiedDescription(data);

            data.name = "fortify";
        }
        else if (cardEnum == CardEnum.sword)
        {
            data.attack = 5;
            data.playerCardData.needsTarget = true;
            actions.getDescriptionAction = () =>
            {
                return "Deal " + actions.getDamageAmount(card) + " damage.";

            };
            data.description = actions.getModifiedDescription(data);
            data.name = "sword";
        }
        else if (cardEnum == CardEnum.shield)
        {
            data.defend = 3;
            actions.getDescriptionAction = () =>
            {
                return "Gain " + actions.getBlockAmount(card) + " block.";

            };
            data.description = actions.getModifiedDescription(data);
            data.name = "shield";
        }
        else if (cardEnum == CardEnum.shatter)
        {
            data.attack = 12;
            data.exhaust = true;
            data.playerCardData.energyCost = 1;
            data.playerCardData.hitsAll = true;
            actions.getDescriptionAction = () =>
            {
                return "Deal " + actions.getDamageAmount(card) + " damage to all enemies. Exhaust.";

            };
            data.description = actions.getModifiedDescription(data);
            data.name = "shatter";
        }
        else if (cardEnum == CardEnum.hiddenDaggers)
        {
            data.attack = 8;
            data.playerCardData.energyCost = 1;
            data.playerCardData.needsTarget = true;
            actions.getDescriptionAction = () =>
            {
                return "Deal " + actions.getDamageAmount(card) + " damage and add one dagger to your hand.";

            };
            actions.onCardPlayedAction = () =>
            {
                Card newDagger = getCardFromEnum(CardEnum.dagger);
                GameData.getInstance().deckService.addCardToHand(newDagger);
                GameData.getInstance().deckService.deckData.cardsToRemoveAfterFight.Add(newDagger);
            };
            data.description = actions.getModifiedDescription(data);
            data.name = "hiddenDaggers";
        }
        else if (cardEnum == CardEnum.dagger)
        {
            data.attack = 3;
            data.playerCardData.needsTarget = true;
            actions.getDescriptionAction = () =>
            {
                return "Deal " + actions.getDamageAmount(card) + " damage.";

            };
            data.description = actions.getModifiedDescription(data);
            data.name = "dagger";
        }
        else if (cardEnum == CardEnum.reload)
        {
            data.exhaust = true;
            data.playerCardData.needsTarget = false;
            actions.getDescriptionAction = () =>
            {
                return "Move all dagger cards from discard pile to your hand. Exhaust.";

            };
            actions.onCardPlayedAction = () =>
            {
                List<Card> daggerCards = new List<Card>();
                foreach (Card discardCard in GameData.getInstance().deckData.discardCards)
                {
                    if (discardCard.cardEnum == CardEnum.dagger)
                    {
                        daggerCards.Add(discardCard);
                    }
                }

                foreach (Card daggerCard in daggerCards)
                {
                    GameData.getInstance().deckService.addCardToHand(daggerCard);
                    GameData.getInstance().deckService.deckData.discardCards.Remove(daggerCard);
                }
            };
            data.description = actions.getModifiedDescription(data);
            data.name = "reload";
        }
        else if (cardEnum == CardEnum.enemy_basic)
        {
            data.campEventType = CampEventType.basic;
            actions.getDescriptionAction = () =>
            {
                return "Fight 1 enemy, get a card as an reward";

            };
            data.description = actions.getModifiedDescription(data);
            data.name = "basic enemy";
        }
        else if (cardEnum == CardEnum.enemy_elite)
        {
            data.campEventType = CampEventType.elite;
            actions.getDescriptionAction = () =>
            {
                return "Fight 1 elite, get an upgrade as an reward";

            };
            data.description = actions.getModifiedDescription(data);
            data.name = "elite enemy";
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
