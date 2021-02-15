using System.Collections.Generic;
using UnityEngine;

public class DeckState
{

    public List<Card> deckCards = new List<Card>();
    public List<Card> discardCards = new List<Card>();
    public List<Card> hand = new List<Card>();
    public List<Card> cardsToRemoveAfterFight = new List<Card>();

    public void initDeck()
    {
        deckCards = new List<Card>();
        discardCards = new List<Card>();
        hand = new List<Card>();

        for (int i = 0; i < 4; i++)
        {
            if (i < 3)
            {
                deckCards.Add(CardTypes.getBasicAttack());
                deckCards.Add(CardTypes.getBasicDefend());
            }
            else if (i == 3)
            {
                deckCards.Add(CardTypes.getBetterAttack());
                deckCards.Add(CardTypes.getBetterDefense());
                deckCards.Add(CardTypes.getDrawCard());
            }
            else
            {
                float random = Random.Range(0, 2);
                if (random > 1)
                {
                    deckCards.Add(CardTypes.getBetterAttack());
                }
                else
                {
                    deckCards.Add(CardTypes.getBetterDefense());
                }
            }
        }
    }

    public void startFight()
    {
        drawNewHand();
    }

    public void endTurn()
    {
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

    public Card drawCard()
    {
        Card drawnCard = randomCardFromDeck();
        if (drawnCard != null)
        {
            hand.Add(drawnCard);
        }
        return drawnCard;
    }

    public void shuffleDiscardIntoDeck()
    {
        while (discardCards.Count > 0)
        {
            int index = Random.Range(0, discardCards.Count);
            Card card = discardCards[index];
            discardCards.RemoveAt(index);
            deckCards.Add(card);
        }
    }
    public Card randomCardFromDeck()
    {
        if (deckCards.Count == 0)
        {
            shuffleDiscardIntoDeck();
        }
        if (deckCards.Count == 0)
        {
            //no cards to draw
            return null;
        }

        int index = Random.Range(0, deckCards.Count);
        Card drawnCard = deckCards[index];
        deckCards.RemoveAt(index);
        return drawnCard;
    }

    public void discardHand()
    {
        List<Card> handCopy = new List<Card>(hand);
        foreach (Card card in handCopy)
        {
            discardCard(card);
        }
    }

    public void playCard(Card card)
    {
        FightManagerService fightManagerService = FightManagerService.getInstance();

        for (int i = 0; i < card.cardsToDraw; i++)
        {
            Card drawnCard = randomCardFromDeck();
            if (drawnCard != null)
            {
                hand.Add(drawnCard);
                fightManagerService.cardDrawn(drawnCard);
            }
        }
        discardCard(card);
    }

    private void discardCard(Card card)
    {
        hand.Remove(card);
        discardCards.Add(card);
        card.onDiscard();
    }

    public List<Card> generateCards(int numCardsToGenerate)
    {
        List<Card> cards = new List<Card>();
        cards.Add(CardTypes.getBetterAttack());
        cards.Add(CardTypes.getBetterDefense());
        cards.Add(CardTypes.getDrawCard());
        return cards;
    }

    public void addCardToDeck(Card card)
    {
        deckCards.Add(card);
        if (card.isEnemycard)
        {
            cardsToRemoveAfterFight.Add(card);
        }
    }

    public void onFightEnd()
    {
        foreach (Card cardToRemove in cardsToRemoveAfterFight)
        {
            deckCards.Remove(cardToRemove);
        }
    }
}
