using System.Collections.Generic;

public class DeckData
{

    public int maxHandSize = 10;
    public List<Card> deckCards = new List<Card>();
    public List<Card> discardCards = new List<Card>();
    public List<Card> hand = new List<Card>();
    public List<Card> trash = new List<Card>();
    public List<Card> powerCards = new List<Card>();

    public void initDeck(CardTypes cardTypes)
    {
        deckCards.Add(cardTypes.getCardFromEnum(CardTypes.CardEnum.threaten));
        deckCards.Add(cardTypes.getCardFromEnum(CardTypes.CardEnum.doubleSmack));
        deckCards.Add(cardTypes.getCardFromEnum(CardTypes.CardEnum.turtle));
        for (int i = 0; i < 3; i++)
        {
            deckCards.Add(cardTypes.getCardFromEnum(CardTypes.CardEnum.smack));
            deckCards.Add(cardTypes.getCardFromEnum(CardTypes.CardEnum.defend));
        }
        discardCards = new List<Card>();
        hand = new List<Card>();
    }
}
