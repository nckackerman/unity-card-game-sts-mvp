using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DeckState
{

    public static List<Card> deckCards = new List<Card>();
    public static List<Card> discardCards = new List<Card>();
    public static List<Card> hand = new List<Card>();

    private static Card drawCard = new Card();
    private static Card betterDefense = new Card();
    private static Card betterAttack = new Card();
    private static Card basicDefense = new Card();
    private static Card basicAttack = new Card();

    public static void initDeck()
    {
        deckCards = new List<Card>();
        discardCards = new List<Card>();
        hand = new List<Card>();
        UiManager.destroyPlayerCardUi();

        basicAttack.attack = 5;
        basicAttack.energyCost = 1;
        basicDefense.defend = 5;
        basicDefense.energyCost = 1;
        betterAttack.attack = 13;
        betterAttack.energyCost = 2;

        betterDefense.defend = 13;
        betterDefense.energyCost = 2;
        drawCard.cardsToDraw = 2;
        drawCard.energyCost = 1;
        for (int i = 0; i < 7; i++)
        {
            if (i < 3)
            {
                deckCards.Add(basicAttack);
                deckCards.Add(basicDefense);
            }
            else if (i == 3)
            {
                deckCards.Add(betterAttack);
                deckCards.Add(betterDefense);
                deckCards.Add(drawCard);
            }
            else
            {
                float random = Random.Range(0, 2);
                if (random > 1)
                {
                    deckCards.Add(betterAttack);
                }
                else
                {
                    deckCards.Add(betterDefense);
                }
            }
        }
    }

    public static void drawNewHand()
    {

        for (int i = 0; i < 5; i++)
        {
            Card drawnCard = RandomCardFromDeck();
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
    public static Card RandomCardFromDeck()
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
        cards.Add(drawCard);
        cards.Add(betterAttack);
        cards.Add(betterDefense);
        return cards;
    }

    public static void addCardToDeck(Card card)
    {
        deckCards.Add(card);
    }
}
