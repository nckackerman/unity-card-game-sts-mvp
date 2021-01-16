using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DeckState
{

    public static List<Card> deckCards = new List<Card>();
    public static List<Card> discardCards = new List<Card>();
    public static List<Card> hand = new List<Card>();

    public static void initDeck()
    {
        deckCards = new List<Card>();
        discardCards = new List<Card>();
        hand = new List<Card>();
        UiManager.destroyPlayerCardUi();

        for (int i = 0; i < 7; i++)
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

    public static void drawNewHand()
    {

        for (int i = 0; i < 5; i++)
        {
            Card drawnCard = randomCardFromDeck();
            if (drawnCard != null)
            {
                hand.Add(drawnCard);
            }
        }

        UiManager.showHand();

    }

    public static void shuffleDiscardIntoDeck()
    {
        while (discardCards.Count > 0)
        {
            int index = Random.Range(0, discardCards.Count);
            Card card = discardCards[index];
            discardCards.RemoveAt(index);
            deckCards.Add(card);
        }
    }
    public static Card randomCardFromDeck()
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

    public static void discardHand(bool drawNew)
    {
        UiManager.destroyPlayerCardUi();
        for (int i = 0; i < hand.Count; i++)
        {
            Card card = hand[i];
            discardCards.Add(card);
        }

        hand = new List<Card>();
        if (drawNew)
        {
            drawNewHand();
        }
    }

    public static void playCard(Card card)
    {
        discardCards.Add(card);
        hand.Remove(card);
    }

    public static List<Card> generateCards(int numCardsToGenerate)
    {
        List<Card> cards = new List<Card>();
        cards.Add(CardTypes.getBetterAttack());
        cards.Add(CardTypes.getBetterDefense());
        cards.Add(CardTypes.getDrawCard());
        return cards;
    }

    public static void addCardToDeck(Card card)
    {
        deckCards.Add(card);
    }
}
