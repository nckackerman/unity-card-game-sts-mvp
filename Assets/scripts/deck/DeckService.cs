using System.Collections.Generic;
using UnityEngine;

public class DeckService
{
    public List<Card> cardsPlayedThisTurn = new List<Card>();

    public DeckData deckData;
    public CardUiManager cardUiManager;
    public PlayerService playerService;
    public EnemyManagerService enemyManagerService;

    public DeckService(DeckData deckData, CardUiManager cardUiManager, PlayerService playerService)
    {
        this.deckData = deckData;
        this.cardUiManager = cardUiManager;
        this.playerService = playerService;
    }

    public void initialize(EnemyManagerService enemyManagerService)
    {
        this.enemyManagerService = enemyManagerService;
    }

    public void initDeck(CardTypes cardTypes)
    {
        deckData.deckCards.Add(cardTypes.getCardFromEnum(CardTypes.CardEnum.shield));
        deckData.deckCards.Add(cardTypes.getCardFromEnum(CardTypes.CardEnum.threaten));
        for (int i = 0; i < 4; i++)
        {
            deckData.deckCards.Add(cardTypes.getCardFromEnum(CardTypes.CardEnum.smack));
            deckData.deckCards.Add(cardTypes.getCardFromEnum(CardTypes.CardEnum.defend));
        }
        deckData.discardCards = new List<Card>();
        deckData.hand = new List<Card>();
    }

    //Shouldnt be called directly (normally). This should only be called from cardService.onCardPlayed();
    public void onCardPlayed(Card card)
    {
        deckData.hand.Remove(card);
        cardsPlayedThisTurn.Add(card);

        for (int i = 0; i < card.data.cardsToDraw; i++)
        {
            drawCard();
        }

        if (card.data.exhaust)
        {
            trashCard(card);
        }
        else if (card.data.playerCardData.statuses.Count > 0)
        {
            deckData.hand.Remove(card);
            deckData.powerCards.Add(card);
        }
        else
        {
            discardCard(card);
        }
    }

    public bool isCardPlayable(Card card)
    {
        if (card.data.playerCardData.firstCardPlayed && cardsPlayedThisTurn.Count != 0)
        {
            return false;
        }
        return true;
    }

    public void startFight()
    {
        cardUiManager.destroyPlayerHandUi();
        cardsPlayedThisTurn = new List<Card>();
        drawNewHand();
    }

    public void endTurn()
    {
        cardsPlayedThisTurn = new List<Card>();
        discardHand();
        drawNewHand();
    }

    public void drawNewHand()
    {
        for (int i = 0; i < 5; i++)
        {
            drawCard();
        }
    }

    public bool canDrawCard()
    {
        return deckData.hand.Count < deckData.maxHandSize;
    }

    public Card drawCard()
    {
        if (!canDrawCard())
        {
            return null;
        }
        Card drawnCard = randomCardFromDeck();
        if (drawnCard != null)
        {
            drawnCard.actions.onCardDrawn();
            if (drawnCard.data.isEnemycard)
            {
                enemyManagerService.onEnemyCardDrawn(drawnCard);
            }
            addCardToHand(drawnCard);
        }
        return drawnCard;
    }

    public void shuffleDiscardIntoDeck()
    {
        while (deckData.discardCards.Count > 0)
        {
            int index = Random.Range(0, deckData.discardCards.Count);
            Card card = deckData.discardCards[index];
            deckData.discardCards.RemoveAt(index);
            deckData.deckCards.Add(card);
        }
    }
    public Card randomCardFromDeck()
    {
        if (deckData.deckCards.Count == 0)
        {
            shuffleDiscardIntoDeck();
        }
        if (deckData.deckCards.Count == 0)
        {
            //no cards to draw
            return null;
        }

        int index = Random.Range(0, deckData.deckCards.Count);
        Card card = deckData.deckCards[index];
        deckData.deckCards.RemoveAt(index);
        return card;
    }

    public void discardHand()
    {
        List<Card> handCopy = new List<Card>(deckData.hand);
        foreach (Card card in handCopy)
        {
            discardCard(card);
        }
    }

    private void discardCard(Card card)
    {
        deckData.hand.Remove(card);
        deckData.discardCards.Add(card);
        foreach (CardMod cardMod in card.data.cardMods)
        {
            cardMod.onDiscard();
        }
        card.data.cardMods = new List<CardMod>();
    }

    private void trashCard(Card card)
    {
        deckData.hand.Remove(card);
        deckData.trash.Add(card);
    }

    public void addCardToDeck(Card card)
    {
        deckData.deckCards.Add(card);
        if (card.data.isEnemycard)
        {
            deckData.cardsToRemoveAfterFight.Add(card);
        }
    }

    public void addCardToHand(Card card)
    {
        deckData.hand.Add(card);
        cardUiManager.showCardInHand(card, deckData.hand.Count);
    }

    public void onFightEnd()
    {
        foreach (Card cardToRemove in deckData.cardsToRemoveAfterFight)
        {
            deckData.deckCards.Remove(cardToRemove);
        }

        foreach (Card trashCard in deckData.trash)
        {
            deckData.deckCards.Add(trashCard);
        }

        foreach (Card powerCard in deckData.powerCards)
        {
            deckData.deckCards.Add(powerCard);
        }
        deckData.trash = new List<Card>();
    }

    public void extraDraw()
    {
        if (playerService.canExtraDraw() && canDrawCard())
        {
            playerService.onExtraDraw();
            Card drawnCard = drawCard();
            if (drawnCard != null && drawnCard.data.playerCardData.energyCost > 0)
            {
                CardMod cardMod = new CardMod();
                cardMod.onDiscardAction = () =>
                {
                    drawnCard.data.playerCardData.energyCost++;
                };
                drawnCard.data.cardMods.Add(cardMod);
                drawnCard.data.playerCardData.energyCost--;
            }
        }
    }
}